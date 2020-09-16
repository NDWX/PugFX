using System.Collections.Generic;

namespace Pug.Extensions
{
	public static class DictionaryExtensions
	{
		public static IDictionary<TKey, TValue> ReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
				return null;

			return new ReadOnlyDictionary<TKey, TValue>(dictionary);
		}
	}
}
