using System;

namespace Pug
{
	public class Range<T> where T : IComparable<T>
	{
		T start, end;

		public Range(T start, T end)
		{
			this.start = start;
			this.end = end;
		}

		public T Start
		{
			get
			{
				return start;
			}
		}

		public T End
		{
			get
			{
				return end;
			}
		}

		public bool Contains(T val)
		{
			return val.CompareTo(start) >= 0 && val.CompareTo(end) <= 0;
		}

		public static Range<T> Between(T start, T end)
		{
			return new Range<T>(start, end);
		}
	}
}
