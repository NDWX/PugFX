using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Pug.Application.Threading
{
	public class WorkerTaskQueue<T> 
		: Pug.Application.Threading.IWorkerTaskWait<T>
	{
#if TRACE
		static TraceSwitch traceSwitch = new TraceSwitch("Pug.Application.Threading.WorkerTaskQueue", "WorkerTaskQueue trace switch");
#endif
		Queue<T> taskQueue;
		EventWaitHandle taskWaitHandle;

		public WorkerTaskQueue()
		{
			taskWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

			taskQueue = new Queue<T>();
		}

		public bool HasTasks
		{
			get
			{
				return taskQueue.Count > 0;
			}
		}

		public T GetNextTask()
		{
			return GetNextTask(0);
		}

		public T GetNextTask(int waitTimeout)
		{
			bool taskReceived;
			
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread: {0} asking for new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
#if DEBUG
			Debug.WriteLine(string.Format("[{2}] Thread: {0} There is currently {1} task in the queue", taskQueue.Count, DateTime.Now.ToString("s")));
#endif
			if (taskQueue.Count == 0)
			{
				taskWaitHandle.Reset();
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread: {0} has no new task, waiting . . .", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
				if( waitTimeout > 0 )
					taskReceived = taskWaitHandle.WaitOne(waitTimeout);
				else
					taskReceived = taskWaitHandle.WaitOne();
#if TRACE
				Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread: {0} new received new task signal", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			}
			
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread: {0} dequeuing  new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			T task = default(T);

			if( taskQueue.Count > 0 )
				task = taskQueue.Dequeue();
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread: {0} obtained new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			return task;
		}

		public void Enqueue(T task)
		{
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread: {0} enqueuing new task", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
			taskQueue.Enqueue(task);
			taskWaitHandle.Set();            
#if TRACE
			Trace.WriteLineIf(traceSwitch.TraceInfo, string.Format("[{1}] Thread: {0} enqueued new task and new task event is signalled", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("s")));
#endif
		}
	}
}
