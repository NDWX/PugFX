using System;
using System.Data;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
    internal class DbTransaction : IDbTransaction
    {
        internal enum Status
        {
            Open,
            RolledBack,
            Committed
        }

        internal delegate void EventHandler(DbTransaction transaction);

        IDbTransaction x;

        Status status = Status.Open;

        public IDbConnection Connection => x.Connection;

        public IsolationLevel IsolationLevel => x.IsolationLevel;

        public event EventHandler RolledBack, Commited, Ended;

        public DbTransaction(IDbTransaction x)
        {
            this.x = x;
        }

        public void Commit()
        {
            Exception exception = null;

            try
            {
                x.Commit();

                if (Commited != null)
                    Commited(this);
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                this.status = Status.Committed;

                if (Ended != null)
                    Ended(this);
            }

            if (exception != null)
                throw exception;
        }

        public void Dispose()
        {
            Exception exception = null;

            if (status == Status.Open)
                try
                {
                    Rollback();
                }
                catch (Exception e)
                {
                    exception = e;
                }

            x.Dispose();

            if (exception != null)
                throw exception;
        }

        public void Rollback()
        {
            Exception exception = null;

            try
            {
                x.Rollback();

                if (RolledBack != null)
                    RolledBack(this);
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                this.status = Status.RolledBack;

                if (Ended != null)
                    Ended(this);
            }

            if (exception != null)
                throw exception;
        }
    }
}
