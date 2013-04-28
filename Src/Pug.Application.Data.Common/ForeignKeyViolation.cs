using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Pug.Application.Data
{
    public class ForeignKeyViolation : DatabaseError
    {
        public ForeignKeyViolation(string message, DbException exception)
            : base(message, exception)
        {
        }
    }
}
