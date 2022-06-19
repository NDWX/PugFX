
using System.Collections.Generic;

namespace Pug.Application.Data
{
	public interface IApplicationData<out T> where T : class, IApplicationDataSession
	{
		T GetSession();
		
		IEnumerable<SchemaUpgradeScript> GetSchemaUpgradeScripts( int currentVersion );
		
		IEnumerable<SchemaUpgradeScript> GetSchemaUpgradeScripts();
	}
}