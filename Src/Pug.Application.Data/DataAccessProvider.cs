using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Pug.Application.Data
{
    public interface IConnection
    {
        void BeginTransaction();

        void BeginTransaction( System.Data.IsolationLevel isolationLevel );

        void RollbackTransaction();

        void CommitTransaction();


        int Execute( string sql, object parameter );

        void ExecuteReader( string sql, object parameter, Action<DbDataReader> action );

        object ExecuteScalar( string sql, object parameter );
    }

    public abstract class DataAccessProvider : IDataAccessProvider
    {
        public DbProviderFactory DbProviderFactory => throw new NotImplementedException();

        public DataExceptionHandler DataExceptionHandler => throw new NotImplementedException();

        string connectionString;

        public DataAccessProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void 

        public string ConnectionString => this.connectionString;
    }
}
