namespace Pug
{
	public record UnexpectedError : ErrorBase
	{
		public UnexpectedError( string message ) : base( message )
		{
			
		}
	}
}