namespace Pug.Lang
{
	public record UnexpectedError : ErrorBase
	{
		public UnexpectedError( string message ) : base( message )
		{
			
		}
	}
}