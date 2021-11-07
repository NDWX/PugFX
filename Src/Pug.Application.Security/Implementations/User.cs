using System.Collections.Generic;
using System.Security.Principal;

namespace Pug.Application.Security
{

	public class User : IUser
	{
		private readonly IUserRoleProvider _userRoleProvider;
		private readonly IAuthorizationProvider _userSecurity;

		public User(IPrincipalIdentity credentials, IUserRoleProvider userRoleProvider,
					IAuthorizationProvider userSecurity)
		{
			this.Identity = credentials;
			this._userRoleProvider = userRoleProvider;
			this._userSecurity = userSecurity;
		}

		public bool IsInRole(string role)
		{
			return _userRoleProvider.UserIsInRole(Identity.Identifier, null, role);
		}
		
		public bool IsInRole(string role, string domain)
		{
			return _userRoleProvider.UserIsInRole(Identity.Identifier, domain, role);
		}

		public bool IsAuthorized(IDictionary<string, string> context, string operation, string objectType,
								string objectName = "", string purpose = "", string domain = null)
		{
			return _userSecurity.UserIsAuthorized(context, this, operation, objectType, objectName, purpose, domain);
		}

		public IEnumerable<string> GetRoles(string domain = null)
		{
			return _userRoleProvider.GetUserRoles(this.Identity.Identifier, domain);
		}

		public IPrincipalIdentity Identity { get; }

		IIdentity IPrincipal.Identity => Identity;

		public void Dispose()
		{
			this.Identity.Dispose();
		}
	}
}