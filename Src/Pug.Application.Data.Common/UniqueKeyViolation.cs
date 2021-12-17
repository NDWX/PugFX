using System.Data.Common;

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
