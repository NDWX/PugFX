using System;
using System.Data;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
	public interface IApplicationDataSession : IDisposable
	{
		void BeginTransaction();

		void BeginTransaction(System.Data.IsolationLevel isolationLevel);

		void CommitTransaction();

		void RollbackTransaction();
#if NETFX
		void EnlistInTransaction(Transaction transaction);
#endif
	}
}
