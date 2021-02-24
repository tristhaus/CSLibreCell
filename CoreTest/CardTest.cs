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

            // Assert
            Assert.AreEqual("4C", result);
        }

        [TestMethod]
        public void CardShouldParseAsciiRepresentationCorrectly()
        {
            // Arrange
            var asciiRepresentation = "3S"; // Three of Spades

            // Act
            var result = Card.ParseFromAsciiRepresentation(asciiRepresentation);

            // Assert
            Assert.AreEqual(new Card(11), result);
        }

        [TestMethod]
        public void CardShouldCorrectlyRoundTripEntireDeck()
        {
            // Arrange
            var referenceDeck = new List<Card>(52);
            for (uint i = 0; i < 52; i++)
            {
                referenceDeck.Add(new Card(i));
            }

            // Act
            var asciiRepresentationDeck = referenceDeck.Select(x => x.AsciiRepresentation).ToList();
            var result = asciiRepresentationDeck.Select(x => Card.ParseFromAsciiRepresentation(x)).ToList();

            // Assert
            Assert.IsTrue(referenceDeck.SequenceEqual(result));
        }

    }
}
