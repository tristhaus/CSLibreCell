﻿using Core;
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
        public void GameCreationShouldGiveCorrectGame30828()
        {
            // Arrange
            var game = new Game(30828);

            var reference = @" ..  ..  ..  .. || ..  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦  7♠  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥  5♥  6♦  2♠  3♦
  3♣  8♣  3♥  6♥  6♠  5♦  A♠  2♦
  4♥  5♣  9♣  4♣  A♣  Q♥  6♣  9♥
  8♦  A♦  T♦  K♣  7♥  A♥  8♠  T♠
  Q♣  2♥  T♣  J♥  K♦  Q♦  9♠  2♣
  7♣  J♠  4♠  7♦                ";

            // Act
            var unicodeRepresentation = game.UnicodeRepresentation;

            // Assert
            Assert.AreEqual(reference, unicodeRepresentation);
        }

        [TestMethod]
        public void GameShouldCorrectlyRoundtripUnicode()
        {
            // Arrange
            var reference = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦  7♠  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥  5♥  6♦  2♠  3♦
  3♣  8♣  3♥  6♥  6♠  5♦  A♠  2♦
  4♥  5♣  9♣  4♣  7♥  Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(reference);
            var result = game.UnicodeRepresentation;

            // Assert
            Assert.AreEqual(reference, result);
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

        [TestMethod]
        public void MoveWithIdenticalSourceAndDestinationShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Foundation, Location.Foundation);
            var result2 = game.IsMoveLegal(Location.Cell2, Location.Cell2);
            var result3 = game.IsMoveLegal(Location.Column2, Location.Column2);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void MoveFromFoundationShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Foundation, Location.Column4);
            var result2 = game.IsMoveLegal(Location.Foundation, Location.Cell0);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void MoveFromEmptyCellShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Cell0, Location.Column4);
            var result2 = game.IsMoveLegal(Location.Cell0, Location.Cell1);
            var result3 = game.IsMoveLegal(Location.Cell0, Location.Foundation);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void MoveFromFilledCellToEmptyColumnShouldBeLegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.IsMoveLegal(Location.Cell2, Location.Column4);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MoveFromEmptyColumnShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Column4, Location.Cell0);
            var result2 = game.IsMoveLegal(Location.Column4, Location.Column3);
            var result3 = game.IsMoveLegal(Location.Column4, Location.Foundation);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void MoveFromFilledColumnToEmptyCellShouldBeLegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.IsMoveLegal(Location.Column3, Location.Cell0);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MoveFromFilledColumnToFilledCellShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.IsMoveLegal(Location.Column3, Location.Cell2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IncorrectMoveToFoundationShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      A♥  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  9♠    
  7♣  J♠  4♠  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Column3, Location.Foundation);
            var result2 = game.IsMoveLegal(Location.Cell2, Location.Foundation);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void CorrectMoveToFoundationShouldBeLegal()
        {
            // Arrange
            var state = @" A♥  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦        
  7♣  J♠  3♣  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Column2, Location.Foundation);
            var result2 = game.IsMoveLegal(Location.Cell0, Location.Foundation);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void CorrectSimpleMoveToFilledColumnShouldBeLegal()
        {
            // Arrange
            var state = @" ..  6♠  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♠  K♣      9♠  8♠  T♦
  Q♣  2♥  T♣  J♥      Q♦  A♥    
  7♣  J♠  3♣  7♦                
  7♠                            
  5♥                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Column7, Location.Column1);
            var result2 = game.IsMoveLegal(Location.Cell1, Location.Column3);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void ÎncorrectSimpleMoveToFilledColumnShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  6♠  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  7♦      9♠  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  A♥    
  7♣  J♠  3♣  K♣                
  7♠                            
  5♥                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Column6, Location.Column1);
            var result2 = game.IsMoveLegal(Location.Cell2, Location.Column3);
            var result3 = game.IsMoveLegal(Location.Column7, Location.Column1);
            var result4 = game.IsMoveLegal(Location.Column3, Location.Column5);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        [TestMethod]
        public void SimpleMoveToEmptyColumnShouldBeLegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  A♥    
  7♣  J♠  3♣  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.IsMoveLegal(Location.Column2, Location.Column4);
            var result2 = game.IsMoveLegal(Location.Cell2, Location.Column4);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void WonGameShouldBeWon()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || K♣  K♠  K♥  K♦
---------------------------------";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.IsWon;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnfinishedGameShouldNotBeWon()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
---------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  T♣  J♥      Q♦  A♥    
  7♣  J♠  3♣  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.IsWon;

            // Assert
            Assert.IsFalse(result);
        }
    }
}
