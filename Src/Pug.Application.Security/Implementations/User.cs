using System.Collections.Generic;

using System.Security.Principal;

namespace Pug.Application.Security
{

	public class User : Pug.Application.Security.IUser, System.Security.Principal.IPrincipal
	{
		IPrincipalIdentity identity;
		IUserRoleProvider userRoleProvider;
		IAuthorizationProvider userSecurity;

		public User(IPrincipalIdentity credentials, IUserRoleProvider userRoleProvider, IAuthorizationProvider userSecurity)
		{
			this.identity = credentials;
			this.userRoleProvider = userRoleProvider;
			this.userSecurity = userSecurity;
		}

		public bool IsInRole(string role)
		{
			return userRoleProvider.UserIsInRole(identity.Identifier, role);
		}

		public bool IsAuthorized( IDictionary<string, string> context, string operation, string objectType, string objectName = "" )
		{
			return userSecurity.UserIsAuthorized( context, identity, operation, objectType, objectName);
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
