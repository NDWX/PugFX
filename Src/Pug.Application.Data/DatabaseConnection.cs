using System;
using System.Data;
using System.Data.Common;
#if NETFX
using System.Transactions;
#endif

namespace Pug.Application.Data
{
    internal partial class DatabaseConnection : IDbConnection
    {
        IDbConnection connection = null;
        DateTime sessionStart;
        DateTime sessionEnd = DateTime.MaxValue;

        internal DatabaseConnection(IDbConnection connection)
        {
            this.connection = connection;

            sessionStart = DateTime.Now;
            //this.dbProviderFactory = dbProviderFactory;
        }

        public string ConnectionString { get => connection.ConnectionString; set => connection.ConnectionString = value; }

        public int ConnectionTimeout => connection.ConnectionTimeout;

        public string Database => connection.Database;

        public ConnectionState State => connection.State;

        public IDbTransaction BeginTransaction()
        {
            return this.connection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return this.connection.BeginTransaction(isolationLevel);
        }

        public void ChangeDatabase(string databaseName)
        {
            connection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            connection.Close();
        }

        public IDbCommand CreateCommand()
        {
            return connection.CreateCommand();
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public void Open()
        {
            connection.Open();
        }

        IDbCommand CreateCommand(CommandInfo info, IDbTransaction transaction = null)
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
        public IDataReader ExecuteQuery(CommandInfo command, IDbTransaction transaction = null)
        {
            IDataReader dataReader = null;

            using (IDbCommand _command = CreateCommand(command, transaction))
            {
                dataReader = _command.ExecuteReader(command.Behavior);
            }

            return dataReader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotConnected"></exception>
        public void Execute(CommandInfo command, IDbTransaction transaction = null)
        {
            using (IDbCommand _command = CreateCommand(command, transaction))
            {
                _command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotConnected"></exception>
        public object ExecuteScalar(CommandInfo command, IDbTransaction transaction = null)
        {
            object scalarValue = null;

            using (IDbCommand _command = CreateCommand(command, transaction))
            {
                scalarValue = _command.ExecuteScalar();
            }

            return scalarValue;
        }
    }
}