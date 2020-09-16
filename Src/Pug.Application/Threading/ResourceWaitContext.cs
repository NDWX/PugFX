using System;
using System.Collections.Generic;
using System.Threading;

namespace Pug.Application.Threading
{
	public class ResourceWaitContext<T>
	{
		EventWaitHandle waitHandle;
		object shakeSync;
		bool isWaiting;

		T emptyResource;
		EqualityComparer<T> resourceComparer;

		public ResourceWaitContext(int waitTimeout)
		{
			this.WaitTimeout = waitTimeout;

			TaskReceived = false;
			Resource = default(T);
			this.waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

			shakeSync = new object();
			isWaiting = true;
			emptyResource = default(T);
			resourceComparer = EqualityComparer<T>.Default;
		}

		public bool TaskReceived
		{
			get;
			protected set;
		}

		public T Resource
		{
			get;
			protected set;
		}

		public int WaitTimeout
		{
			get;
			protected set;
		}

		bool Shake(T resource)
		{
			lock (shakeSync)
			{
				if (resourceComparer.Equals(resource, emptyResource))
				{
					waitHandle.Dispose();
					isWaiting = false;

					return !resourceComparer.Equals(this.Resource, emptyResource);
				}
				else
				{
					if (!isWaiting)
						return false;

					try
					{
						waitHandle.Set();
					}
					catch
					{
						return false;
					}

					this.Resource = resource;

					return true;
				}
			}
		}

		public bool Wait()
		{
			waitHandle.WaitOne(WaitTimeout);

			TaskReceived = Shake(default(T));

			return TaskReceived;
		}

		public bool Set(T resource)
		{
			return Shake(resource);
		}

		public void Cancel()
		{
			try
			{
				waitHandle.Set();
			}
			catch
			{

			}
		}
	}
}
