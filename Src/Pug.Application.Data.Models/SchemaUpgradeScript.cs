namespace Pug.Application.Data
{
	public class SchemaUpgradeScript
	{
		public SchemaUpgradeScript(int schemaVersion, int sequence, UpgradeScript upgradeScript)
		{
			SchemaVersion = schemaVersion;
			Sequence = sequence;
			UpgradeScript = upgradeScript;
		}

		public int SchemaVersion { get; }
			
		public int Sequence { get; }
			
		public UpgradeScript UpgradeScript { get; }

		public void Deconstruct( out int schemaVersion, out int sequence, out UpgradeScript upgradeScript)
		{
			schemaVersion = SchemaVersion;
			sequence = Sequence;
			upgradeScript = UpgradeScript;
		}
	}
}