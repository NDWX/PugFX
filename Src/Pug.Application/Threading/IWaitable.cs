using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Application.Threading
{
	public interface IWaitable
	{
		bool Wait();
		bool Wait(int timeout);
	}

	public interface IWaitable<R>
	{
		bool Wait(ref R result);
		bool Wait(int timeout, ref R result);
	}
}
