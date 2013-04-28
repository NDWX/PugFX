using System;

namespace Pug.Application.Data.SqlServer
{
	public class DataAccessProvider : IDataAccessProvider
	{
		public System.Data.Common.DbProviderFactory DbProviderFactory
		{
			get { return System.Data.SqlClient.SqlClientFactory.Instance; }
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
