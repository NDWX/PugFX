using System;

namespace Pug.Lang
{
	public static class LanguageExtensions
	{
		public static Unit AsParameterOf<TValue>(this TValue value, Action<TValue> action)
		{
			action( value );

			return Unit.Value;
		}

		public static Result<TResult> Map<TValue, TResult>(this TValue value, Func<TValue, TResult> function )
		{
			return new Result<TResult>(function( value ));
		}
		
		public static bool Is<TValue, TOptionType>(this TValue value)
		{
			return value.GetType() == typeof(TOptionType);
		}
	}
}