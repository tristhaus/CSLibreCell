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

        [TestMethod]
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
