using System;
using System.Data.Common;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
	public abstract class ApplicationDataSession : IApplicationDataSession
	{
		DatabaseSession databaseSession;
		IDataAccessProvider dataAccessProvider;

		public ApplicationDataSession(DatabaseSession databaseSession, IDataAccessProvider dataAccessProvider)
		{
			this.databaseSession = databaseSession;
			this.dataAccessProvider = dataAccessProvider;
		}

		//public ApplicationDataSession(DataAccessProviderFactory providerFactory)
		//    : this(providerFactory.GetInstance())
		//{
		//}

		#region IApplicationData Members

		protected DatabaseSession DatabaseSession
		{
			get
			{
				return databaseSession;
			}
		}

		protected IDataAccessProvider DataAccessProvider
		{
			get
			{
				return dataAccessProvider;
			}
		}

		public void BeginTransaction()
		{
			DatabaseSession.BeginTransaction();
		}

		public void RollbackTransaction()
		{
			DatabaseSession.RollbackTransaction();
		}

		public void CommitTransaction()
		{
			DatabaseSession.CommitTransaction();
		}
#if NETFX
		public void EnlistInTransaction(Transaction transaction)
		{
			DatabaseSession.EnlistTransaction(transaction);
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
			DatabaseSession.Close();
		}

#endregion
	}
}
