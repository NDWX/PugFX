
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Security
{
	public abstract class SecurityManager : Pug.Application.Security.ISecurityManager
	{
		IUserIdentityProvider userIdentityProvider;
		IUserSecurity userSecurity;

		protected SecurityManager(IUserIdentityProvider userIdentityProvider, IUserSecurity userSecurity)
		{
			this.userIdentityProvider = userIdentityProvider;
			this.userSecurity = userSecurity;
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
						currentUser = new User(userIdentity, userSecurity);
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
	}
}