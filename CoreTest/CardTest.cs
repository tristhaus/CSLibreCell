﻿/*
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
using System.Collections.Generic;
using System.Linq;

namespace CoreTest
{
    /// <summary>
    /// Tests for the card class.
    /// </summary>
    [TestClass]
    public class CardTest
    {
        [TestMethod]
        public void CardShouldHandleEqualsCorrectly()
        {
            // Arrange
            var referenceCard = new Card(7);
            var sameCard = new Card(7);
            var differentCard = new Card(9);

            // Act
            var sameResult = sameCard.Equals(referenceCard);
            var differentResult = differentCard.Equals(referenceCard);

            // Assert
            Assert.IsTrue(sameResult);
            Assert.IsFalse(differentResult);
        }

        [TestMethod]
        public void CardShouldGiveCorrectAsciiRepresentation()
        {
            // Arrange
            var card = new Card(12); // Four of Clubs

            // Act
            var result = card.AsciiRepresentation;
            var isBlack = card.IsBlack;

            // Assert
            Assert.AreEqual("4C", result);
            Assert.IsTrue(isBlack);
        }

        [TestMethod]
        public void CardShouldParseDefaultAsciiRepresentationCorrectly()
        {
            // Arrange
            var asciiRepresentation = "3S"; // Three of Spades

            // Act
            var result = Card.ParseFromDefaultAsciiRepresentation(asciiRepresentation);

            // Assert
            Assert.AreEqual(new Card(11), result);
        }

        [TestMethod]
        public void CardShouldCorrectlyRoundTripEntireDeckDefaultAscii()
        {
            // Arrange
            var referenceDeck = new List<Card>(52);
            for (uint i = 0; i < 52; i++)
            {
                referenceDeck.Add(new Card(i));
            }

            // Act
            var asciiRepresentationDeck = referenceDeck.Select(x => x.AsciiRepresentation).ToList();
            var result = asciiRepresentationDeck.Select(x => Card.ParseFromDefaultAsciiRepresentation(x)).ToList();

            // Assert
            Assert.IsTrue(referenceDeck.SequenceEqual(result));
        }

        [TestMethod]
        public void CardShouldCorrectlyRoundTripEntireDeckDefaultUnicode()
        {
            // Arrange
            var referenceDeck = new List<Card>(52);
            for (uint i = 0; i < 52; i++)
            {
                referenceDeck.Add(new Card(i));
            }

            // Act
            var unicodeRepresentationDeck = referenceDeck.Select(x => x.UnicodeRepresentation).ToList();
            var result = unicodeRepresentationDeck.Select(x => Card.ParseFromDefaultUnicodeRepresentation(x)).ToList();

            // Assert
            Assert.IsTrue(referenceDeck.SequenceEqual(result));
        }
    }
}
