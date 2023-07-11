using System;

namespace Pug
{
	public class Range<T> : IRange<T>
		where T : struct, IComparable<T>
	{
		public Range()
		{
		}
		
		public Range(T? start, T? end)
		{
			Start = start;
			End = end;
		}
		
		public T? Start { get; set; }
		public T? End { get; set; }
	}
}
