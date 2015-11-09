using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug
{
	public interface IGetAndRemember<K, V>
		where K : IEquatable<K>, IComparable<K>
	{
		V this[K key]
		{
			get;
		}

		V Refresh(K key);

		void Throttle();

		void Trim();

		void ForgetAll();
	}
}
