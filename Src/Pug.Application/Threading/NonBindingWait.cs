using System;
using System.Threading;

namespace Pug.Application.Threading
{
	public struct NonBindingWait<T>
	{
		int waitTimeout;
		EventWaitHandle waitHandle;
		public IWaitable<T> Source;
		public bool TaskReceived;
		public T Task;

		public NonBindingWait(IWaitable<T> source)
		{
			TaskReceived = false;
			Task = default(T);
			Source = null;
			waitTimeout = 0;
			this.waitHandle = null;
		}

		void Wait()
		{
			T task = default(T);

			TaskReceived = Source.Wait(waitTimeout, ref task);

			try
			{
				waitHandle.Set();
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
			this.waitTimeout = waitTimeout;
			this.waitHandle = wait;

			new Thread(new ThreadStart(this.Wait)).Start();
		}
	}
}
