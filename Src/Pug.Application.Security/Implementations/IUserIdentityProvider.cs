using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Security
{
	public interface IUserIdentityProvider
	{
		IPrincipalIdentity GetUserIdentity();
	}
}
