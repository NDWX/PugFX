using System;
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
		private static readonly TraceSwitch _traceSwitch = new TraceSwitch("Pug.Application.Threading.WaitingWorker", "WaitingWorker trace switch");
#endif
		private readonly I _identifier;

		private readonly object _disposeSync = new object();
		private readonly object _startSync = new object();

		private readonly IEventListener _eventListener;
		private readonly TaskSourceWaitableAdapter<T> _taskSource;
		private readonly IWorker _worker;

		private Thread _workerThread;
		private readonly EventWaitHandle _taskWait;
		private readonly EventWaitHandle _workingWaitHandle;

		private bool _isWorking;
		private bool _isDisposing;
		
		public WaitingWorker(I identifier, IWorkerTaskSource<T> workWait, IWorker worker, IEventListener eventListener)
		{
			if (identifier == null)
				throw new ArgumentNullException("identifier");
				
			if( workWait == null )
				throw new ArgumentNullException("workWait");

			if (worker == null)
				throw new ArgumentNullException("worker");

			this._identifier = identifier;

			_taskSource = new TaskSourceWaitableAdapter<T>(workWait);
			this._worker = worker;
			this._eventListener = eventListener;

			_taskWait = new EventWaitHandle(false, EventResetMode.AutoReset);
			_workingWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
		}

		public EventHandler Ready, Finishing;

		private R DoWork(T task)
		{
			_isWorking = true;

			R result = default(R);
#if TRACE
			Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} is doing work", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			try
			{
				result = _worker.DoWork(task);
			}
			catch (Exception exception)
			{
#if TRACE
				Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} encountered error while doing work : {3}", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s"), exception.Message));
#endif
				if (_eventListener != null)
					_eventListener.TaskError(task, exception, this);
			}
#if TRACE
			Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} has finished working", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			_isWorking = false;

			return result;
		}

		private void WaitForWork()
		{
			_workingWaitHandle.Reset();

			//T task;
			R result;

			if (_eventListener != null)
			{
#if TRACE
				Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying READY for work", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				_eventListener.Ready(this);
			}

#if TRACE
			Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} waiting for work", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif

			NonBindingWait<T> waiter = new NonBindingWait<T>(_taskSource);

			while( !_isDisposing)
			{
				waiter.WaitAndNotify(0, _taskWait);

				_taskWait.WaitOne();

				if (!_isDisposing && waiter.TaskReceived)
				{
#if TRACE
					Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} work received", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
					if (_eventListener != null)
					{
#if TRACE
						Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying STARTING work", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
						_eventListener.StartingTask(waiter.Task, this);
					}

					result = DoWork(waiter.Task);

					if (_eventListener != null)
					{
#if TRACE
						Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying FINISHED work", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
						_eventListener.TaskCompleted(waiter.Task, result, this);
					}
				}
					
			}

			_taskWait.Dispose();

			if (Finishing != null)
			{ 
#if TRACE
				Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} notifying FINISHING", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				_eventListener.Finishing(this);
			}

			_workingWaitHandle.Set();
		}

		public void Start()
		{
			lock(_startSync)
			{
				if (_workerThread != null)
					return;

				_workerThread = new Thread(new ThreadStart(WaitForWork));
				_workerThread.Start();
			}
		}

		public bool IsWorking
		{
			get
			{
				return _isWorking;
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			lock( _disposeSync)
			{
				_isDisposing = true;

				// stop waiting for task;
				_taskWait.Set();

#if TRACE
				Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} waiting for all work to finish before disposing", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif

				_workingWaitHandle.WaitOne();
#if TRACE
				Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{2}] WaitingWorker: {0}@{1} has finished all works and is ready to dispose", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				try
				{
					_workingWaitHandle.Dispose();
				}
				catch
				{
#if TRACE
					Trace.WriteLineIf(_traceSwitch.TraceWarning, string.Format("[{2}] WaitingWorker: {0}@{1} encounter error when trying to close working event handle on dispose", _identifier, Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
					throw;
				}
				finally
				{
				}
			}

		}

		#endregion
	}
}