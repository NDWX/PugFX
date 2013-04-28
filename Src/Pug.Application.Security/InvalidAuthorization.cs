using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
