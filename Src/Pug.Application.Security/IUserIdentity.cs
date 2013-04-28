using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Security
{
	public interface IUserIdentity : ICredentials, IDisposable
	{
		string Name
		{
			get;
		}

		IDictionary<string, string> Attributes
		{
			get;
		}
	}
}
