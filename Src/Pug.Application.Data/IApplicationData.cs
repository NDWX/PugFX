using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Data
{
	public interface IApplicationData<T> where T : IApplicationDataSession
	{
		T GetSession();
	}
}