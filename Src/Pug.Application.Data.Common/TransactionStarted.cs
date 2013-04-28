using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Pug.Application.Data
{
    [Serializable]
    public class TransactionStarted : Exception
    {
        public TransactionStarted()
            : base()
        {
        }

        public TransactionStarted(string message)
            : base()
        {
        }

        public TransactionStarted(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TransactionStarted(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
