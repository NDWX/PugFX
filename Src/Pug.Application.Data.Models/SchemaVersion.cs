using System.Collections.Generic;

namespace Pug.Application.Data
{
	public class SchemaVersion
	{
		public SchemaVersion(int number, IEnumerable<UpgradeScript> upgradeScripts)
		{
			Number = number;
			UpgradeScripts = upgradeScripts;
		}

		public int Number { get; }
			
		public IEnumerable<UpgradeScript> UpgradeScripts { get; }

		public void Deconstruct( out int number, out IEnumerable<UpgradeScript> upgradeScripts)
		{
			number = Number;
			upgradeScripts = UpgradeScripts;
		}
	}
}