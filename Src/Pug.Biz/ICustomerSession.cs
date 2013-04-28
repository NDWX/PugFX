using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Biz
{
	public interface ICustomerSession
	{
		string Organization
		{
			get;
		}

		string Staff
		{
			get;
		}

		string Identifier
		{
			get;
		}

		IDictionary<string, string> AuthInfo
		{
			get;
		}
	}
}
