using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Security
{
	public class InvalidCredentials : Exception
	{
		public InvalidCredentials()
			: base()
		{
		}

		public InvalidCredentials(string message)
			: base(message)
		{
		}

		public InvalidCredentials(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}


	public class NotAuthorized : Exception
	{
		public NotAuthorized()
			: base()
		{
		}

		public NotAuthorized(string message)
			: base(message)
		{
		}

		public NotAuthorized(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
