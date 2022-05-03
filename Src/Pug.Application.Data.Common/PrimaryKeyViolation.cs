using System.Data.Common;

namespace Pug.Application.Data
{
    public class PrimaryKeyViolation : DatabaseError
    {

        public PrimaryKeyViolation(string message, DbException exception)
            : base(message, exception)
        {
        }
    }
}
