using System.Text;

using System.Security;
using System.Security.Cryptography;

namespace Pug.Application.Security.Extensions
{
	public static class StringExtensions
	{
		public static SecureString ToSecureString(this string text)
		{
			SecureString secureString = new SecureString();

			char[] chars = text.ToCharArray();

			foreach( char character in chars )
				secureString.AppendChar(character);

			return secureString;
		}

		public static byte[] ComputeHashUsing(this string text, HashAlgorithm algorithm)
		{
			byte[] data = Encoding.Unicode.GetBytes(text);

			return algorithm.ComputeHash(data);
		}
	}
}
