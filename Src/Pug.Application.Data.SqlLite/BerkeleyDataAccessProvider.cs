using System;

using System.Data.Berkeley.SQLite;

namespace Pug.Application.Data.SqlLite.Berkeley
{
	public class DataAccessProvider : IDataAccessProvider
	{
		public System.Data.Common.DbProviderFactory DbProviderFactory
		{
			get { return SQLiteFactory.Instance; }
		}

		public DataExceptionHandler DataExceptionHandler
		{
			get { return new DataExceptionHandler(this.Handle); }
		}

		public void Handle(Exception exception)
		{
			throw exception;
		}
	}
}
