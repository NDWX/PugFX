using System.Collections.Generic;

namespace Pug.Application.Security
{
	/// <summary>
	/// An IUser object encapsulates a user's identity and provides a single point of entry for all user related security.
	/// </summary>
	public interface IUser
	{
		Pug.Application.Security.IPrincipalIdentity Identity { get; }

		bool IsAuthorized( IDictionary<string, string> context, string operation, string objectType, string objectName = "" );

		void Dispose();
	}
}
