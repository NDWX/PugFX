using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Pug.Application.Data
{
	public abstract class ApplicationData<T> : IApplicationData<T> where T : IApplicationDataSession
	{
		string location;
		IDataAccessProvider dataProvider;

		protected ApplicationData(string location, IDataAccessProvider dataProvider)
		{
			this.location = location;
			this.dataProvider = dataProvider;
		}

		protected string Location
		{
			get
			{
				return this.location;
			}
		}

		protected IDataAccessProvider DataAccessProvider
		{
			get
			{
				return this.dataProvider;
			}
		}

		public abstract T GetSession();
	}
}
