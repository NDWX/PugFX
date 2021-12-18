namespace Pug.Application.Threading
{
	public class TaskSourceWaitableAdapter<T> : IWaitable<T>
	{
		private readonly IWorkerTaskSource<T> _source;

		public TaskSourceWaitableAdapter(IWorkerTaskSource<T> source)
		{
			this._source = source;
		}

		public bool Wait(ref T result)
		{
			return _source.GetNextTask(ref result);
		}

		public bool Wait(int timeout, ref T result)
		{
			return _source.GetNextTask(timeout, ref result);
		}
	}

}
