using System.Collections.Generic;
using System.Threading;

namespace Pug.Application.Threading
{
	public class ResourceWaitContext<T>
	{
		private readonly EventWaitHandle _waitHandle;
		private readonly object _shakeSync;
		private bool _isWaiting;

		private readonly T _emptyResource;
		private readonly EqualityComparer<T> _resourceComparer;

		public ResourceWaitContext(int waitTimeout)
		{
			WaitTimeout = waitTimeout;

			TaskReceived = false;
			Resource = default(T);
			_waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

			_shakeSync = new object();
			_isWaiting = true;
			_emptyResource = default(T);
			_resourceComparer = EqualityComparer<T>.Default;
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

		private bool Shake(T resource)
		{
			lock (_shakeSync)
			{
				if (_resourceComparer.Equals(resource, _emptyResource))
				{
					_waitHandle.Dispose();
					_isWaiting = false;

					return !_resourceComparer.Equals(Resource, _emptyResource);
				}
				else
				{
					if (!_isWaiting)
						return false;

					try
					{
						_waitHandle.Set();
					}
					catch
					{
						return false;
					}

					Resource = resource;

					return true;
				}
			}
		}

		public bool Wait()
		{
			_waitHandle.WaitOne(WaitTimeout);

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
				_waitHandle.Set();
			}
			catch
			{

			}
		}
	}
}
