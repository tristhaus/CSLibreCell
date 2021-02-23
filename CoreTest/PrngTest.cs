using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CoreTest
{
    /// <summary>
    /// Tests for the Pseudo-Random Number Generator.
    /// </summary>
    [TestClass]
    public class PrngTest
    {
        [TestMethod]
        public void CreationOfCorrectNumbersShouldWork1()
        {
            // Arrange
            var prng = new Prng();
            prng.Initialize(1);

            // values from original C++ LibreCell implementation
            var reference = new List<uint> { 2745024 / 65536, 1210316419 / 65536 };

            // Act
            var result = new List<uint>(reference.Count);

            for (int i = 0; i < reference.Count; i++)
            {
                result.Add(prng.GetNext());
            }

            // Assert
            Assert.IsTrue(reference.SequenceEqual(result));
        }

        public void CreationOfCorrectNumbersShouldWork2()
        {
            // Arrange
            var prng = new Prng();
            prng.Initialize(0);

            // values from Rosetta Code
            var reference = new List<uint>
            {
                38,
                7719,
                21238,
                2437,
                8855,
                11797,
                8365,
                32285,
                10450,
                30612
            };

            // Act
            var result = new List<uint>(reference.Count);

            for (int i = 0; i < reference.Count; i++)
            {
                result.Add(prng.GetNext());
            }

            // Assert
            Assert.IsTrue(reference.SequenceEqual(result));
        }
    }
}
