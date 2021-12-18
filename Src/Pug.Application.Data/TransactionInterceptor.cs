using System;
using System.Data;
using System.Linq;

#if NETFX
using System.Transactions;
#endif

using Castle.DynamicProxy;

namespace Pug.Application.Data
{
    internal class TransactionInterceptor : IInterceptor
    {
        private readonly string[] completionMethods = new string[] { "Commit", "Rollback" };
        private readonly Action<Chain<IDbTransaction>.Link> onTransactionCompleted;
        private readonly Action<Chain<IDbTransaction>.Link> onTransactionDisposed;

        public TransactionInterceptor(Action<Chain<IDbTransaction>.Link> onTransactionCompleted, Action<Chain<IDbTransaction>.Link> onTransactionDisposed)
        {
            this.onTransactionCompleted = onTransactionCompleted;
            this.onTransactionDisposed = onTransactionDisposed;
        }

        public void Intercept(IInvocation invocation)
        {
            if (completionMethods.Contains(invocation.Method.Name))
            {
                invocation.Proceed();

                onTransactionCompleted((Chain<IDbTransaction>.Link)invocation.Proxy);
            }
            else if (invocation.Method.Name == "Dispose")
            {
                onTransactionDisposed((Chain<IDbTransaction>.Link)invocation.Proxy);

                invocation.Proceed();
            }
        }
    }
}
