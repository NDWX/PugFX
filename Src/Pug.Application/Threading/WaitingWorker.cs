using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Pug.Application.Threading
{
#if !NET35
	public delegate R Func<T, R>(T taskObject);
#endif

	//public delegate T WorkWait<T>();

	public class WaitingWorker<I,T,R> : IDisposable
	{
		public delegate void EventHandler(WaitingWorker<I, T, R> worker);
		public delegate void TaskEventHandler(T task, WaitingWorker<I, T, R> worker);
		public delegate void TaskCompletedEventHandler(T task, R result, WaitingWorker<I, T, R> worker);
		public delegate void TaskErrorEventHandler(T task, Exception error, WaitingWorker<I, T, R> worker);
#if TRACE
		static TraceSwitch traceSwitch = new TraceSwitch("Pug.Application.Threading.WaitingWorker", "WaitingWorker trace switch");
#endif
		I identifier;

		//Action setup, cleanup;
		IWorkerTaskWait<T> workWait;
		Func<T, R> work;

		Thread workerThread;
		EventWaitHandle workingWaitHandle;

		bool isWaiting;
		bool isWorking;
		bool isDisposing, isDisposed;
		
		public WaitingWorker(I identifier, IWorkerTaskWait<T> workWait, Func<T, R> worker)
		{
			if (identifier == null)
				throw new ArgumentNullException("identifier");
				
			if( workWait == null )
				throw new ArgumentNullException("workWait");

			if (worker == null)
				throw new ArgumentNullException("worker");

			this.identifier = identifier;

			this.workWait = workWait;
			this.work = worker;

			workingWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
		}

		public EventHandler Ready, Finishing;

		public TaskEventHandler OnStartingTask;
		public TaskCompletedEventHandler OnTaskCompleted;

		public TaskErrorEventHandler OnTaskError;

		void WaitForWork()
		{
			workingWaitHandle.Reset();

			T task;
			R result;

			if (Ready != null)
			{
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying READY for work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				Ready(this);
			}

			isWaiting = true;
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} waiting for work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			while (workWait.HasTasks || !isDisposing)
			{
				task = workWait.GetNextTask(200);

				if (task != null)
				{
#if TRACE
					Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} work received", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
					if (OnStartingTask != null)
					{
#if TRACE
						Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying STARTING work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
						OnStartingTask(task, this);
					}

					result = DoWork(task);

					if (OnTaskCompleted != null)
					{
#if TRACE
						Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying FINISHED work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
						OnTaskCompleted(task, result, this);
					}
				}
			}

			isWaiting = false;

			if (Finishing != null)
			{ 
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying FINISHING", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				Finishing(this);
			}

			//if( cleanup != null )
			//    cleanup();

			workingWaitHandle.Set();
		}

		R DoWork(T task)
		{
			isWorking = true;
			
			R result = default(R);
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} is doing work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			try
			{
				result = work(task);
			}
			catch(Exception exception)
			{
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} encountered error while doing work : {3}", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s"), exception.Message));
#endif
				if( OnTaskError != null )
					OnTaskError(task, exception, this);
			}
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} has finished working", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			isWorking = false;

			return result;
		}

//        void SetUpAndWaitForWork()
//        {
//#if TRACE
//            Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} is setting up", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//            setup();
//#if TRACE
//            Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} is ready for work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//            WaitForWork();
//        }

		public void Start()
		{
			workerThread = new Thread(new ThreadStart(WaitForWork));
			workerThread.Start();
		}

		//public void Start(Action setup)
		//{
		//    if( setup == null )
		//        throw new ArgumentNullException("setup");

		//    this.setup = setup;
		//    workerThread = new Thread(new ThreadStart(SetUpAndWaitForWork));
		//    workerThread.Start();
		//}

		public bool IsWorking
		{
			get
			{
				return isWorking;
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			isDisposing = true;

#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} waiting for all work to finish before disposing", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif

			workingWaitHandle.WaitOne();
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} has finished all works and is ready to dispose", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			try
			{
				workingWaitHandle.Close();
			}
			catch
			{
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceWarning, string.Format("[{2}] WaitingWorker: {0}@{1} encounter error when trying to close working event handle on dispose", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				throw;
			}
			finally
			{
				isDisposed = true;
			}

		}

		//public void Dispose(Action cleanUp)
		//{
		//    this.cleanup = cleanUp;
		//    Dispose();
		//}

		#endregion
	}
}