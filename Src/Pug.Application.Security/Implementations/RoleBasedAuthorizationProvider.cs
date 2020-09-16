using System.Collections.Generic;

namespace Pug.Application.Security
{
	public abstract class RoleBasedAuthorizationProvider : IAuthorizationProvider
	{
		IUserRoleProvider roleProvider;

		protected RoleBasedAuthorizationProvider(IUserRoleProvider roleProvider)
		{
			this.roleProvider = roleProvider;
		}

		protected IUserRoleProvider RoleProvider
		{
			get
			{
				return roleProvider;
			}
		}
		
		public abstract bool UserIsAuthorized( IDictionary<string, string> context, ICredentials credentials, string operation, string objectType, string objectName = "" );
	}
}
