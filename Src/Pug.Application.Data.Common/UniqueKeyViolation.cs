using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Pug.Application.Data
{
    public class UniqueKeyViolation : DatabaseError
    {
        public UniqueKeyViolation(string message, DbException exception)
            : base(message, exception)
        {
        }
    }

}
