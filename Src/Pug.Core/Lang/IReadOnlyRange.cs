using System;

namespace Pug
{
	public interface IReadOnlyRange<T>
		where T : struct, IComparable<T>
	{
		public new T? Start
		{
			get;
		}
	
		public new T? End
		{
			get;
		}
	}
}