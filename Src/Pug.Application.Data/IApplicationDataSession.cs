using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Pug.Application.Data
{
    public interface IApplicationDataSession : IDisposable
    {
        void BeginTransaction();

        void RollbackTransaction();

        void CommitTransaction();

        void EnlistInTransaction(Transaction transaction);
    }
}
