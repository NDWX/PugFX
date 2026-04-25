using System;

namespace Pug.Lang
{
	public record Option<TValue> : IOption
	{
		public Option()
		{
			
		}
		
		public Option( TValue value )
		{
			Value = value;
		}
		
		public static implicit operator Option<TValue>(TValue t) => new (t);

		public TValue Value { get; set; }

		public Unit AsParameterOf( Action<TValue> action )
		{
			action( Value );

			return Unit.Value;
		}

		public Result<TResult> Map<TResult>( Func<TValue, TResult> function )
		{
			return new Result<TResult>(function( Value ));
		}

		public bool Is<TOptionType>()
		{
			return Value.GetType() == typeof(TOptionType);
		}
	}
}