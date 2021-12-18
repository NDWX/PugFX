
using System;

#if !NETSTANDARD_1_3
using System.Transactions;
#endif

using Pug.Application.Data;

namespace Pug.Application.ServiceModel
{
	internal class ApplicationTransaction<DS> : IApplicationTransaction<DS>
		where DS : class, IApplicationDataSession
	{
		private readonly object sync = new object();

		// ReSharper disable RedundantDefaultMemberInitializer
		private bool transactionEnded = false;
		// ReSharper restore RedundantDefaultMemberInitializer
		
		public string Identifier { get; }

#if NETSTANDARD_1_3
		private readonly Action<ApplicationTransaction<DS>> onEnded;

		internal DS DataSession { get; }

		public DS DataSessionProxy { get;  }
#else
		private readonly TransactionScope transaction;
#endif

#if NETSTANDARD_1_3
		public ApplicationTransaction(DS dataSession, DS dataSessionProxy, Action<ApplicationTransaction<DS>> onEnded)
		{
			Identifier = Guid.NewGuid().ToString();

			DataSession = dataSession;
			DataSessionProxy = dataSessionProxy;
			this.onEnded = onEnded;
		}
#else
		private ApplicationTransaction(TransactionScope transactionScope)
		{
			Identifier = Guid.NewGuid().ToString();

			transaction = transactionScope;
		}

		public ApplicationTransaction()
			: this(new TransactionScope())
		{
		}

		public ApplicationTransaction(TransactionScopeOption transactionScopeOption)
			: this(new TransactionScope(transactionScopeOption))
		{

		}

		public ApplicationTransaction(TransactionScopeAsyncFlowOption asyncFlowOption)
			: this(new TransactionScope(asyncFlowOption))
		{

		}

		public ApplicationTransaction(TransactionScopeOption transactionScopeOption,
									TransactionOptions transactionOptions)
			: this(new TransactionScope(transactionScopeOption, transactionOptions))
		{

		}

		public ApplicationTransaction(TransactionScopeOption transactionScopeOption, TimeSpan timeout)
			: this(new TransactionScope(transactionScopeOption, timeout))
		{

		}

		public ApplicationTransaction(TransactionScopeOption transactionScopeOption,
									TransactionScopeAsyncFlowOption asyncFlowOption)
			: this(new TransactionScope(transactionScopeOption, asyncFlowOption))
		{

		}

		public ApplicationTransaction(TransactionScopeOption transactionScopeOption, TimeSpan timeout,
									TransactionScopeAsyncFlowOption asyncFlowOption)
			: this(new TransactionScope(transactionScopeOption, timeout, asyncFlowOption))
		{

		}

		public ApplicationTransaction(TransactionScopeOption transactionScopeOption,
									TransactionOptions transactionOptions,
									TransactionScopeAsyncFlowOption asyncFlowOption)
			: this(new TransactionScope(transactionScopeOption, transactionOptions, asyncFlowOption))
		{

		}

		public ApplicationTransaction(Transaction transaction)
			: this(new TransactionScope(transaction))
		{

		}

		public ApplicationTransaction(Transaction transaction,
									TransactionScopeAsyncFlowOption asyncFlowOption)
			: this(new TransactionScope(transaction, asyncFlowOption))
		{

		}

		public ApplicationTransaction(Transaction transaction, TimeSpan timeout,
									TransactionScopeAsyncFlowOption asyncFlowOption)
			: this(new TransactionScope(transaction, timeout, asyncFlowOption))
		{

		}
#endif

		public void Commit()
		{
			lock(sync)
			{
				if(transactionEnded)
					throw new InvalidTransactionState();

				try
				{
#if NETSTANDARD_1_3
					DataSession.CommitTransaction();
#else
					transaction.Complete();
#endif
				}
				finally
				{
					transactionEnded = true;
#if NETSTANDARD_1_3
					onEnded(this);
#endif
				}

			}
		}

		private void rollback()
		{
			try
			{
#if NETSTANDARD_1_3
				DataSession.RollbackTransaction();
#else
				transaction.Dispose();
#endif
			}
			finally
			{
				transactionEnded = true;
#if NETSTANDARD_1_3
				onEnded(this);
#endif
			}
		}

		public void Rollback()
		{
			lock(sync)
			{
				if(transactionEnded)
					throw new InvalidTransactionState();

				rollback();
			}
		}

		public void Dispose()
		{
			lock(sync)
			{
				if(!transactionEnded)
				{
#if NETSTANDARD_1_3
					rollback();
#else
					transaction.Dispose();
#endif
				}
			}
		}
	}
}
