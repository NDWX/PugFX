using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Security.Cryptography
{
	public static class CharExtensions
	{
		internal static char[] rAndALCharacters;
		internal static char[] lCharacters;

		internal static bool IsIn(this char character, char[] list)
		{
			return list.Contains(character);
		}

		public static bool IsRAndALCharacter(this char character)
		{
			return character.IsIn(rAndALCharacters);
		}

		public static bool IsLCharacter(this char character)
		{
			return character.IsIn(rAndALCharacters);
		}
	}
}
