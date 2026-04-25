using System;

namespace Pug.Lang
{
	public record OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : IOneOf
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

		public OneOf(TFifth value)
		{
			Fifth = value;
		}

		public OneOf( TSixth value )
		{
			Sixth = value;
		}

		public TFirst First { get; set; }
		
		public TSecond Second { get; set; }

		public TThird Third { get; set; }

		public TFourth Fourth { get; set; }

		public TFifth Fifth { get; set; }

		public TSixth Sixth { get; set; }
		
		public bool Is<TOption>()
		{
			return First is TOption || Second is TOption || Third is TOption;
		}
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(TFirst value) => new () { First = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(TSecond value) => new () { Second = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(TThird value) => new () { Third = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(TFourth value) => new () { Fourth = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(TFifth value) => new () { Fifth = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(TSixth value) => new () { Sixth = value };

		public void When( Action<TFirst> onFirst, Action<TSecond> onSecond, Action<TThird> onThird, Action<TFourth> onFourth, Action<TFifth> onFifth, Action<TSixth> onSixth )
		{
			Unit _ = First?.AsParameterOf( onFirst ) ??
					Second?.AsParameterOf( onSecond ) ??
					Third?.AsParameterOf( onThird ) ??
					Fourth?.AsParameterOf( onFourth )??
					Fifth?.AsParameterOf( onFifth )??
					Sixth?.AsParameterOf( onSixth );
		}
		
		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, Func<TThird, TResult> onThird, Func<TFourth, TResult> onFourth, Func<TFifth,TResult> onFifth, Func<TSixth,TResult> onSixth )
			where TResult : class
		{
			Result<TResult> result = First?.Map( onFirst ) ??
									Second?.Map( onSecond ) ??
									Third?.Map( onThird ) ??
									Fourth?.Map( onFourth )??
									Fifth?.Map( onFifth )??
									Sixth?.Map( onSixth );

			return result?.Value;
		}

		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, Func<TThird, TResult> onThird, Func<TFourth, TResult> onFourth, Func<TFifth,TResult> onFifth, Func<TSixth,TResult> onSixth, TResult nullResult )
		{
			Result<TResult> result = First?.Map( onFirst ) ??
									Second?.Map( onSecond ) ??
									Third?.Map( onThird ) ??
									Fourth?.Map( onFourth )??
									Fifth?.Map( onFifth )??
									Sixth?.Map( onSixth );

			return result is null ? nullResult : result.Value;
		}
	}
	
	public record OneOf<TFirst, TSecond, TThird, TFourth, TFifth> : IOneOf
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

		public OneOf(TFifth value)
		{
			Fifth = value;
		}

		public TFirst First { get; set; }
		
		public TSecond Second { get; set; }

		public TThird Third { get; set; }

		public TFourth Fourth { get; set; }

		public TFifth Fifth { get; set; }
		
		public bool Is<TOption>()
		{
			return First is TOption || Second is TOption || Third is TOption;
		}
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth>(TFirst value) => new () { First = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth>(TSecond value) => new () { Second = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth>(TThird value) => new () { Third = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth>(TFourth value) => new () { Fourth = value };
		
		public static implicit operator OneOf<TFirst, TSecond, TThird, TFourth, TFifth>(TFifth value) => new () { Fifth = value };

		public void When( Action<TFirst> onFirst, Action<TSecond> onSecond, Action<TThird> onThird, Action<TFourth> onFourth, Action<TFifth> onFifth )
		{
			Unit _ = First?.AsParameterOf( onFirst ) ??
					Second?.AsParameterOf( onSecond ) ??
					Third?.AsParameterOf( onThird ) ??
					Fourth?.AsParameterOf( onFourth )??
					Fifth?.AsParameterOf( onFifth );
		}
		
		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, Func<TThird, TResult> onThird, Func<TFourth, TResult> onFourth, Func<TFifth,TResult> onFifth )
			where TResult : class
		{
			Result<TResult> result = First?.Map( onFirst ) ??
									Second?.Map( onSecond ) ??
									Third?.Map( onThird ) ??
									Fourth?.Map( onFourth )??
									Fifth?.Map( onFifth );

			return result?.Value;
		}

		public TResult When<TResult>( Func<TFirst, TResult> onFirst, Func<TSecond, TResult> onSecond, Func<TThird, TResult> onThird, Func<TFourth, TResult> onFourth, Func<TFifth,TResult> onFifth, TResult nullResult )
		{
			Result<TResult> result = First?.Map( onFirst ) ??
									Second?.Map( onSecond ) ??
									Third?.Map( onThird ) ??
									Fourth?.Map( onFourth )??
									Fifth?.Map( onFifth );

			return result is null ? nullResult : result.Value;
		}
	}
	
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

		public TFirst First { get; set; }
		
		public TSecond Second { get; set; }

		public TThird Third { get; set; }

		public TFourth Fourth { get; set; }
		
		public bool Is<TOption>()
		{
			return First is TOption || Second is TOption || Third is TOption;
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

		public TFirst First { get; set; }
		
		public TSecond Second { get; set; }

		public TThird Third { get; set; }
		
		public bool Is<TOption>()
		{
			return First is TOption || Second is TOption || Third is TOption;
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

		public TFirst First { get; set; }
		
		public TSecond Second { get; set; }
		
		public bool Is<TOption>()
		{
			return First is TOption || Second is TOption;
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