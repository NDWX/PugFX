using System;
using System.Data.Common;
using System.Runtime.Serialization;

namespace Pug.Application.Data
{
    [Serializable]
    public class NotConnected : DatabaseError
    {
        public NotConnected()
            : base()
        {
        }

        public NotConnected(string message)
            : base()
        {
        }

        public NotConnected(string message, DbException innerException)
            : base(message, innerException)
        {
        }

        protected NotConnected(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
