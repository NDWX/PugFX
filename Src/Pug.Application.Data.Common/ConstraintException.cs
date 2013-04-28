using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Pug.Application.Data
{
    public class ConstraintViolation : DatabaseError
    {
        public ConstraintViolation(string message, DbException exception)
            : base(message, exception)
        {
        }
    }
}
