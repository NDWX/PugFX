using System;
using System.Data;
#if NETFX || NETSTANDARD_2_0
using System.Transactions;
#endif

namespace Pug.Application.Data
{

	public abstract class ApplicationDataSession : IApplicationDataSession
	{
		private readonly IDbConnection connection;
		private Chain<IDbTransaction>.Link currentTxLink;

		private readonly object transactionSync = new object();
		
		public ApplicationDataSession(IDbConnection databaseSession)
		{
			connection = databaseSession;
		}

		//private void onTransactionCompleted(Chain<IDbTransaction>.Link link)
		//{
		//    link.Content.Dispose();
		//}

		private void onTransactionDisposed(Chain<IDbTransaction>.Link link)
		{
			currentTxLink = link.Previous;
			TransactionDepth--;
		}

		//IDbTransaction Mix(Chain<IDbTransaction>.Link link)
		//{
		//    IDbTransaction transaction = link.Content;

		//    Type transactionType = transaction.GetType();

		//    ProxyGenerationOptions options = new ProxyGenerationOptions();
		//    options.AddMixinInstance(link);

		//    TransactionInterceptor interceptor = new TransactionInterceptor(onTransactionCompleted, onTransactionDisposed);

		//    IDbTransaction proxy = (IDbTransaction)dynamicProxyGenerator.CreateClassProxyWithTarget(transactionType, link.Content, options, interceptor);

		//    return proxy;
		//}

		#region IApplicationData Members

		protected IDbConnection Connection
		{
			get
			{
				return connection;
			}
		}

		protected IDbTransaction Transaction
		{   get
			{
				return currentTxLink.Content;
			}
		}

		public int TransactionDepth { get; private set; }

		public void BeginTransaction()
		{
			lock (transactionSync)
			{
				currentTxLink = new Chain<IDbTransaction>.Link(Connection.BeginTransaction(), currentTxLink);
				TransactionDepth++;
			}
		}

		public void BeginTransaction(System.Data.IsolationLevel isolation )
		{
			lock (transactionSync)
			{ 
				currentTxLink = new Chain<IDbTransaction>.Link(Connection.BeginTransaction(isolation), currentTxLink);
				TransactionDepth++;
			}
		}

		public void RollbackTransaction()
		{
			lock (transactionSync)
				if ( currentTxLink != null)
					try
					{
						currentTxLink.Content.Rollback();
					}
					finally
					{
						currentTxLink.Content.Dispose();
						onTransactionDisposed(currentTxLink);
					}
		}

		public void CommitTransaction()
		{
			lock (transactionSync)
				if (currentTxLink != null)
					try
					{
						currentTxLink.Content.Commit();
					}
					finally
					{
						currentTxLink.Content.Dispose();
						onTransactionDisposed(currentTxLink);
					}
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
			while( currentTxLink != null )
				RollbackTransaction();

			Connection.Close();
		}

#endregion
	}
}
