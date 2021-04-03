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

using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTest
{
    /// <summary>
    /// Tests for the card factory.
    /// </summary>
    [TestClass]
    public class CardFactoryTest
    {
        [TestMethod]
        public void CardFactoryShouldCreateCorrectDeck()
        {
            // Arrange
            var cardFactory = new CardFactory();

            // Act
            var deck = cardFactory.CreateDeck();

            // Assert
            Assert.AreEqual(52, deck.Count);
            Assert.AreEqual("A♣", deck[0].UnicodeRepresentation);
            Assert.AreEqual("A♦", deck[1].UnicodeRepresentation);
            Assert.AreEqual("A♥", deck[2].UnicodeRepresentation);
            Assert.AreEqual("A♠", deck[3].UnicodeRepresentation);
            Assert.AreEqual("2♣", deck[4].UnicodeRepresentation);
            Assert.AreEqual("3♣", deck[8].UnicodeRepresentation);
            Assert.AreEqual("K♠", deck[51].UnicodeRepresentation);
        }
    }
}
