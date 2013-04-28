using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Pug.Application.Data
{
    public class SchemaRaisedError : DatabaseError
    {
        public SchemaRaisedError(string message, DbException exception)
            : base(message, exception)
        {
        }
    }

}
