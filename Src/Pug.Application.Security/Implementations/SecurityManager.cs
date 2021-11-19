using System;

namespace Pug.Application.Security
{

    public class SecurityManager : ISecurityManager
	{
		private readonly string _application;
		private readonly IUserSessionProvider _sessionProvider;

		public SecurityManager(string application, ISessionUserIdentityAccessor sessionUserIdentityAccessor, IUserRoleProvider userRoleProvider, IAuthorizationProvider uathorizationProvider, IUserSessionProvider sessionProvider)
		{
            _application = application;
			SessionUserIdentityAccessor = sessionUserIdentityAccessor;
			UserRoleProvider = userRoleProvider;
			AuthorizationProvider = uathorizationProvider;
            _sessionProvider = sessionProvider;
		}

		public IUser CurrentUser
		{
			get
			{
				IUser currentUser = GetCurrentUser();

				if (currentUser == null)
				{
					IPrincipalIdentity userIdentity = SessionUserIdentityAccessor.GetUserIdentity();

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
            _sessionProvider.CurrentSession.Set($"{_application}.SecurityManager.User", user);
        }

		protected virtual IUser GetCurrentUser()
		{
			IUser currentUser = null;

			try
			{
				currentUser = _sessionProvider.CurrentSession?.Get<IUser>($"{_application}.SecurityManager.User");
			}
			catch(NullReferenceException)
			{
			}

			return currentUser;
		}

		protected ISessionUserIdentityAccessor SessionUserIdentityAccessor { get; }

		public  IUserRoleProvider UserRoleProvider { get;  }

        public IAuthorizationProvider AuthorizationProvider { get; }
	}
}