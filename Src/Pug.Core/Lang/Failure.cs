namespace Pug.Lang
{
	public record Failure
	{
		public UnexpectedError Error { get; set; }

		public Failure()
		{

		}

		public Failure( UnexpectedError error )
		{
			Error = error;
		}

		public static implicit operator Failure( UnexpectedError _ ) => new ( _ );
	}

	public record Failure<T0> : OneOf<T0, UnexpectedError>
	{
		public Failure() : base()
		{
		}

		public Failure( T0 _ ) : base( _ )
		{
		}

		public Failure( UnexpectedError _ ) : base( _ )
		{
		}

		public static implicit operator Failure<T0>( T0 _ ) => new ( _ );

		public static implicit operator Failure<T0>( UnexpectedError _ ) => new ( _ );
	}

	public record Failure<T0, T1> : OneOf<T0, T1, UnexpectedError>
	{
		public Failure() : base()
		{
		}

		public Failure( T0 _ ) : base( _ )
		{
		}

		public Failure( T1 _ ) : base( _ )
		{
		}

		public Failure( UnexpectedError _ ) : base( _ )
		{
		}

		public static implicit operator Failure<T0, T1>( T0 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1>( T1 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1>( UnexpectedError _ ) => new ( _ );
	}

	public record Failure<T0, T1, T2> : OneOf<T0, T1, T2, UnexpectedError>
	{
		public Failure() : base()
		{
		}

		public Failure( T0 _ ) : base( _ )
		{
		}

		public Failure( T1 _ ) : base( _ )
		{
		}

		public Failure( T2 _ ) : base( _ )
		{
		}

		public Failure( UnexpectedError _ ) : base( _ )
		{
		}

		public static implicit operator Failure<T0, T1, T2>( T0 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2>( T1 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2>( T2 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2>( UnexpectedError _ ) => new ( _ );
	}

	public record Failure<T0, T1, T2, T3> : OneOf<T0, T1, T2, T3, UnexpectedError>
	{
		public Failure() : base()
		{
		}

		public Failure( T0 _ ) : base( _ )
		{
		}

		public Failure( T1 _ ) : base( _ )
		{
		}

		public Failure( T2 _ ) : base( _ )
		{
		}

		public Failure( T3 _ ) : base( _ )
		{
		}

		public Failure( UnexpectedError _ ) : base( _ )
		{
		}

		public static implicit operator Failure<T0, T1, T2, T3>( T0 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2, T3>( T1 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2, T3>( T2 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2, T3>( T3 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2, T3>( UnexpectedError _ ) => new ( _ );
	}

	public record Failure<T0, T1, T2, T3, T4> : OneOf<T0, T1, T2, T3, T4, UnexpectedError>
	{
		public Failure() : base()
		{
		}

		public Failure( T0 _ ) : base( _ )
		{
		}

		public Failure( T1 _ ) : base( _ )
		{
		}

		public Failure( T2 _ ) : base( _ )
		{
		}

		public Failure( T3 _ ) : base( _ )
		{
		}

		public Failure( T4 _ ) : base( _ )
		{
		}

		public Failure( UnexpectedError _ ) : base( _ )
		{
		}

		public static implicit operator Failure<T0, T1, T2, T3, T4>( T0 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2, T3, T4>( T1 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2, T3, T4>( T2 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2, T3, T4>( T3 _ ) => new ( _ );

		public static implicit operator Failure<T0, T1, T2, T3, T4>( UnexpectedError _ ) => new ( _ );
	}
}