namespace Pug.Lang
{
	public record Success<TResult> : Result<TResult>
	{
		public Success( TResult result ) : base( result )
		{
		}

		public static Success<TResult> From( TResult result )
		{
			return new Success<TResult>( result );
		}
	}
}