namespace Pug.Application.Data
{
	public class UpgradeScript
	{
		public UpgradeScript(string name, string description, string script, string rollbackScript)
		{
			Name = name;
			Description = description;
			Script = script;
			RollbackScript = rollbackScript;
		}

		public string Name { get; }

		public string Description { get; }

		public string Script { get; }

		public string RollbackScript { get; }

		public void Deconstruct( out string name, out string description, out string script, out string rollbackScript)
		{
			name = Name;
			description = Description;
			script = Script;
			rollbackScript = RollbackScript;
		}
	}
}