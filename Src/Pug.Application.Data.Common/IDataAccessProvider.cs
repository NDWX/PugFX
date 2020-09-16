using System.Data.Common;

namespace Pug.Application.Data
{
	public interface IDataAccessProvider
	{
		DbProviderFactory DbProviderFactory
		{
			get;
		}

		DataExceptionHandler DataExceptionHandler
		{
			get;
		}
	}
}
