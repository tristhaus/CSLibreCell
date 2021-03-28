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

using Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoreTest
{
    [TestClass]
    public class ByteConverterTest
    {
        [TestMethod]
        public void ShouldProduceCorrectBytes()
        {
            // Arrange
            uint input = 0b_1010_1111_0000_0101;

            // Act
            var bytes = ByteConverter.UintToLSBBytes(input);

            // Assert
            Assert.AreEqual(bytes[0], 0b_0000_0101);
            Assert.AreEqual(bytes[1], 0b_1010_1111);
            Assert.AreEqual(bytes[2], 0b0);
            Assert.AreEqual(bytes[3], 0b0);
        }

        [TestMethod]
        public void ShouldCreateCorrectlyFromBytes()
        {
            // Arrange
            uint reference = 0b_1010_1111_0000_0101;

            // Act
            var input = new ArraySegment<byte>(new byte[] { 0b_0000_0101, 0b_1010_1111, 0b0, 0b0 });

            var number = ByteConverter.LSBBytesToUint(input);

            // Assert
            Assert.AreEqual(reference, number);
        }

        [TestMethod]
        public void RoundTrippingShouldWork()
        {
            // Arrange
            uint number1 = 1;
            uint number2 = 64321;
            uint number3 = uint.MaxValue;

            // Act
            var bytes1 = new ArraySegment<byte>(ByteConverter.UintToLSBBytes(number1));
            var bytes2 = new ArraySegment<byte>(ByteConverter.UintToLSBBytes(number2));
            var bytes3 = new ArraySegment<byte>(ByteConverter.UintToLSBBytes(number3));

            var result1 = ByteConverter.LSBBytesToUint(bytes1);
            var result2 = ByteConverter.LSBBytesToUint(bytes2);
            var result3 = ByteConverter.LSBBytesToUint(bytes3);

            // Assert
            Assert.AreEqual(number1, result1);
            Assert.AreEqual(number2, result2);
            Assert.AreEqual(number3, result3);
        }
    }
}
