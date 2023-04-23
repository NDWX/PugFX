﻿namespace Pug
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
	}
}