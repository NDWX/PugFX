using System;

namespace Pug.Application.Caching
{
	public interface ICache<K, V>
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
