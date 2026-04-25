namespace Pug.Lang
{
	public record Success
	{
		public static Success Value { get; } = new ();
	}

	public record Success<TValue> : Success
	{
		public Success( TValue value )
		{
			Value = value;
		}

		public new TValue Value { get; set; }

		public bool HasValue => Value is not null;

		public static Success<TValue> With( TValue value )
		{
			return new Success<TValue>( value );
		}
	}
}