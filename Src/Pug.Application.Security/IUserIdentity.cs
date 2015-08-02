using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Principal;

namespace Pug.Application.Security
{
	public interface IPrincipalIdentity : ICredentials, IDisposable, System.Security.Principal.IIdentity
	{
		IDictionary<string, string> Attributes
		{
			get;
		}
	}
}
