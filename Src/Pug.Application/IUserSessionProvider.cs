using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Pug.Application
{
	public interface IApplicationUserSessionProvider
	{
		ApplicationUserSession CurrentSession
		{
			get;
		}
	}
}
