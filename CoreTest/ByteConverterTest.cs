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
