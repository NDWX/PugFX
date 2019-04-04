using System;
using System.Data;
using System.Data.Common;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
    public abstract class ApplicationDataSession : IApplicationDataSession
	{
        IDbConnection connection;
        IDbTransaction transaction;

		public ApplicationDataSession(IDbConnection databaseSession)
		{
			this.connection = databaseSession;
		}

		//public ApplicationDataSession(DataAccessProviderFactory providerFactory)
		//    : this(providerFactory.GetInstance())
		//{
		//}

		#region IApplicationData Members

		protected IDbConnection Connection
		{
			get
			{
				return connection;
			}
		}

        public void BeginTransaction()
        {
            transaction = Connection.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolation )
		{
            transaction = Connection.BeginTransaction(isolation);
		}

		public void RollbackTransaction()
		{
			transaction.Rollback();
		}

		public void CommitTransaction()
		{
            transaction.Commit();
		}

#if NETFX
		public void EnlistInTransaction(Transaction transaction)
		{
			Connection.EnlistTransaction(transaction);
		}
#endif
        #endregion

        protected T EvaluateIsNullToDefault<T>(object obj)
		{
			if (DBNull.Value == obj)
				return default(T);

			return (T)obj;
		}
		
		protected T EvaluateIsNull<T>(object obj, T replacement)
		{
			if( DBNull.Value == obj )
			{
				return replacement;
			}
			
			return (T)obj;
		}

#region IDisposable Members

		public virtual void Dispose()
		{
			Connection.Close();
		}

#endregion
	}
}
