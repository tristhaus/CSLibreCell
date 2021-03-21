using System;

namespace Core.Internal
{
    /// <summary>
    /// Helper class to convert four <see cref="Byte"/> to an <see cref="uint"/> and vice versa.
    /// </summary>
    /// <remarks>
    /// Used for serialization purposes and backwards compatibility.
    /// </remarks>
    internal static class ByteConverter
    {
        /// <summary>
        /// Converts an unsigned integer to an array of four bytes, least significant byte first.
        /// </summary>
        /// <param name="input">The integer to convert.</param>
        /// <returns>An array of bytes.</returns>
        internal static byte[] UintToLSBBytes(uint input)
        {
            return new byte[] { ConvertToByte(input, 0), ConvertToByte(input, 8), ConvertToByte(input, 16), ConvertToByte(input, 24), };
        }

        /// <summary>
        /// Converts an array of four bytes, least significant byte first, to an unsigned integer.
        /// </summary>
        /// <param name="input">The bytes to convert.</param>
        /// <returns>An unsigned integer.</returns>
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
