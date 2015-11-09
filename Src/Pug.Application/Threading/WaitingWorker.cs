using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Pug.Application.Threading
{
	//#if !NET35
	//	public delegate R Func<T, R>(T taskObject);
	//#endif

	//public delegate T WorkWait<T>();


	public class WaitingWorker<I,T,R> : IDisposable
	{
		public interface IEventListener
		{
			void Ready(WaitingWorker<I, T, R> waitingWorker);

			void StartingTask(T task, WaitingWorker<I, T, R> waitingWorker);

			void TaskError(T task, Exception error, WaitingWorker<I, T, R> waitingWorker);

			void TaskCompleted(T task, R result, WaitingWorker<I, T, R> waitingWorker);

			void Finishing(WaitingWorker<I, T, R> waitingWorker);
		}

		public interface IWorker
		{
			R DoWork(T task);
		}

#if TRACE
		static TraceSwitch traceSwitch = new TraceSwitch("Pug.Application.Threading.WaitingWorker", "WaitingWorker trace switch");
#endif
		I identifier;

		object disposeSync = new object();
		object startSync = new object();
		
		IEventListener eventListener;
		TaskSourceWaitableAdapter<T> taskSource;
		IWorker worker;

		Thread workerThread;
		EventWaitHandle taskWait;
        EventWaitHandle workingWaitHandle;

		bool isWaiting;
		bool isWorking;
		bool isDisposing, isDisposed;
		
		public WaitingWorker(I identifier, IWorkerTaskSource<T> workWait, IWorker worker, IEventListener eventListener)
		{
			if (identifier == null)
				throw new ArgumentNullException("identifier");
				
			if( workWait == null )
				throw new ArgumentNullException("workWait");

			if (worker == null)
				throw new ArgumentNullException("worker");

			this.identifier = identifier;

			this.taskSource = new TaskSourceWaitableAdapter<T>(workWait);
			this.worker = worker;
			this.eventListener = eventListener;

			taskWait = new EventWaitHandle(false, EventResetMode.AutoReset);
			workingWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
		}

		public EventHandler Ready, Finishing;

		R DoWork(T task)
		{
			isWorking = true;

			R result = default(R);
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} is doing work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			try
			{
				result = worker.DoWork(task);
			}
			catch (Exception exception)
			{
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} encountered error while doing work : {3}", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s"), exception.Message));
#endif
				if (eventListener != null)
					eventListener.TaskError(task, exception, this);
			}
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} has finished working", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			isWorking = false;

			return result;
		}

		void WaitForWork()
		{
			workingWaitHandle.Reset();

			//T task;
			R result;

			if (eventListener != null)
			{
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying READY for work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				eventListener.Ready(this);
			}

			isWaiting = true;
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} waiting for work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif

			NonBindingWait<T> waiter = new NonBindingWait<T>(taskSource);

			while( !isDisposing)
			{
				waiter.WaitAndNotify(0, taskWait);

				taskWait.WaitOne();

				if (!isDisposing && waiter.TaskReceived)
				{
#if TRACE
					Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} work received", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
					if (eventListener != null)
					{
#if TRACE
						Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying STARTING work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
						eventListener.StartingTask(waiter.Task, this);
					}

					result = DoWork(waiter.Task);

					if (eventListener != null)
					{
#if TRACE
						Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying FINISHED work", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
						eventListener.TaskCompleted(waiter.Task, result, this);
					}
				}
					
			}

			taskWait.Close();

			isWaiting = false;

			if (Finishing != null)
			{ 
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying FINISHING", identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				eventListener.Finishing(this);
			}

			workingWaitHandle.Set();
		}

		public void Start()
		{
			lock(startSync)
			{
				if (workerThread != null)
					return;

				workerThread = new Thread(new ThreadStart(WaitForWork));
				workerThread.Start();
			}
		}

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
			lock( disposeSync)
			{
				isDisposing = true;

				// stop waiting for task;
				taskWait.Set();

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

		}

		#endregion
	}
}