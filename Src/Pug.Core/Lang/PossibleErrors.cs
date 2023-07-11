namespace Pug.Lang
{
	public record PossibleErrors<T0> : OneOf<T0, UnexpectedError>
{
	public PossibleErrors( T0 _ ) : base( _ )
	{
	}

	public PossibleErrors( UnexpectedError _ ) : base( _ )
	{
	}

	public static implicit operator PossibleErrors<T0>( T0 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0>( UnexpectedError _ ) => new ( _ );
}

public record PossibleErrors<T0, T1> : OneOf<T0, T1, UnexpectedError>
{
	public PossibleErrors( T0 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T1 _ ) : base( _ )
	{
	}

	public PossibleErrors( UnexpectedError _ ) : base( _ )
	{
	}

	public static implicit operator PossibleErrors<T0, T1>( T0 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1>( T1 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1>( UnexpectedError _ ) => new ( _ );
}

public record PossibleErrors<T0, T1, T2> : OneOf<T0, T1, T2, UnexpectedError>
{
	public PossibleErrors( T0 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T1 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T2 _ ) : base( _ )
	{
	}

	public PossibleErrors( UnexpectedError _ ) : base( _ )
	{
	}
	
	public static implicit operator PossibleErrors<T0, T1, T2>( T0 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2>( T1 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2>( T2 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2>( UnexpectedError _ ) => new ( _ );
}

public record PossibleErrors<T0, T1, T2, T3> : OneOf<T0, T1, T2, T3, UnexpectedError>
{
	public PossibleErrors( T0 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T1 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T2 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T3 _ ) : base( _ )
	{
	}

	public PossibleErrors( UnexpectedError _ ) : base( _ )
	{
	}

	public static implicit operator PossibleErrors<T0, T1, T2, T3>( T0 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2, T3>( T1 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2, T3>( T2 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2, T3>( T3 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2, T3>( UnexpectedError _ ) => new ( _ );
}

public record PossibleErrors<T0, T1, T2, T3, T4> : OneOf<T0, T1, T2, T3, T4, UnexpectedError>
{
	public PossibleErrors( T0 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T1 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T2 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T3 _ ) : base( _ )
	{
	}
	
	public PossibleErrors( T4 _ ) : base( _ )
	{
	}

	public PossibleErrors( UnexpectedError _ ) : base( _ )
	{
	}

	public static implicit operator PossibleErrors<T0, T1, T2, T3, T4>( T0 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2, T3, T4>( T1 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2, T3, T4>( T2 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2, T3, T4>( T3 _ ) => new ( _ );

	public static implicit operator PossibleErrors<T0, T1, T2, T3, T4>( UnexpectedError _ ) => new ( _ );
}
}