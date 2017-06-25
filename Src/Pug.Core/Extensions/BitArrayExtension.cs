using System;
using System.Collections;

namespace Pug.Extensions
{
	public static class BitArrayExtension
	{
		public static byte[] GetBytes(this BitArray bitArray, int start, int length)
		{
			// reduce calculation time by storing comparison length
			int lastBitIdx = start + length - 1;

			if( lastBitIdx > bitArray.Length )
				throw new ArgumentOutOfRangeException("length");

			byte[] bitDictionary = new byte[] {1, 2, 4, 8, 16, 32, 64, 128};

			byte[] result = new byte[(length / 8) + 1];

			byte currentByte;

			int nextBitIdx = 0;

			for (int byteIdx = 0; byteIdx < result.Length; byteIdx++)
			{
				currentByte = 0;

				nextBitIdx = start + (8 * byteIdx) + 0;

				for (byte currentByteBitIdx = 0; currentByteBitIdx < 7 && nextBitIdx <= lastBitIdx; currentByteBitIdx++)
				{
					if( bitArray[nextBitIdx] )
						currentByte += bitDictionary[currentByteBitIdx];

					nextBitIdx = start + (8 * byteIdx) + currentByteBitIdx + 1;
				}

				result[byteIdx] = currentByte;
			}

			return result;
		}
	}
}
