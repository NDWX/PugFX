using System;

using System.Data.SQLite;

namespace Pug.Application.Data.SqlLite
{
	public class DataAccessProvider : IDataAccessProvider
	{
		public System.Data.Common.DbProviderFactory DbProviderFactory
		{
			get { return System.Data.SQLite.SQLiteFactory.Instance; }
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
