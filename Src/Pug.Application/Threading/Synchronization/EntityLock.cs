using System;
using System.Threading;

namespace Pug.Application.Threading.Synchronization
{
    internal class EntityLock : IDisposable
    {
        string identifier;
        Mutex entityMutex;

        object counterSync;

        int waitCounter;
        bool locked;
        DateTime lastLock = DateTime.MinValue;

        internal EntityLock(string identifier, Mutex mutex)
        {
            this.identifier = identifier;
            counterSync = new object();

            this.entityMutex = mutex;
        }

        void IncreaseLockWait()
        {
            Monitor.Enter(counterSync);

            waitCounter++;

            Monitor.Exit(counterSync);
        }

        void DecreaseLockWait()
        {
            Monitor.Enter(counterSync);

            waitCounter--;

            Monitor.Exit(counterSync);
        }

        public bool TryLock(int timeout)
        {
            IncreaseLockWait();

            bool entityLocked = entityMutex.WaitOne(timeout, false);

            if (entityLocked)
            {
                lastLock = DateTime.Now;
                locked = true;
            }

            DecreaseLockWait();

            return entityLocked;
        }

        public void ReleaseLock()
        {
            locked = false;
            entityMutex.ReleaseMutex();
        }

        public int WaitCounter
        {
            get { return waitCounter; }
            set { waitCounter = value; }
        }

        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        public DateTime LastLock
        {
            get { return lastLock; }
            set { lastLock = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Monitor.Enter(counterSync);

            if (this.locked || this.waitCounter > 0)
                throw new EntityLocked(this.identifier);

            this.entityMutex.Close();
            this.entityMutex = null;

            Monitor.Exit(counterSync);

            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
