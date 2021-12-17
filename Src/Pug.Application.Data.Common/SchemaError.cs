using System.Data.Common;

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
