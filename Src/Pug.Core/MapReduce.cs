using System;
using System.Collections.Generic;

namespace Pug.MapReduce
{
	public static class MapReduce
	{
		public static IEnumerable<R> Map<I, R>(this IEnumerable<I> items, Func<I, R> func)
		{
			List<R> results = new List<R>();

			IEnumerator<I> enumerator = items.GetEnumerator();

			while (enumerator.MoveNext())
			{
				results.Add(func(enumerator.Current));
			}

			return results;
		}

		public static void Map<I, A>(this IEnumerable<I> items, Action<I, A> action, A argument)
		{
			IEnumerator<I> enumerator = items.GetEnumerator();

			while (enumerator.MoveNext())
			{
				action(enumerator.Current, argument);
			}
		}

		public static R Reduce<I, IR, R>(this IEnumerable<I> items, Func<I, R, R> reduction)
		{
			R result = default(R);

			IEnumerator<I> enumerator = items.GetEnumerator();

			while(enumerator.MoveNext())
			{
				result = reduction(enumerator.Current, result);
			}

			return result;
		}
	}
}

namespace Pug.MapReduce.Specialized
{
	public static class MapReduce
	{
		public static R[] Map<I, R>(this I[] items, Func<I, R> func)
		{
			R[] results = new R[items.Length];

			for (int idx = 0; idx < items.Length; idx++)
				results[idx] = func(items[idx]);

			return results;
		}

		public static void Map<I, A>(this I[] items, Action<I, A> action, A argument)
		{
			for (int idx = 0; idx < items.Length; idx++)
			{
				action(items[idx], argument);
			}
		}

		public static R Reduce<I, IR, R>(this I[] items, Func<I, R, R> reduction)
		{
			R result = default(R);

			for (int idx = 0; idx < items.Length; idx++)
			{
				result = reduction(items[idx], result);
			}

			return result;
		}
	}
}
