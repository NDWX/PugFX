using System.Web;

namespace Pug.Application.Security.Web
{
	public class AspNetBasicSecurityManager : SecurityManager
	{
		string stateKeyPrefix;

		public AspNetBasicSecurityManager(IUserIdentityProvider identityProvider, IUserSecurity userSecurity) : this(identityProvider, userSecurity, "_PAS_")
		{}

		public AspNetBasicSecurityManager(IUserIdentityProvider identityProvider, IUserSecurity userSecurity, string stateKeyPrefix) : base(identityProvider, userSecurity)
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
