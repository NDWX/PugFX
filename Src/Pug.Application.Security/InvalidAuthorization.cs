using System;

namespace Pug.Application.Security
{
	public class InvalidAuthorization : Exception
	{
		public InvalidAuthorization()
			: base()
		{
		}

		public InvalidAuthorization(string message ) : base(message)
		{

		}
	}
}
