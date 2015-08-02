using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pug.Security.Cryptography
{
	public static class StringExtensions
	{
		//internal static long FindFirstCharacter(this string text, char[] list)
		//{
		//    long index = 0;

		//    foreach (char character in text)
		//    {
		//        if (character.IsIn(list))
		//            return index;

		//        index++;
		//    }

		//    return -1;
		//}

		public static long FindFirstRAndALCharacter(this string text)
		{
			return text.IndexOfAny(CharExtensions.rAndALCharacters);
		}

		public static bool HasRAndALCharacter(this string text)
		{
			return text.FindFirstRAndALCharacter() > -1;
		}

		public static long FindFirstLCharacter(this string text)
		{
			return text.IndexOfAny(CharExtensions.lCharacters);
		}

		public static bool HasLCharacter(this string text)
		{
			return text.FindFirstLCharacter() > -1;
		}

		public static string Prepare(this string text, StringPrep.Profile profile)
		{
			StringPrep stringPrep = new StringPrep(nullprofile);
			return stringPrep.Prepare(text);
		}
	}
}
