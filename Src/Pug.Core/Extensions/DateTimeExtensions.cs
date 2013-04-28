using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Extensions
{
	public static class DateTimeExtensions
	{
		public static bool IsWithin(this DateTime currentDateTime, Range<DateTime> range)
		{
			return range.Contains(currentDateTime);
		}

		public static TimeSpan TimeSpan(this Range<DateTime> range)
		{
			return range.End.Subtract(range.Start);
		}
	}
}
