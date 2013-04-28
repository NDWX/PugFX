using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Security
{
	public class DummyUserSecurity : IUserSecurity
	{
		#region IUserSecurity Members

		public bool UserIsAuthorized(ICredentials credentials, string operation, ICollection<string> objectNames, IDictionary<string, string> context)
		{
			return true;
		}

		#endregion
	}
}
