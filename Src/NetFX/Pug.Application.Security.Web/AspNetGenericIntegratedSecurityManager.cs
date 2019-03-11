using System.Collections.Generic;
using System.Web;

using System.Security.Principal;
using System;

namespace Pug.Application.Security.Web
{
	public class AspNetIntegratedSecurityManager : SecurityManager
	{
		internal class UserPseudoIdentity  : IPrincipalIdentity
		{
			IIdentity identity;

			public UserPseudoIdentity(IIdentity identity)
			{
				this.identity = identity;
			}

			public string Name
			{
				get { return this.identity.Name; }
			}

			public IDictionary<string, string> Attributes
			{
				get { return null; }
			}

			public string Identifier
			{
				get { return string.Empty; }
			}

			public bool Equals(ICredentials other)
			{
				return (other is UserPseudoIdentity && ((UserPseudoIdentity)other).Name == this.Name);
			}

			public void Dispose()
			{
			}

			public string AuthenticationType
			{
				get { return this.identity.AuthenticationType; }
			}

			public bool IsAuthenticated
			{
				get { return this.identity.IsAuthenticated; }
			}
		}

		internal class UserPseudoIdentityProvider : IUserIdentityProvider
		{
			public Security.IPrincipalIdentity GetUserIdentity()
			{
				return new UserPseudoIdentity(HttpContext.Current.User.Identity);
			}
		}

		public AspNetIntegratedSecurityManager( IUserRoleProvider userROleProvider, IAuthorizationProvider authorizationProvider) :
			 base(new UserPseudoIdentityProvider(), userROleProvider, authorizationProvider)
		{
		}

		protected override void SetCurrentUser(IUser user)
		{
			HttpContext.Current.User = (User)user;

			System.Threading.Thread.CurrentPrincipal = (User)user;
		}

		protected override IUser GetCurrentUser()
		{
			if (HttpContext.Current.User is User)
				return (User)HttpContext.Current.User;

			return null;
		}
	}
}
