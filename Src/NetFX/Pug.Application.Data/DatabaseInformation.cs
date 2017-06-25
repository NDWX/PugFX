using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Pug.Application.Data
{
    public class DatabaseInformation
    {
        string connectionString;

        DbProviderFactory daoFactory;

        public DatabaseInformation(string connectionString, DbProviderFactory daoFactory)
        {
            this.connectionString = connectionString;
            this.daoFactory = daoFactory;
        }

		internal string ConnectionString
		{
			get
			{
				return connectionString;
			}
		}

        internal DbProviderFactory ProviderFactory
        {
            get
            {
                return this.daoFactory;
            }
        }
    }
}
