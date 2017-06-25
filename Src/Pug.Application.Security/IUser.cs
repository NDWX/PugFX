namespace Pug.Application.Security
{
	/// <summary>
	/// An IUser object encapsulates a user's identity and provides a single point of entry for all user related security.
	/// </summary>
	public interface IUser
	{
		Pug.Application.Security.IPrincipalIdentity Identity { get; }

		bool IsAuthorized(string operation, System.Collections.Generic.ICollection<string> objectNames, System.Collections.Generic.IDictionary<string, string> context);

		void Dispose();
	}
}
