using System;
//using System.Runtime.Serialization;

namespace Pug.Application.Data
{
	//[Serializable]
	public class TransactionStarted : Exception
    {
        public TransactionStarted()
        {
        }

        public TransactionStarted(string message) : base(message)
        {
        }

        public TransactionStarted(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        //protected TransactionStarted(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }
}
