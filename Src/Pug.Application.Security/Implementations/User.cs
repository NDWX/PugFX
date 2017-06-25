using System.Collections.Generic;

using System.Security.Principal;

namespace Pug.Application.Security
{
	public class User : Pug.Application.Security.IUser, System.Security.Principal.IPrincipal
	{
		IPrincipalIdentity identity;
		IUserSecurity userSecurity;

		public User(IPrincipalIdentity credentials, IUserSecurity userSecurity)
		{
			this.identity = credentials;
			this.userSecurity = userSecurity;
		}

		public bool IsInRole(string role)
		{
			return userSecurity.UserIsInRole(identity, role);
		}

		public bool IsAuthorized(string operation, ICollection<string> objectNames, IDictionary<string, string> context)
		{
			return userSecurity.UserIsAuthorized(identity, operation, objectNames, context);
		}

		public IPrincipalIdentity Identity
		{
			get { return identity; }
		}

		IIdentity IPrincipal.Identity
		{
			get
			{
				return identity;
			}
		}

		public void Dispose()
		{
			this.identity.Dispose();
		}
	}
}
