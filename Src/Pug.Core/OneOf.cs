using System;

namespace Pug
{
	public record OneOf<TFirst, TSecond, TThird, TFourth> : IOneOf
	{
		public OneOf()
		{
		}

		public OneOf( TFirst value )
		{
			First = value;
		}

		public OneOf( TSecond value )
		{
			Second = value;
		}

		public OneOf(TThird value)
		{
			Third = value;
		}

		public OneOf(TFourth value)
		{
			Fourth = value;
		}

		public Option<TFirst> First { get; set; }
		
		public Option<TSecond> Second { get; set; }

		public Option<TThird> Third { get; set; }

		public Option<TFourth> Fourth { get; set; }
		
		public bool Is<TOption>()
		{
			return First is Option<TOption> || Second is Option<TOption> || Third is Option<TOption>;
		}
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth>(TFirst value) => new () { First = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth>(TSecond value) => new () { Second = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth>(TThird value) => new () { Third = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth>(TFourth value) => new () { Fourth = value };

		public void When( Action<TFirst> onFirst, Action<TSecond> onSecond, Action<TThird> onThird, Action<TFourth> onFourth )
		{
			Unit _ = First?.AsParameterOf( onFirst ) ??
					Second?.AsParameterOf( onSecond ) ??
					Third?.AsParameterOf( onThird ) ??
					Fourth?.AsParameterOf( onFourth );
		}
		
		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, Func<TThird, TResult> onThird, Func<TFourth, TResult> onFourth )
			where TResult : class
		{
			Result<TResult> result = First?.Map( onFirst ) ??
									Second?.Map( onSecond ) ??
									Third?.Map( onThird ) ??
									Fourth?.Map( onFourth );

			return result?.Value;
		}

		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, Func<TThird, TResult> onThird, Func<TFourth, TResult> onFourth, TResult nullResult )
		{
			Result<TResult> result = First?.Map( onFirst ) ??
									Second?.Map( onSecond ) ??
									Third?.Map( onThird ) ??
									Fourth?.Map( onFourth );

			return result is null ? nullResult : result.Value;
		}
	}
	
	public record OneOf<TFirst, TSecond, TThird> : IOneOf
	{
		public OneOf()
		{
		}

		public OneOf( TFirst value )
		{
			First = value;
		}

		public OneOf( TSecond value )
		{
			Second = value;
		}

		public OneOf(TThird value)
		{
			Third = value;
		}

		public Option<TFirst> First { get; set; }
		
		public Option<TSecond> Second { get; set; }

		public Option<TThird> Third { get; set; }
		
		public bool Is<TOption>()
		{
			return First is Option<TOption> || Second is Option<TOption> || Third is Option<TOption>;
		}
		
		public static implicit operator OneOf<TFirst, TSecond, TThird>(TFirst value) => new () { First = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird>(TSecond value) => new () { Second = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird>(TThird value) => new () { Third = value };

		public void When( Action<TFirst> onFirst, Action<TSecond> onSecond, Action<TThird> onThird )
		{
			Unit _ = First?.AsParameterOf( onFirst ) ??
					Second?.AsParameterOf( onSecond ) ??
					Third?.AsParameterOf( onThird );
		}
		
		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, Func<TThird, TResult> onThird )
			where TResult : class
		{
			Result<TResult> result = First?.Map( onFirst ) ??
									Second?.Map( onSecond ) ??
									Third?.Map( onThird );

			return result?.Value;
		}

		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, Func<TThird, TResult> onThird, TResult nullResult )
		{
			Result<TResult> result = First?.Map( onFirst ) ??
									Second?.Map( onSecond ) ??
									Third?.Map( onThird );

			return result is null ? nullResult : result.Value;
		}
	}
	
	public record OneOf<TFirst, TSecond> : IOneOf
	{
		public OneOf()
		{
		}

		public OneOf( TFirst value )
		{
			First = value;
		}

		public OneOf( TSecond value )
		{
			Second = value;
		}

		public Option<TFirst> First { get; set; }
		
		public Option<TSecond> Second { get; set; }
		
		public bool Is<TOption>()
		{
			return First is Option<TOption> || Second is Option<TOption>;
		}
		
		public static implicit operator OneOf<TFirst, TSecond>(TFirst t) => new () { First = t };
		
		public static implicit operator OneOf<TFirst, TSecond>(TSecond t) => new () { Second = t };

		public void When( Action<TFirst> onFirst, Action<TSecond> onSecond )
		{
			Unit _ = First?.AsParameterOf( onFirst ) ??
					Second?.AsParameterOf( onSecond );
		}
		
		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond )
			where TResult : class
		{
			Result<TResult> result = null;

			result = First?.Map( onFirst ) ??
					Second?.Map( onSecond );

			return result?.Value;
		}

		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, TResult nullResult )
		{
			Result<TResult> result = null;
			
			result = First?.Map( onFirst ) ??
					Second?.Map( onSecond );

			return result is null ? nullResult : result.Value;
		}
	}
}