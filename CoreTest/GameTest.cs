using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTest
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void GameCreationShouldGiveCorrectGame1()
        {
            // Arrange
            var game = new Game(1);

            var reference = @" ..  ..  ..  .. || ..  ..  ..  ..
---------------------------------
  JD  2D  9H  JC  5D  7H  7C  5H
  KD  KC  9S  5S  AD  QC  KH  3H
  2S  KS  9D  QD  JS  AS  AH  3C
  4C  5C  TS  QH  4H  AC  4D  7S
  3S  TD  4S  TH  8H  2C  JH  7D
  6D  8S  8D  QS  6C  3D  8C  TC
  6S  9C  2H  6H                ";

            // Act
            var asciiRepresentation = game.AsciiRepresentation;

            // Assert
            Assert.AreEqual(reference, asciiRepresentation);
        }

        [TestMethod]
        public void GameCreationShouldGiveCorrectGame617()
        {
            // Arrange
            var game = new Game(617);

            var reference = @" ..  ..  ..  .. || ..  ..  ..  ..
---------------------------------
  7D  AD  5C  3S  5S  8C  2D  AH
  TD  7S  QD  AC  6D  8H  AS  KH
  TH  QC  3H  9D  6S  8D  3D  TC
  KD  5H  9S  3C  8S  7H  4D  JS
  4C  QS  9C  9H  7C  6H  2C  2S
  4S  TS  2H  5D  JC  6C  JH  QH
  JD  KS  KC  4H                ";

            // Act
            var asciiRepresentation = game.AsciiRepresentation;

            // Assert
            Assert.AreEqual(reference, asciiRepresentation);
        }

        [TestMethod]
        public void GameSimpleMovesShouldWorkCorrectly()
        {
            // Arrange
            var game = new Game(20761);

            var reference = @" ..  TH  ..  .. || ..  ..  AH  ..
---------------------------------
  3S  AC  8C  AD  2H  AS  4C  6S
  QD  3C  9S  TS  JS  8D  9D  2D
  JC  9H  JD  5D  TC  2S  7C  QC
  4H  2C  7H  7S  6H  6C  KH  4D
  TD  QH  KD  KC  3D  KS  5S  6D
  JH  4S  9C      8S  7D  5C  3H
  QS  8H  5H                    ";

            // Act
            game.MakeMove(Location.Column3, Location.Cell1);
            game.MakeMove(Location.Column3, Location.Foundation);

            var asciiRepresentation = game.AsciiRepresentation;

            // Assert
            Assert.AreEqual(reference, asciiRepresentation);
        }

        [TestMethod]
        public void GameCopyCtorShouldWorkCorrectly()
        {
            // Arrange
            var game = new Game(20761);

            var copyReference = @" ..  ..  ..  .. || ..  ..  ..  ..
---------------------------------
  3S  AC  8C  AD  2H  AS  4C  6S
  QD  3C  9S  TS  JS  8D  9D  2D
  JC  9H  JD  5D  TC  2S  7C  QC
  4H  2C  7H  7S  6H  6C  KH  4D
  TD  QH  KD  KC  3D  KS  5S  6D
  JH  4S  9C  AH  8S  7D  5C  3H
  QS  8H  5H  TH                ";

            // Act
            var copy = new Game(game);
            
            game.MakeMove(Location.Column3, Location.Cell1);
            game.MakeMove(Location.Column3, Location.Foundation);

            var copyAsciiRepresentation = copy.AsciiRepresentation;

            // Assert
            Assert.AreEqual(copyReference, copyAsciiRepresentation);
        }
    }
}
