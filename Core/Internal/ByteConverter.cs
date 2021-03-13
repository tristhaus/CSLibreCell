using System;

namespace Core.Internal
{
    internal class ByteConverter
    {
        internal static byte[] UintToLSBBytes(uint input)
        {
            return new byte[] { ConvertToByte(input, 0), ConvertToByte(input, 8), ConvertToByte(input, 16), ConvertToByte(input, 24), };
        }

        internal static uint LSBBytesToUint(ArraySegment<byte> input)
        {
            return ConvertFromeByte(input.Array[input.Offset]) + ConvertFromeByte(input.Array[input.Offset + 1]) * 0x100 + ConvertFromeByte(input.Array[input.Offset + 2]) * 0x10000 + ConvertFromeByte(input.Array[input.Offset + 3]) * 0x1000000;
        }

        private static byte ConvertToByte(uint input, int reverseBitOffset)
        {
            return Convert.ToByte((input >> reverseBitOffset) & 0xFF);
        }

        private static uint ConvertFromeByte(byte input)
        {
            return Convert.ToUInt32(input);
        }
    }
}
