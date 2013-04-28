using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pug.Application;

namespace Pug.Biz
{
	public interface ICsutomerSessionManager<Auth>
	{
		string CreateNewSession(string company, string staff, Auth auth);

		ICustomerSession Current
		{
			get;
		}
	}
}
