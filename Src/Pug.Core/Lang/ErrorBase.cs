using System.Runtime.Serialization;

namespace Pug
{
	public record ErrorBase
	{
		public string Message { get; set; }

		protected ErrorBase( string message = "" )
		{
			this.Message = message;
		}
	}
}