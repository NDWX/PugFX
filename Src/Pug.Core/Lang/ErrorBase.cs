using System.Runtime.Serialization;

namespace Pug.Lang
{
	public record ErrorBase
	{
		public string Message { get; set; }

		protected ErrorBase( string message = "" )
		{
			Message = message;
		}
	}
}