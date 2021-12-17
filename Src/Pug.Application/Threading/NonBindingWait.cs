using System;
using System.Threading;

namespace Pug.Application.Threading
{
	public struct NonBindingWait<T>
	{
		private int _waitTimeout;
		private EventWaitHandle _waitHandle;
		public IWaitable<T> Source;
		public bool TaskReceived;
		public T Task;

		public NonBindingWait(IWaitable<T> source)
		{
			TaskReceived = false;
			Task = default(T);
			Source = null;
			_waitTimeout = 0;
			_waitHandle = null;
		}

		private void Wait()
		{
			T task = default(T);

			TaskReceived = Source.Wait(_waitTimeout, ref task);

			try
			{
				_waitHandle.Set();
			}
			catch (ObjectDisposedException)
			{

			}
			catch
			{
				// todo: log error?
			}
		}

		public void WaitAndNotify(int waitTimeout, EventWaitHandle wait)
		{
			this._waitTimeout = waitTimeout;
			_waitHandle = wait;

			new Thread(new ThreadStart(Wait)).Start();
		}
	}
}
