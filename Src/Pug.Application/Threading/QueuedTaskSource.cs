using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Pug.Application.Threading
{
	public class QueuedTaskSource<T> 
		: IWorkerTaskSource<T>, IDisposable
	{
#if TRACE
		private static readonly TraceSwitch _traceSwitch = new TraceSwitch("Pug.Application.Threading.WorkerTaskQueue", "WorkerTaskQueue trace switch");
#endif
		private readonly Queue<T> _taskQueue;
		private readonly EventWaitHandle _taskWaitHandle;

		public QueuedTaskSource()
		{
			_taskWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

			_taskQueue = new Queue<T>();
		}

		public bool HasTasks
		{
			get
			{
				return _taskQueue.Count > 0;
			}
		}

		public void Enqueue(T task)
		{
#if TRACE
			Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : enqueuing new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			_taskQueue.Enqueue(task);
			_taskWaitHandle.Set();
#if TRACE
			Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : enqueued new task and new task event is signalled", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
		}

		public bool GetNextTask(ref T task)
		{
			return GetNextTask(0, ref task);
		}

		private readonly object _taskDequeueSync = new object();

		public bool GetNextTask(int waitTimeout, ref T task)
		{
			bool taskReceived = true;
			task = default(T);

#if TRACE
			Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : asking for new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif

			lock(_taskDequeueSync)
			{
				//taskWaitHandle.Reset();

				if (_taskQueue.Count == 0)
				{
#if TRACE
					Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : has no new task, waiting . . .", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
					if (waitTimeout > 0)
						taskReceived = _taskWaitHandle.WaitOne(waitTimeout);
					else
						taskReceived = _taskWaitHandle.WaitOne();
#if TRACE
					Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : new received new task signal", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				}

#if TRACE
				Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : dequeuing  new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				//if( taskQueue.Count > 0 )
				if( taskReceived )
					task = _taskQueue.Dequeue();
#if TRACE
				Trace.WriteLineIf(_traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : obtained new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			}

			return taskReceived;
		}

//		public void GetNextTask(ResourceWaitContext<T> context)
//		{
//			bool taskReceived = true;
//			T task = default(T);

//#if TRACE
//			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : asking for new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//#if DEBUG
//			Debug.WriteLine(string.Format("[{2}] Thread {0} : There is currently {1} task in the queue", taskQueue.Count, DateTime.Now.ToString("s")));
//#endif

//			lock (taskDequeueSync)
//			{
//				taskWaitHandle.Reset();

//				if (taskQueue.Count == 0)
//				{
//#if TRACE
//					Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : has no new task, waiting . . .", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//					if (context.WaitTimeout > 0)
//						taskReceived = taskWaitHandle.WaitOne(context.WaitTimeout);
//					else
//						taskReceived = taskWaitHandle.WaitOne();
//#if TRACE
//					Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : new received new task signal", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//				}

//#if TRACE
//				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : dequeuing  new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//				//if( taskQueue.Count > 0 )
//				if (taskReceived)
//				{
//#if TRACE
//					Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : obtained new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//					task = taskQueue.Peek();

//#if TRACE
//					Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : offering new task...", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//					if ( context.Set(task) )
//					{
//						taskQueue.Dequeue();
//#if TRACE
//						Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : task accepted.", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//#endif
//					}
//#if TRACE
//					else
//					{
//						Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread {0} : Wait timed out, task left in queue.", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
//					}
//#endif
//				}
//			}
//		}

		public void Dispose()
		{
			_taskWaitHandle.Dispose();
		}
	}
}
