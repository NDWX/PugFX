using System;
//using System.Runtime.Serialization;

namespace Pug.Application.Data
{
	//[Serializable]
	public class TransactionNotStarted : Exception
	{
		public TransactionNotStarted()
		{
		}

		public TransactionNotStarted(string message) : base(message)
		{
		}

		public TransactionNotStarted(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		//protected TransactionNotStarted(SerializationInfo info, StreamingContext context)
		//    : base(info, context)
		//{
		//}
	}
}
