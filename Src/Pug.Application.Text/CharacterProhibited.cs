using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Security.Cryptography
{
	public class CharacterProhibited : Exception
	{
		public CharacterProhibited(char prohibitedCharacter)
			: base(string.Format("'{0}' is a prohibited character.", prohibitedCharacter))
		{
			this.ProhibitedCharacter = prohibitedCharacter;
			this.Profile = string.Empty;
		}

		public CharacterProhibited(char prohibitedCharacter, string profile)
			: base(string.Format("'{0}' is a prohibited character for profile: {1}.", prohibitedCharacter, profile))
		{
			this.ProhibitedCharacter = prohibitedCharacter;
			this.Profile = profile;
		}

		public char ProhibitedCharacter
		{
			get;
			protected set;
		}

		string Profile
		{
			get;
			protected set;
		}
	}
}
