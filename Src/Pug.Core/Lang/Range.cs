using System;
using System.Runtime.Serialization;

namespace Pug
{
	[DataContract]
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
		
		[DataMember]
		public T? Start { get; set; }
		
		[DataMember]
		public T? End { get; set; }
	}
}
