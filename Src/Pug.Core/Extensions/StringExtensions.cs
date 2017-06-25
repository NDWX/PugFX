using System.Text;

namespace Pug.Extensions
{
	public static class StringExtensions
	{
		public static string Pad(this string original, char paddingCharacter, int desiredLength)
		{
			StringBuilder result = new StringBuilder(original);

			while (result.Length < desiredLength)
				result.Append(paddingCharacter);
			
			return result.ToString();
		}

		public static string Pad(this string original, string padding, int desiredLength)
		{
			StringBuilder result = new StringBuilder(original);

			while (result.Length < desiredLength)
				result.Append(padding);

			return result.ToString();
		}

		public static string PrePad(this string original, char paddingCharacter, int desiredLength)
		{
			StringBuilder result = new StringBuilder(original);

			while (result.Length < desiredLength)
				result.Insert(0, paddingCharacter);

			return result.ToString();
		}

		public static string PrePad(this string original, string padding, int desiredLength)
		{
			StringBuilder result = new StringBuilder(original);

			while (result.Length < desiredLength)
				result.Insert(0, padding);

			return result.ToString();
		}
	}
}
