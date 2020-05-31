using System.Numerics;
using System.Security.Cryptography;

namespace Implementeringsprojekt {
    public class BitGenerator {
        public static BigInteger RandomInRange(BigInteger min, BigInteger max)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            if (min > max)
            {
                var buff = min;
                min = max;
                max = buff;
            }

            // offset to set min = 0
            BigInteger offset = -min;
            min = 0;
            max += offset;

            var value = BitGenerator.RandomInRangeFromZeroToPositive(rng, max) - offset;
            return value;
        }

        private static BigInteger RandomInRangeFromZeroToPositive(RandomNumberGenerator rng, BigInteger max)
        {
            BigInteger value;
            var bytes = max.ToByteArray();
            byte zeroBitsMask = 0b00000000;
            var mostSignificantByte = bytes[bytes.Length - 1];
            for (var i = 7; i >= 0; i--)
            {
                if ((mostSignificantByte & (0b1 << i)) == 0) {
                    continue;
                }

                var zeroBits = 7 - i;
                zeroBitsMask = (byte)(0b11111111 >> zeroBits);
                break;
            }
            do {
                rng.GetBytes(bytes);
                bytes[bytes.Length - 1] &= zeroBitsMask;
                value = new BigInteger(bytes);
            } while (value > max);
            return value;
        }

        public static BigInteger[] RandomArray(int length) {
            BigInteger[] array = new BigInteger[length];
            for (int i = 0; i <= length - 1; i++) {
                array[i] = BitGenerator.RandomInRange(0, BigInteger.Pow(2, 89) - 1);
            }
            return array;
        }
    }
}