using Pug.Application;

namespace Pug.Application.Security
{

    public class SecurityManager : ISecurityManager
	{
        string application;
        ISessionUserIdentityAccessor sessionUserIdentityAccessor;
        IAuthorizationProvider authorizationProvider;
        IUserSessionProvider sessionProvider;

		public SecurityManager(string application, ISessionUserIdentityAccessor sessionUserIdentityAccessor, IUserRoleProvider userRoleProvider, IAuthorizationProvider uathorizationProvider, IUserSessionProvider sessionProvider)
		{
            this.application = application;
			this.sessionUserIdentityAccessor = sessionUserIdentityAccessor;
			this.UserRoleProvider = userRoleProvider;
			this.AuthorizationProvider = uathorizationProvider;
            this.sessionProvider = sessionProvider;
		}

		public IUser CurrentUser
		{
			get
			{
				IUser currentUser = GetCurrentUser();

				if (currentUser == null)
				{
					IPrincipalIdentity userIdentity = sessionUserIdentityAccessor.GetUserIdentity();

					if (userIdentity != null)
					{
						currentUser = new User(userIdentity, UserRoleProvider, AuthorizationProvider);
						SetCurrentUser(currentUser);
					}
				}

				return currentUser;
			}
		}

		protected virtual void SetCurrentUser(IUser user)
        {
            sessionProvider.CurrentSession.Set<IUser>($"{application}.SecurityManager.User", user);
        }

		protected virtual IUser GetCurrentUser()
        {
            return sessionProvider.CurrentSession?.Get<IUser>($"{application}.SecurityManager.User");
        }

		protected ISessionUserIdentityAccessor SessionUserIdentityAccessor
		{
			get
			{
				return sessionUserIdentityAccessor;
			}
		}

        public  IUserRoleProvider UserRoleProvider { get; private set; }

        public IAuthorizationProvider AuthorizationProvider { get => authorizationProvider; private set => authorizationProvider = value; }
    }
}