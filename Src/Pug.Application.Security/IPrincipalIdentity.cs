using System;
using System.Collections.Generic;

namespace Pug.Application.Security
{
	public class IdentityAttributeNames
	{
		public static readonly string ClientIdentifier = "client.identifier";
	}

	public interface IPrincipalIdentity : ICredentials, IDisposable, System.Security.Principal.IIdentity
	{

		IDictionary<string, string> Attributes
		{
			get;
		}
	}
}
