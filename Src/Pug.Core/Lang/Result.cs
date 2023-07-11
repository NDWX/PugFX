namespace Pug.Lang
{
	public record Result<TResult>
	{
		public Result()
		{
		}

		public Result( TResult value )
		{
			Value = value;
		}

		public TResult Value { get; set; }

		public bool HasValue => Value is not null;

		public static Result<TResult> Of( TResult result )
		{
			return new Result<TResult>( result );
		}
	}
}