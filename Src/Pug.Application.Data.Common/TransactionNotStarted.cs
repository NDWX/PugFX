﻿using System;
//using System.Runtime.Serialization;

namespace Pug.Application.Data
{
	//[Serializable]
	public class TransactionNotStarted : Exception
	{
		public TransactionNotStarted()
			: base()
		{
		}

		public TransactionNotStarted(string message)
			: base()
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
