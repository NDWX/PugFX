using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Transactions;

namespace Pug.Application.Data
{
	public interface IApplicationData<out T> where T : class, IApplicationDataSession
	{
		T GetSession();
	}
}