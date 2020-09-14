using System.Web;

namespace Pug.Application.Security.Web
{
	public class AspNetBasicSecurityManager : SecurityManager
	{
		string stateKeyPrefix;

		public AspNetBasicSecurityManager(string application, IUserIdentityProvider identityProvider, IUserRoleProvider userRoleProvider, IAuthorizationProvider authorizationProvider) : this(application, identityProvider, userRoleProvider, authorizationProvider, "_PAS_")
		{}

		public AspNetBasicSecurityManager(string application, IUserIdentityProvider identityProvider, IUserRoleProvider userRoleProvider, IAuthorizationProvider authorizationProvider, string stateKeyPrefix) : base(application, identityProvider, userRoleProvider, authorizationProvider)
		{
			this.stateKeyPrefix += stateKeyPrefix;
		}

		string GetCurrentUserSessionKey()
		{
			return string.Format("{0}CurrentUser", stateKeyPrefix);
		}

		protected override void SetCurrentUser(IUser user)
		{
			HttpContext.Current.Session[GetCurrentUserSessionKey()] = user;
		}

		protected override IUser GetCurrentUser()
		{
			return (IUser)HttpContext.Current.Session[GetCurrentUserSessionKey()];

		}
	}
}
