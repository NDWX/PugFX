using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;

namespace Pug.Application.Synchronization
{
	public class EntitySynchronizationManager : IDisposable
	{
		static string GetRandomIdentity()
		{
			return Guid.NewGuid().ToString();
		}

		static IdentityTracker<string> identityTracker = new IdentityTracker<string>(GetRandomIdentity);

		string name;
		HybridDictionary locks;
		ReaderWriterLockSlim globalLock;
		TimeSpan lockIdleTimeout;
		bool isDisposing, isDisposed;
		int cleanupInterval;

		Timer cleanupTimer;
#if TRACE
		TraceSwitch traceSwitch;
#endif
		public EntitySynchronizationManager(string name, bool identityIsCaseSensitive, TimeSpan lockIdleTimeout, int cleanupInterval)
		{
			this.name = name;

			if (!identityTracker.RegisterIdentifier(name))
				throw new ArgumentException(string.Format("Specified name: {0} is already used", name), "name");

			globalLock = new ReaderWriterLockSlim();

			locks = new HybridDictionary(!identityIsCaseSensitive);

			this.lockIdleTimeout = lockIdleTimeout;

			cleanupTimer = new Timer(new TimerCallback(cleanupTimer_Elapsed), null, Timeout.Infinite, 0);

			this.cleanupInterval = cleanupInterval;
#if TRACE
			traceSwitch = new TraceSwitch("Pug.Application.Threading.EntitySynchronizationManager", "Entity Threading Trace");
#endif
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="identityIsCaseSensitive"></param>
		/// <param name="lockIdleTimeout"></param>
		/// <param name="cleanupInterval"></param>
		public EntitySynchronizationManager(bool identityIsCaseSensitive, TimeSpan lockIdleTimeout, int cleanupInterval)
			: this(identityTracker.GetNewIdentifier(), identityIsCaseSensitive, lockIdleTimeout, cleanupInterval)
		{
		}

		void cleanupTimer_Elapsed(object state)
		{
			Cleanup();
		}

		void Cleanup()
		{
#if TRACE
			Trace.WriteLineIf(this.traceSwitch.TraceVerbose, "Starting cleanup");
#endif
			string[] entities = new string[locks.Count];

			this.locks.Keys.CopyTo(entities, 0);            
#if TRACE
			Trace.WriteLineIf(this.traceSwitch.TraceVerbose, string.Format("{0} entities to check.", locks.Count.ToString()));
#endif
			object lockObj;
			EntityLock entityLock;
			TimeSpan idle;

			foreach (string entity in entities)
			{
				if (isDisposing || isDisposed)
					return;

				if (string.IsNullOrEmpty(entity))
					continue;
#if TRACE
				Trace.WriteLineIf(this.traceSwitch.TraceVerbose, string.Format("Checking {0} . . .", entity));
#endif
				if (!locks.Contains(entity))
				{
#if TRACE
					Trace.WriteLineIf(this.traceSwitch.TraceWarning, string.Format("Lock {0} not found", entity));
#endif
					continue;
				}

				lockObj = locks[entity];

				if (lockObj == null)
					continue;

				entityLock = (EntityLock)lockObj;

				idle = DateTime.Now.Subtract(entityLock.LastLock);                
#if TRACE
				Trace.WriteLineIf(this.traceSwitch.TraceVerbose, string.Format("Lock {0} {1}locked.", entity, entityLock.Locked? string.Empty:"not "));
				Trace.WriteLineIf(this.traceSwitch.TraceVerbose, string.Format("Lock {0} idle for {1}.", entity, idle.TotalMilliseconds.ToString()));
				Trace.WriteLineIf(this.traceSwitch.TraceVerbose, string.Format("Lock {0} wait: {1}.", entity, entityLock.WaitCounter.ToString()));
#endif
				if (!entityLock.Locked && entityLock.WaitCounter == 0 && idle.CompareTo(lockIdleTimeout) > -1)
				{
#if TRACE
					Trace.WriteLineIf(this.traceSwitch.TraceInfo, string.Format("Lock {0} expired.", entity));
#endif
					globalLock.EnterWriteLock();

					try
					{
						entityLock.Dispose();
					}
					catch (EntityLocked)
					{
						globalLock.ExitWriteLock();
						continue;
					}

					locks.Remove(entity);                    
#if TRACE
					Trace.WriteLineIf(this.traceSwitch.TraceInfo, string.Format("Lock {0} removed.", entity));
#endif
					globalLock.ExitWriteLock();
				}
			}

			if ( this.locks.Count > 0 && !isDisposing )
			{
				this.cleanupTimer.Change(cleanupInterval, 0);
#if TRACE
				Trace.WriteLineIf(this.traceSwitch.TraceInfo, "Cleanup timer restarted");
#endif
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="identifier"></param>
		/// <param name="timeout"></param>
		/// <returns></returns>
		/// <exception cref="ObjectDisposedException"></exception>
		public bool ObtainLock(string identifier, int timeout)
		{
			bool locked = false;

			EntityLock entityLock;

			if (isDisposing)
			{
				throw new ObjectDisposedException("EntitySynchronizationManager");
			}

			globalLock.EnterUpgradeableReadLock();

			if (isDisposed)
			{
				globalLock.ExitUpgradeableReadLock();
				throw new ObjectDisposedException("EntitySynchronizationManager");
			}

			if (locks.Contains(identifier))
			{                
#if TRACE
				Trace.WriteLineIf(this.traceSwitch.TraceInfo, string.Format("{0}: Lock for {1} already exists, reusing.", System.Threading.Thread.CurrentThread.ManagedThreadId, identifier));
#endif
				entityLock = (EntityLock)locks[identifier];

				globalLock.ExitUpgradeableReadLock();

				locked = entityLock.TryLock(timeout);
			}
			else
			{
#if TRACE
				Trace.WriteLineIf(this.traceSwitch.TraceInfo, string.Format("{0}: Lock for {1} does not exist, creating . . .", System.Threading.Thread.CurrentThread.ManagedThreadId, identifier));
#endif
				entityLock = new EntityLock(identifier, new Mutex(false));
				entityLock.TryLock(timeout);

				globalLock.EnterWriteLock();

				if (locks.Contains(identifier))
				{
					globalLock.ExitWriteLock();

					entityLock.Dispose();

					entityLock = (EntityLock)locks[identifier];

					locked = entityLock.TryLock(timeout);
				}
				else
				{
					locks.Add(identifier, entityLock);

					globalLock.ExitWriteLock();
					globalLock.ExitUpgradeableReadLock();

					if (this.locks.Count > 0)
					{
						this.cleanupTimer.Change(cleanupInterval, 0);
#if TRACE
						Trace.WriteLineIf(this.traceSwitch.TraceInfo, "Cleanup timer started");
#endif
					}

					locked = true;
				}
			}            
#if TRACE
			Trace.WriteLineIf(this.traceSwitch.TraceInfo, string.Format("{0}: lock for {1} {2}obtained.", System.Threading.Thread.CurrentThread.ManagedThreadId, identifier, locked ? string.Empty : "not "));
#endif
			return locked;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="identifier"></param>
		/// <exception cref="ObjectDisposedException"></exception>
		public void ReleaseLock(string identifier)
		{
			EntityLock entityLock;

			if (isDisposed )
				throw new ObjectDisposedException("EntitySynchronizationManager");

			if( !locks.Contains(identifier) )
				throw new ArgumentOutOfRangeException();

			entityLock = (EntityLock)locks[identifier];

			try
			{
				entityLock.ReleaseLock();
			}
			catch (Exception)
			{
#if TRACE
				Trace.WriteLineIf(this.traceSwitch.TraceWarning, string.Format("{0}: Lock for {1} was not obtained.", System.Threading.Thread.CurrentThread.ManagedThreadId, identifier));
#endif
				return;
			}            
#if TRACE
			Trace.WriteLineIf(this.traceSwitch.TraceInfo, string.Format("{0}: Lock for {1} released.", System.Threading.Thread.CurrentThread.ManagedThreadId, identifier));
#endif
		}

		#region IDisposable Members

		public void Dispose()
		{
			isDisposing = true;

			this.Dispose(true);

			isDisposing = false;

			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cleanupManagedResources"></param>
		/// <exception cref="EntityLocked"></exception>
		protected virtual void Dispose(bool cleanupManagedResources)
		{
			if (cleanupManagedResources)
			{
				this.cleanupTimer.Change(Timeout.Infinite, 0);

				globalLock.EnterWriteLock();

				ArrayList entities = new ArrayList(locks.Count);

				IDictionaryEnumerator enumerator = this.locks.GetEnumerator();

				while (enumerator.MoveNext())
				{
					try
					{
						((EntityLock)enumerator.Value).Dispose();

						entities.Add(enumerator.Key);
					}
					catch (EntityLocked)
					{
						foreach (object entity in entities)
							locks.Remove(entity);

						throw;
					}
				}

				this.locks.Clear();

				isDisposed = true;

				globalLock.ExitWriteLock();

				cleanupTimer.Dispose();

				globalLock.Dispose();
			}
		}

		#endregion
	}
}