using System;
using System.Data;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
    public static class IDbConnectionExtensions
    {
        public static IDbCommand CreateCommand(this IDbConnection connection, CommandInfo info, IDbTransaction transaction = null)
        {
            IDbCommand command = connection.CreateCommand();

            command.CommandText = info.Query;
            command.CommandType = info.Type;
            command.CommandTimeout = info.Timeout;

            if (transaction != null)
                command.Transaction = transaction;

            foreach (IDbDataParameter parameter in info.Parameters)
                command.Parameters.Add(parameter);


            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotConnected"></exception>
        [Obsolete("Use ExecuteQuery<R>( DbCommand command, Func<DbDataReader, R> func ) instead.")]
        public static IDataReader ExecuteQuery(this IDbConnection connection, CommandInfo command, IDbTransaction transaction = null)
        {
            IDataReader dataReader = null;

            using (IDbCommand _command = CreateCommand(connection, command, transaction))
            {
                dataReader = _command.ExecuteReader(command.Behavior);
            }

            return dataReader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotConnected"></exception>
        public static void Execute(this IDbConnection connection, CommandInfo command, IDbTransaction transaction = null)
        {
            using (IDbCommand _command = CreateCommand(connection, command, transaction))
            {
                _command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotConnected"></exception>
        public static object ExecuteScalar(this IDbConnection connection, CommandInfo command, IDbTransaction transaction = null)
        {
            object scalarValue = null;

            using (IDbCommand _command = CreateCommand(connection, command, transaction))
            {
                scalarValue = _command.ExecuteScalar();
            }

            return scalarValue;
        }

        public static void ExecuteQuery(this IDbConnection connection, CommandInfo command, Action<IDataReader> action, IDbTransaction transaction = null)
        {
#pragma warning disable 618
            using (IDataReader dataReader = connection.ExecuteQuery(command, transaction))
            {
                action(dataReader);
            }
#pragma warning restore 618
        }

        public static R ExecuteQuery<R>(this IDbConnection connection, CommandInfo command, Func<IDataReader, R> func, IDbTransaction transaction = null)
        {
            R returnValue = default(R);

#pragma warning disable 618
            using (IDataReader dataReader = connection.ExecuteQuery(command, transaction))
            {
                returnValue = func(dataReader);
            }
#pragma warning restore 618

            return returnValue;
        }
        
        public static void ExecuteQuery<TContext>(this IDbConnection connection, CommandInfo command, Action<IDataReader, TContext> action, TContext context = null, IDbTransaction transaction = null)
            where TContext: class
        {
#pragma warning disable 618
            using (IDataReader dataReader = connection.ExecuteQuery(command, transaction))
            {
                action(dataReader, context);
            }
#pragma warning restore 618
        }

        public static R ExecuteQuery<R, TContext>(this IDbConnection connection, CommandInfo command, Func<IDataReader, TContext, R> func, TContext context = null, IDbTransaction transaction = null)
            where TContext: class
        {
            R returnValue = default(R);

#pragma warning disable 618
            using (IDataReader dataReader = connection.ExecuteQuery(command, transaction))
            {
                returnValue = func(dataReader, context);
            }
#pragma warning restore 618

            return returnValue;
        }

    }
}