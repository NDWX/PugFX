using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
    public interface IApplicationDataSession : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
#if NETFX
        void EnlistInTransaction(Transaction transaction);
#endif
    }
}
