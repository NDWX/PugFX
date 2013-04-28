using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pug.Extensions;

namespace Pug
{
	public class Base32
	{
		/*
		static string GetString( byte data)
		{
			string result = string.Empty;

			switch( data)
			{
				case 0:
					result = "A";
					break;

				case 1:
					result = "B";
					break;

				case 2:
					result = "C";
					break;

				case 3:
					result = "D";
					break;

				case 4:
					result = "E";
					break;

				case 5:
					result = "F";
					break;

				case 6:
					result = "G";
					break;

				case 7:
					result = "H";
					break;

				case 8:
					result = "I";
					break;

				case 9:
					result = "J";
					break;

				case 10:
					result = "K";
					break;

				case 11:
					result = "L";
					break;

				case 12:
					result = "M";
					break;

				case 13:
					result = "N";
					break;

				case 14:
					result = "O";
					break;

				case 15:
					result = "P";
					break;
				
				case 16:
					result = "Q";
					break;

				case 17:
					result = "R";
					break;

				case 18:
					result = "S";
					break;

				case 19:
					result = "T";
					break;

				case 20:
					result = "U";
					break;

				case 21:
					result = "V";
					break;

				case 22:
					result = "W";
					break;

				case 23:
					result = "X";
					break;

				case 24:
					result = "Y";
					break;

				case 25:
					result = "Z";
					break;

				case 26:
					result = "2";
					break;

				case 27:
					result = "3";
					break;

				case 28:
					result = "4";
					break;

				case 29:
					result = "5";
					break;

				case 30:
					result = "6";
					break;

				case 31:
					result = "7";
					break;
			}

			return result;
		}

		static byte[] Expand(byte[] data)
		{
			byte[] result = new byte[] {0, 0, 0, 0, 0, 0, 0, 0};

			for (int idx = 0; idx < data.Length; idx++)
			{
				result[idx] = data[idx];
			}

			return result;
		}

		static string GetString(byte[] data)
		{
			StringBuilder result = new StringBuilder();

			// 5, 3+2, 5, 1+4, 4 + 1, 5, 2+3, 5

			int base32StringLength = (int)Math.Ceiling((double)((data.Length * 8) / 5));

			UInt64 dataNumber = BitConverter.ToUInt64(Expand(data), 0);

			byte character;

			character = BitConverter.GetBytes(dataNumber & 31)[0];

			result.Append(GetString(character));


			for (int idx = 1; idx < base32StringLength; idx++)
			{
				dataNumber = dataNumber >> 5;

				character = BitConverter.GetBytes(dataNumber & 31)[0];

				result.Append(GetString(character));
			}

			return result.ToString().Pad('=', 8);
		}

		public static string From(byte[] data)
		{
			StringBuilder result = new StringBuilder();

			byte[] dataSegment;

			int oddLength = data.Length % 5;
			int wordsLength = data.Length  - oddLength;

			int remainingDataLength = wordsLength;;

			for (int idx = 0; idx < wordsLength; idx += 5)
			{				
				dataSegment = new byte[5];

				Array.Copy(data, idx, dataSegment, 0, 5 );
				
				result.Append(GetString(dataSegment));
			}

			if (oddLength > 0)
			{
				dataSegment = new byte[oddLength];

				Array.Copy(data, wordsLength, dataSegment, 0, oddLength);
				result.Append(GetString(dataSegment));
			}

			return result.ToString();
		}


		static byte ValueOf(char character)
		{
			byte value = 0;

			switch( character)
			{
				case 'A':
					value = 0;
					break;

				case 'B':
					value = 1;
					break;

				case 'C':
					value = 2;
					break;

				case 'D':
					value = 3;
					break;

				case 'E':
					value = 4;
					break;

				case 'F':
					value = 5;
					break;

				case 'G':
					value = 6;
					break;

				case 'H':
					value = 7;
					break;

				case 'I':
					value = 8;
					break;

				case 'J':
					value = 9;
					break;

				case 'K':
					value = 10;
					break;

				case 'L':
					value = 11;
					break;

				case 'M':
					value = 12;
					break;

				case 'N':
					value = 13;
					break;

				case 'O':
					value = 14;
					break;

				case 'P':
					value = 15;
					break;

				case 'Q':
					value = 16;
					break;

				case 'R':
					value = 17;
					break;

				case 'S':
					value = 18;
					break;

				case 'T':
					value = 19;
					break;

				case 'U':
					value = 20;
					break;

				case 'V':
					value = 21;
					break;

				case 'W':
					value = 22;
					break;

				case 'X':
					value = 23;
					break;

				case 'Y':
					value = 24;
					break;

				case 'Z':
					value = 25;
					break;

				case '2':
					value = 26;
					break;

				case '3':
					value = 27;
					break;

				case '4':
					value = 28;
					break;

				case '5':
					value = 29;
					break;

				case '6':
					value = 30;
					break;

				case '7':
					value = 31;
					break;

				default:
					throw new ArgumentException(string.Format("'{0}' is not valid Base 32 character.", character));
			}

			return value;
		}

		static byte[] GetData(string word)
		{
			int firstPaddingPosition = word.IndexOf('=');
			int wordLength = firstPaddingPosition > 0? firstPaddingPosition : word.Length;

			int resultLength = (int)Math.Ceiling((double)(( wordLength * 5) / 8));

			UInt64 dataNumber = 0;

			dataNumber = ValueOf(word[0]);

			for (int idx = 1; idx < wordLength; idx++)
			{
				dataNumber = dataNumber << 5;
				dataNumber |= word[idx];
			}

			byte[] result = new byte[resultLength];

			byte[] dataNumberBytes = BitConverter.GetBytes(dataNumber);

			Array.Copy(dataNumberBytes, 0, result, 0, result.Length);

			return result;
		}

		public static byte[] GetBytes(string base32String)
		{
			List<byte> result = new List<byte>();

			for (int idx = 0; idx < base32String.Length; idx += 8)
			{
				result.AddRange(GetData(base32String.Substring(idx, 8)));
			}

			return result.ToArray();
		}

		*/

		//[ThreadStatic]
		//static Base32Encoder _encoder = new Base32Encoder();

		public static string From(byte[] data)
		{
			Base32Encoder _encoder = new Base32Encoder();

			string result = string.Empty;

			try
			{
				result = _encoder.Encode(data);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}

		public static byte[] ToBytes(string text)
		{
			Base32Encoder _encoder = new Base32Encoder();

			byte[] result = null;

			try
			{
				result = _encoder.Decode(text);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return result;
		}

	}
}
