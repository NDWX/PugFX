using System.Data.Common;

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
