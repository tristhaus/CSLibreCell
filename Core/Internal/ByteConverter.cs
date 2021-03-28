/*
 * This file is part of CSLibreCell.
 * 
 * CSLibreCell is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * CSLibreCell is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with CSLibreCell.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

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
