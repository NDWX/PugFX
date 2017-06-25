using System;
using System.Data;
using System.Data.Common;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
	public class DatabaseSession
	{
		DbConnection connection;
		DbTransaction transaction;
		
		#if NETFX
		Transaction distributedTransaction;
		#endif

		//DbProviderFactory dbProviderFactory;

		DateTime sessionStart;
		DateTime sessionEnd = DateTime.MaxValue;

		DataExceptionHandler exceptionHandler;

		internal DatabaseSession(DbConnection connection, DataExceptionHandler exceptionHandler)
		{
			this.connection = connection;

			sessionStart = DateTime.Now;
			//this.dbProviderFactory = dbProviderFactory;
			
			this.exceptionHandler = exceptionHandler;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		void CheckConnectionIsOpen()
		{
			if (connection.State != ConnectionState.Open)
				throw new NotConnected();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="TransactionStarted"></exception>
		void CheckTransactionNotExist()
		{
			if (this.transaction != null)
				throw new TransactionStarted();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="TransactionNotStarted"></exception>
		void CheckTransactionExists()
		{
			if (this.transaction == null)
				throw new TransactionNotStarted();
		}

		void Prepare(DbCommand command)
		{
			command.Connection = this.connection;

			if (this.transaction != null)
				command.Transaction = this.transaction;

			//if (command.CommandType == CommandType.StoredProcedure)
			//    command.Prepare();
		}

		//public DbProviderFactory DbProviderFactory
		//{
		//    get
		//    {
		//        return this.DbProviderFactory;
		//    }
		//}

		public DateTime SessionStart
		{
			get
			{
				return this.sessionStart;
			}
		}

		public TimeSpan SessionDuration
		{
			get
			{
				if( sessionEnd == DateTime.MaxValue )
					return DateTime.Now.Subtract(sessionStart);

				return DateTime.Now.Subtract(sessionEnd);
			}
		}

		public ConnectionState State
		{
			get
			{
				return this.connection.State;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		/// <exception cref="TransactionStarted"></exception>
		/// <exception cref="EnlistedInDistributedTransaction"></exception>
		public void BeginTransaction()
		{
			CheckConnectionIsOpen();
#if NETFX
			CheckEnlistedInDistributedTransaction();
#endif
			CheckTransactionNotExist();

			try
			{
				this.transaction = this.connection.BeginTransaction();
			}
			catch( Exception exception )
			{
				exceptionHandler(exception);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		/// <exception cref="TransactionStarted"></exception>
		/// <exception cref="EnlistedInDistributedTransaction"></exception>
		public void BeginTransaction(System.Data.IsolationLevel isolationLevel)
		{
			CheckConnectionIsOpen();
#if NETFX
			CheckEnlistedInDistributedTransaction();
#endif
			CheckTransactionNotExist();
			
			try
			{
				this.transaction = this.connection.BeginTransaction(isolationLevel);
			}
			catch( Exception exception )
			{
				exceptionHandler( exception );
			}
		}

#if NETFX
		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="EnlistedInDistributedTransaction"></exception>
		void CheckEnlistedInDistributedTransaction()
		{
			if (this.distributedTransaction != null)
				throw new EnlistedInDistributedTransaction();

		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="TransactionStarted"></exception>
		public void EnlistTransaction(Transaction transaction)
		{
			CheckTransactionNotExist();

			transaction.TransactionCompleted += new TransactionCompletedEventHandler(transaction_TransactionCompleted);

			this.connection.EnlistTransaction(transaction);

			this.distributedTransaction = transaction;
		}

		void transaction_TransactionCompleted(object sender, TransactionEventArgs e)
		{
			this.distributedTransaction = null;
		}
#endif

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		/// <exception cref="TransactionNotStarted"></exception>
		public void RollbackTransaction()
		{
			CheckTransactionExists();
			
			try
			{
				transaction.Rollback();
			}
			catch( Exception exception )
			{
				exceptionHandler( exception );
			}

			transaction = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		/// <exception cref="TransactionNotStarted"></exception>
		public void CommitTransaction()
		{
			CheckTransactionExists();

			try
			{
				transaction.Commit();
			}
			catch( Exception exception )
			{
				exceptionHandler( exception );
			}

			transaction = null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		public int Execute(DbCommand command)
		{
			CheckConnectionIsOpen();

			Prepare(command);

			int affectedRows = 0;

			try
			{
				affectedRows = command.ExecuteNonQuery();
			}
			catch( Exception exception )
			{
				exceptionHandler( exception );
			}

			return affectedRows;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		public DbDataReader ExecuteQuery(DbCommand command)
		{
			CheckConnectionIsOpen();
			
			Prepare(command);

			DbDataReader dataReader = null;

			try
			{
				dataReader = command.ExecuteReader();
			}
			catch( Exception exception )
			{
				exceptionHandler( exception );
			}

			return dataReader;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		public DbDataReader ExecuteQuery(DbCommand command, CommandBehavior behavior)
		{
			CheckConnectionIsOpen();

			Prepare(command);

			DbDataReader dataReader = null;

			try
			{
				dataReader = command.ExecuteReader( behavior );
			}
			catch( Exception exception )
			{
				exceptionHandler( exception );
			}

			return dataReader;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="NotConnected"></exception>
		public object ExecuteScalar(DbCommand command)
		{
			CheckConnectionIsOpen();

			Prepare(command);

			object scalarValue = null;

			try
			{
				scalarValue = command.ExecuteScalar();
			}
			catch( Exception exception )
			{
				exceptionHandler( exception );
			}

			return scalarValue;
		}

		public void Close()
		{
			if (this.transaction != null)
			{
				this.transaction.Rollback();

				this.transaction = null;
			}

			if (!(connection.State == ConnectionState.Broken || connection.State == ConnectionState.Closed))
				connection.Close();
		}

		[Obsolete("Use Close() instead.", false)]
		public void Disconnect()
		{
			this.Close();
		}

		public static DatabaseSession Create( string location, IDataAccessProvider dataAccessProvider)
		{
			DbConnection connection = dataAccessProvider.DbProviderFactory.CreateConnection();
			connection.ConnectionString = location;

			try
			{
				connection.Open();
			}
			catch( Exception exception )
			{
				throw new UnableToConnect( "Unable to connect to database given the connection string and DbProviderFactory", exception );
			}

			return new DatabaseSession( connection, dataAccessProvider.DataExceptionHandler );
		}
	}
}