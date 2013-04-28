using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Security
{
	public class User : Pug.Application.Security.IUser
	{
		IUserIdentity identity;
		IUserSecurity userSecurity;

		public User(IUserIdentity credentials, IUserSecurity userSecurity)
		{
			this.identity = credentials;
			this.userSecurity = userSecurity;
		}

		public bool IsAuthorized(string operation, ICollection<string> objectNames, IDictionary<string, string> context)
		{
			return userSecurity.UserIsAuthorized(identity, operation, objectNames, context);
		}

		public IUserIdentity Identity
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
