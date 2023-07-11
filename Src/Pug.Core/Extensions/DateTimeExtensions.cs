using System;
using Pug.Lang;

namespace Pug.Extensions
{
	public static class DateTimeExtensions
	{
		public static bool IsWithin(this DateTime currentDateTime, IReadOnlyRange<DateTime> range)
		{
			return range.Contains(currentDateTime);
		}

		public static TimeSpan TimeSpan(this IReadOnlyRange<DateTime> range)
		{
			if( range.Start is null )
			{
				return range.End is null ? System.TimeSpan.MaxValue : new TimeSpan( range.End!.Value.Ticks );
			}

			if( range.End is null )
				throw new ArgumentOutOfRangeException();

			return range.End.Value.Subtract( range.Start.Value );
		}
	}
}
