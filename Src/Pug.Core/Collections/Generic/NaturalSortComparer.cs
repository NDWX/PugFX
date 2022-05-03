using System;
using System.Collections.Generic;

namespace Pug.Collections.Generic
{
	public class NaturalSortComparer : IComparer<string>
	{
		public int Compare( string x, string y )
		{
			switch( x )
			{
				case null when y is null:
					return 0;
				
				case null:
					return -1;
			}

			if( y is null ) return 1;

			int xLength = x.Length, 
				yLength = y.Length;
			
			if( xLength == yLength )
				return string.Compare( x, y, StringComparison.Ordinal );
			
			int comparisonLength = xLength > yLength ? xLength : yLength;

			int result = string.Compare(
					x.Substring( 0, comparisonLength ),
					y.Substring( 0, comparisonLength ),
					StringComparison.Ordinal
				);

			if( result != 0 )
				return result;

			if( xLength > yLength )
				return 1;

			return -1;
		}
	}
}