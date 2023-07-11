using System;

namespace Pug
{
	public class ReadOnlyRange<T> : IReadOnlyRange<T>
		where T : struct, IComparable<T>
	{
	
		public ReadOnlyRange( T? start, T? end )
		{
			Start = start;
			End = end;
		}
		
		public T? Start { get; }
		
		public T? End { get; }
	}
}