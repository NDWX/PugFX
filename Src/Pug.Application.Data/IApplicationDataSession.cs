using System;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
    public interface IApplicationDataSession : IDisposable
    {
        void BeginTransaction();

        void RollbackTransaction();

        void CommitTransaction();
#if NETFX
        void EnlistInTransaction(Transaction transaction);
#endif
    }
}
