﻿using System.Collections.Generic;

namespace Pug.Application.Security
{
	public abstract class RoleBasedUserSecurity : IUserSecurity
	{
		IUserRoleProvider roleProvider;

		protected RoleBasedUserSecurity(IUserRoleProvider roleProvider)
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

		public abstract bool UserIsInRole(ICredentials credentials, string role);

		public abstract bool UserIsAuthorized(ICredentials credentials, string operation, ICollection<string> objectNames, IDictionary<string, string> context);
	}
}
