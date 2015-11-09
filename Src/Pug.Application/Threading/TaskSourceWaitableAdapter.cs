using System;

namespace Pug.Application.Threading
{
	public class TaskSourceWaitableAdapter<T> : IWaitable<T>
	{
		IWorkerTaskSource<T> source;

		public TaskSourceWaitableAdapter(IWorkerTaskSource<T> source)
		{
			this.source = source;
		}

		public bool Wait(ref T result)
		{
			return source.GetNextTask(ref result);
		}

		public bool Wait(int timeout, ref T result)
		{
			return source.GetNextTask(timeout, ref result);
		}
	}

}
