
using System;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

namespace Pug.Application.ServiceModel
{
    internal class TransactionDataSession
    {
        public class InterceptorSelector : IInterceptorSelector
        {
            public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
            {
                throw new NotImplementedException();
            }
        }

        public class Interceptor : IInterceptor
        {
            private string[] interceptedMethods = new string[] { "Dispose" };

            public void Intercept(IInvocation invocation)
            {
                if (!interceptedMethods.Contains(invocation.Method.Name))
                {
                    invocation.Proceed();
                }
            }
        }
    }
}
