
using System;
using System.Collections.Generic;

using System.Reflection;

#if !NETSTANDARD_1_3
using System.Transactions;
#endif

using Castle.DynamicProxy;

using Pug.Application.Data;

namespace Pug.Application.ServiceModel
{
	public abstract class ApplicationService<DS> : IApplicationData<DS>, IDisposable 
		where DS : class, IApplicationDataSession
	{
		private readonly IApplicationData<DS> applicationDataProvider;
		private readonly IUserSessionProvider sessionProvider;
#if NETSTANDARD_1_3
		private readonly ProxyGenerator dynamicProxyGenerator = new ProxyGenerator();
#endif
		protected ApplicationService( IApplicationData<DS> applicationDataProvider, IUserSessionProvider sessionProvider )
		{
			this.applicationDataProvider = applicationDataProvider;
			this.sessionProvider = sessionProvider;

			sessionProvider.SessionStarted += SessionProvider_SessionStarted;
		}
		
		private void SessionProvider_SessionStarted(IUserSession session)
		{
			session.Ending += UserSession_Ending;
		}

		private void UserSession_Ending(object sender, EventArgs e)
		{
			((IUserSession) sender).Ending -= UserSession_Ending;
			
			ApplicationTransaction<DS> transaction = Transaction;

			if(transaction != null)
			{
#if NETSTANDARD_1_3
				transaction.DataSession.Dispose();
				transaction.Dispose();
#else
				foreach(ApplicationTransaction<DS> tx in UserTransactions.Values)
				{
					tx.Dispose();
				}
#endif
			}

		}

		protected IApplicationData<DS> DataProvider => this;

#if NETSTANDARD_1_3

		private DS Proxy(DS session)
		{
			Type sessionType = typeof(DS);
			TransactionDataSession.Interceptor interceptor = new TransactionDataSession.Interceptor();

			DS proxy = null;

			if (sessionType.GetTypeInfo().IsInterface)
				proxy = (DS)dynamicProxyGenerator.CreateInterfaceProxyWithTarget(sessionType, session, interceptor);
			else
				proxy = (DS)dynamicProxyGenerator.CreateClassProxyWithTarget(sessionType, session, interceptor);

			return proxy;
		}

		protected DS GetTransactionDataSessionProxy()
		{
			if (Transaction != null)
				return Transaction.DataSessionProxy;

			return null;
		}
#endif
		private IDictionary<string, ApplicationTransaction<DS>> UserTransactions
		{
			get
			{
				IUserSession userSession = sessionProvider.CurrentSession;

				IDictionary<string, ApplicationTransaction<DS>> userTransactions = userSession.Get<IDictionary<string, ApplicationTransaction<DS>>>();

				if (userTransactions == null)
				{
					userTransactions = new Dictionary<string, ApplicationTransaction<DS>>(1);
					
					userSession.Set(string.Empty, userTransactions);
				}

				return userTransactions;
			}
		}

		private void Register(ApplicationTransaction<DS> transaction)
		{
			UserTransactions.Add(transaction.Identifier, transaction);

			Transaction = transaction;
		}

		private ApplicationTransaction<DS> Transaction
		{
			get
			{
				IUserSession userSession = sessionProvider.CurrentSession;

				return userSession?.Get<ApplicationTransaction<DS>>("CURRENT");
			}
			set
			{
				IUserSession userSession = sessionProvider.CurrentSession;

				userSession.Set("CURRENT", value);
			}
		}
		
		public IApplicationTransaction<DS> CurrentTransaction
		{
			get
			{
				return Transaction;
			}
		}

		public IApplicationTransaction<DS> BeginTransaction()
		{
			ApplicationTransaction<DS> transaction = null;

#if NETSTANDARD_1_3
			transaction = Transaction;

			if (transaction == null)
			{

				DS dataSession = applicationDataProvider.GetSession();

				dataSession.BeginTransaction();

				transaction = new ApplicationTransaction<DS>(dataSession, Proxy(dataSession), onEnded: tx => UserTransactions.Remove(tx.Identifier));
				Register(transaction);
			}
#else
			transaction = new ApplicationTransaction<DS>();
			Register(transaction);
#endif

			return transaction;
		}
#if !NETSTANDARD_1_3
		public IApplicationTransaction<DS> BeginTransaction(Transaction tx)
		{
			ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(tx);
			Register(transaction);

			return transaction;
		}
		public IApplicationTransaction<DS> BeginTransaction(Transaction tx, TimeSpan timeout)
		{
			ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(tx);
			Register(transaction);

			return transaction;
		}
		
		public IApplicationTransaction<DS> BeginTransaction(Transaction tx, TransactionScopeAsyncFlowOption asyncFlowOption)
		{
			ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(tx, asyncFlowOption);
			Register(transaction);

			return transaction;
		}
		
		public IApplicationTransaction<DS> BeginTransaction(Transaction tx, TimeSpan timeout, TransactionScopeAsyncFlowOption asyncFlowOption)
		{
			ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(tx, timeout, asyncFlowOption);
			Register(transaction);

			return transaction;
		}
		
		public IApplicationTransaction<DS> BeginTransaction(TransactionScopeOption option)
		{
			ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(option);
			Register(transaction);

			return transaction;
		}
		
		public IApplicationTransaction<DS> BeginTransaction(TransactionScopeOption option, TimeSpan timeout)
		{
			ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(option, timeout);
			Register(transaction);

			return transaction;
		}
		
		public IApplicationTransaction<DS> BeginTransaction(TransactionScopeOption option, TransactionScopeAsyncFlowOption asyncFlowOption)
		{
			ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(option, asyncFlowOption);
			Register(transaction);

			return transaction;
		}
		
		public IApplicationTransaction<DS> BeginTransaction(TransactionScopeOption option, TimeSpan timeout, TransactionScopeAsyncFlowOption asyncFlowOption)
		{
			ApplicationTransaction<DS> transaction = new ApplicationTransaction<DS>(option, timeout, asyncFlowOption);
			Register(transaction);

			return transaction;
		}
		
		public IApplicationTransaction<DS> BeginTransaction(TransactionScopeOption option, TransactionOptions options, TransactionScopeAsyncFlowOption asyncFlowOption)
		{
			ApplicationTransaction<DS> transaction  = new ApplicationTransaction<DS>(option, options, asyncFlowOption);
			Register(transaction);

			return transaction;
		}
#endif
		DS IApplicationData<DS>.GetSession()
		{
			DS dataSession;
			
#if NETSTANDARD_1_3
			dataSession = GetTransactionDataSessionProxy();

			if (dataSession == null)
				dataSession = applicationDataProvider.GetSession();
#else
			dataSession = applicationDataProvider.GetSession();
#endif
			return dataSession;
		}

		public void Dispose()
		{
		}
	}
}
