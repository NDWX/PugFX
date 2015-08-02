using System;

namespace Pug.Application.Data.Postgres
{
	public class DataAccessProvider : IDataAccessProvider
	{
		public System.Data.Common.DbProviderFactory DbProviderFactory
		{
			get { return Npgsql.NpgsqlFactory.Instance; }
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
