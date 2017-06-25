using System;

namespace Pug.Application.Data
{
	//[Serializable]
	public class EnlistedInDistributedTransaction : Exception
	{
		public EnlistedInDistributedTransaction()
			: base()
		{
		}

		public EnlistedInDistributedTransaction(string message)
			: base()
		{
		}

		public EnlistedInDistributedTransaction(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		//protected EnlistedInDistributedTransaction(SerializationInfo info, StreamingContext context)
		//    : base(info, context)
		//{
		//}
	}
}
