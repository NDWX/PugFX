using System;

namespace Pug
{
	public interface IRange<T> : IReadOnlyRange<T>
		where T : struct, IComparable<T>
	{
		public new T? Start
		{
			get;
			set;
		}
	
		public new T? End
		{
			get;
			set;
		}
	}
}