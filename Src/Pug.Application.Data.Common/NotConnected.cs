using System.Data.Common;

namespace Pug.Application.Data
{
	//[Serializable]
	public class NotConnected : DatabaseError
	{
		public NotConnected()
		{
		}

		public NotConnected(string message) : base(message)
		{
		}

		public NotConnected(string message, DbException innerException)
			: base(message, innerException)
		{
		}

		//protected NotConnected(SerializationInfo info, StreamingContext context)
		//    : base(info, context)
		//{
		//}
	}
}
