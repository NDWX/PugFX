using System;
using System.Data.Common;
//using System.Runtime.Serialization;

namespace Pug.Application.Data
{
    public class DatabaseError : Exception
    {
        public DatabaseError()
        {
        }

        public DatabaseError(string message, DbException exception)
            : base(message, exception)
        {
        }

        //protected DatabaseError(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}
    }
}
