using System;
//using System.Runtime.Serialization;

namespace Pug.Application.Data
{
	//[Serializable]
	public class UnableToConnect : Exception
	{
		public UnableToConnect()
		{
		}

		public UnableToConnect(string message) : base(message)
		{
		}

		public UnableToConnect(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		//protected UnableToConnect(SerializationInfo info, StreamingContext context)
		//    : base(info, context)
		//{
		//}
	}
}
