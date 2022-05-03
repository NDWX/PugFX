using System.Collections;

namespace Pug.Collections.Extensions
{
    public static class BitArrayExtension
    {
        private static readonly byte[] bitDictionary = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };
        private static readonly byte byteLength = 8;

        public static byte[] GetBytes(this BitArray bitArray, int start, int length)
        {
            BitArray resultArray = new BitArray(length);

            for (int idx = 0; idx < length; idx++)
            {
                resultArray.Set(idx, bitArray.Get(start + idx));
            }

            return resultArray.GetBytes();
        }

        public static byte[] GetBytes(this BitArray bitArray)
        {
            byte currentByteBitIdx = 0;
            byte currentByte = 0;
            int resultIdx = 0;

            byte bitValue = 0;

            byte[] result = new byte[(bitArray.Length / 8) + 1];

            for (int idx = 0; idx < bitArray.Length; idx++)
            {
                bitValue = (byte)(bitArray.Get(idx) ? 1 : 0);

                currentByte += (byte)(bitValue * bitDictionary[currentByteBitIdx]);

                currentByteBitIdx++;

                if (currentByteBitIdx >= byteLength)
                {
                    currentByteBitIdx = 0;
                    result[resultIdx] = currentByte;

                    resultIdx++;
                    currentByte = 0;
                }
            }

            return result;
        }
    }
}
