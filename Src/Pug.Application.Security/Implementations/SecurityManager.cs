namespace Pug.Application.Security
{
	public abstract class SecurityManager : Pug.Application.Security.ISecurityManager
	{
		IUserIdentityProvider userIdentityProvider;
		IUserRoleProvider userRoleProvider;
		IAuthorizationProvider authorizationProvider;

		protected SecurityManager(IUserIdentityProvider userIdentityProvider, IUserRoleProvider userRoleProvider, IAuthorizationProvider uathorizationProvider)
		{
			this.userIdentityProvider = userIdentityProvider;
			this.userRoleProvider = userRoleProvider;
			this.authorizationProvider = uathorizationProvider;
		}

		public IUser CurrentUser
		{
			get
			{
				IUser currentUser = GetCurrentUser();

				if (currentUser == null)
				{
					IPrincipalIdentity userIdentity = userIdentityProvider.GetUserIdentity();

					if (userIdentity != null)
					{
						currentUser = new User(userIdentity, userRoleProvider, authorizationProvider);
						SetCurrentUser(currentUser);
					}
				}

				return currentUser;
			}
		}

		protected abstract void SetCurrentUser(IUser user);

		protected abstract IUser GetCurrentUser();

		protected IUserIdentityProvider UserIdentityProvider
		{
			get
			{
				return userIdentityProvider;
			}
		}

		protected IUserRoleProvider UserRoleProvider => this.userRoleProvider;
	}
}