using System;

namespace Pug
{
	public class UnexpectedError : Exception
	{
		public UnexpectedError( string message ) : base( message )
		{
			
		}
	}
}