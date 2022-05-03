
using System;

namespace Pug.Application.ServiceModel
{
	public class InvalidTransactionState : Exception
	{
		public InvalidTransactionState()
		{

		}

		public InvalidTransactionState(string message) : base(message)
		{

		}
	}
}
