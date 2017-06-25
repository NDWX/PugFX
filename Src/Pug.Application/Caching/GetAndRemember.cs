using System;
using System.Collections.Generic;

namespace Pug.Application.Caching
{
	public class LocalGetAndRemember<K, V> : ICache<K, V> where K : IEquatable<K>, IComparable<K>
	{
		IDictionary<K, V> remembered;
		Func<K, V> source;

		private LocalGetAndRemember(Func<K, V> source)
		{
			remembered = new Dictionary<K, V>();
			this.source = source;
		}

		public static LocalGetAndRemember<K, V> From(Func<K, V> source)
		{
			return new LocalGetAndRemember<K, V>(source);
		}

		public V this[K key]
		{
			get
			{
				V value;

				if (remembered.ContainsKey(key))
				{
					value = remembered[key];
				}
				else
				{
					value = source(key);
					remembered.Add(key, value);
				}

				return value;
			}
		}

		public V Refresh(K key)
		{
			V value = source(key);
			remembered[key] = value;

			return value;
		}

		public void ForgetAll()
		{
			remembered.Clear();
		}

		public void Throttle()
		{
			throw new NotImplementedException();
		}

		public void Trim()
		{
			throw new NotImplementedException();
		}
	}
}
