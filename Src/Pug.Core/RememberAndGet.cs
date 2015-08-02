using System;
using System.Collections.Generic;

namespace Pug
{
	public class GetAndRemember<K, V>
	{
		IDictionary<K, V> remembered;
		Func<K, V> source;

		private GetAndRemember(Func<K, V> source)
		{
			remembered = new Dictionary<K, V>();
			this.source = source;
		}

		public static GetAndRemember<K, V> From(Func<K, V> source)
		{
			return new GetAndRemember<K, V>(source);
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
	}
}
