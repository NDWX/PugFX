using System.Collections.Generic;
using System.Data;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
        public class CommandInfo
        {
            public string Query { get; }
            public CommandType Type { get; }
            public ICollection<IDbDataParameter> Parameters { get; }
            public CommandBehavior Behavior { get; }
            public int Timeout { get; }

            public CommandInfo(string query, CommandType type, ICollection<IDbDataParameter> parameters, CommandBehavior behavior, int timeout)
            {
                Query = query;
                Type = type;
                Parameters = parameters;
                Behavior = behavior;
                Timeout = timeout;
            }
    }
}