using System;

namespace Pug.Lang
{
	public static class RangeExtensions
	{
		public static bool Contains<T>( this IReadOnlyRange<T> range, T val ) where T : struct, IComparable<T>
		{
			if( range.Start is null && range.End is null )
				return false;
			
			return ( range.Start is null || val.CompareTo( range.Start.Value ) >= 0 ) && ( range.End is null || val.CompareTo( range.End.Value ) <= 0 );
		}
	}
}