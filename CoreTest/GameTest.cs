using Core;
using Core.Internal;
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
 --------------------------------
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
 --------------------------------
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
 --------------------------------
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
 --------------------------------
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
 --------------------------------
  3S  AC  8C  AD  2H  AS  4C  6S
  QD  3C  9S  TS  JS  8D  9D  2D
  JC  9H  JD  5D  TC  2S  7C  QC
  4H  2C  7H  7S  6H  6C  KH  4D
  TD  QH  KD  KC  3D  KS  5S  6D
  JH  4S  9C      8S  7D  5C  3H
  QS  8H  5H                    ";

            // Act
            game.MakeMove(Location.Column3, Location.Cell1, moveSize: 1);
            game.MakeMove(Location.Column3, Location.Foundation, moveSize: 1);

            var asciiRepresentation = game.AsciiRepresentation;

            // Assert
            Assert.AreEqual(reference, asciiRepresentation);
        }

        [TestMethod]
        public void GameSuperMoveShouldWorkCorrectly()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  T♠
  8♦  A♦  T♦  K♣      A♥  8♠    
  Q♣  2♥  7♦  J♥      Q♦  9♠    
  7♣  J♠  4♠  T♣                
  7♠          9♥                
  5♥                            
  6♠                            
  7♥                            ";

            var reference = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  3♣  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  T♠
  8♦  A♦  T♦  K♣      A♥  8♠    
  Q♣  2♥  7♦          Q♦  9♠    
  7♣  J♠  4♠          J♥        
  7♠                  T♣        
  5♥                  9♥        
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            game.MakeMove(Location.Column3, Location.Column5, 3);
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void GameCopyCtorShouldYieldIndependentCopy()
        {
            // Arrange
            var game = new Game(20761);

            var copyReference = @" ..  ..  ..  .. || ..  ..  ..  ..
 --------------------------------
  3S  AC  8C  AD  2H  AS  4C  6S
  QD  3C  9S  TS  JS  8D  9D  2D
  JC  9H  JD  5D  TC  2S  7C  QC
  4H  2C  7H  7S  6H  6C  KH  4D
  TD  QH  KD  KC  3D  KS  5S  6D
  JH  4S  9C  AH  8S  7D  5C  3H
  QS  8H  5H  TH                ";

            // Act
            var copy = new Game(game);
            
            game.MakeMove(Location.Column3, Location.Cell1, moveSize: 1);
            game.MakeMove(Location.Column3, Location.Foundation, moveSize: 1);

            var copyAsciiRepresentation = copy.AsciiRepresentation;

            // Assert
            Assert.AreEqual(copyReference, copyAsciiRepresentation);
        }

        [TestMethod]
        public void GameCopyCtorShouldWorkCorrectly()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 3♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♥  8♣  3♥  6♥      5♦  A♠  2♦
  8♦  5♣  9♣  4♣      Q♥  6♣  9♥
  Q♣  A♦  T♦  K♣      A♥  8♠  T♠
  7♣  2♥  T♣  J♥      Q♦  9♠    
  7♠  J♠  4♠  7♦                
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);

            var copy = new Game(game);

            var copyUnicodeRepresentation = copy.UnicodeRepresentation;

            // Assert
            Assert.AreEqual(state, copyUnicodeRepresentation);
        }

        [TestMethod]
        public void MoveWithIdenticalSourceAndDestinationShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Foundation, Location.Foundation);
            var result2 = game.GetLegalMoveSize(Location.Cell2, Location.Cell2);
            var result3 = game.GetLegalMoveSize(Location.Column2, Location.Column2);

            // Assert
            Assert.AreEqual(0u, result1);
            Assert.AreEqual(0u, result2);
            Assert.AreEqual(0u, result3);
        }

        [TestMethod]
        public void MoveFromFoundationShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Foundation, Location.Column4);
            var result2 = game.GetLegalMoveSize(Location.Foundation, Location.Cell0);

            // Assert
            Assert.AreEqual(0u, result1);
            Assert.AreEqual(0u, result2);
        }

        [TestMethod]
        public void MoveFromEmptyCellShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Cell0, Location.Column4);
            var result2 = game.GetLegalMoveSize(Location.Cell0, Location.Cell1);
            var result3 = game.GetLegalMoveSize(Location.Cell0, Location.Foundation);

            // Assert
            Assert.AreEqual(0u, result1);
            Assert.AreEqual(0u, result2);
            Assert.AreEqual(0u, result3);
        }

        [TestMethod]
        public void MoveFromFilledCellToEmptyColumnShouldBeLegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result = game.GetLegalMoveSize(Location.Cell2, Location.Column4);

            // Assert
            Assert.AreEqual(1u, result);
        }

        [TestMethod]
        public void MoveFromEmptyColumnShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Column4, Location.Cell0);
            var result2 = game.GetLegalMoveSize(Location.Column4, Location.Column3);
            var result3 = game.GetLegalMoveSize(Location.Column4, Location.Foundation);

            // Assert
            Assert.AreEqual(0u, result1);
            Assert.AreEqual(0u, result2);
            Assert.AreEqual(0u, result3);
        }

        [TestMethod]
        public void MoveFromFilledColumnToEmptyCellShouldBeLegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result = game.GetLegalMoveSize(Location.Column3, Location.Cell0);

            // Assert
            Assert.AreEqual(1u, result);
        }

        [TestMethod]
        public void MoveFromFilledColumnToFilledCellShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result = game.GetLegalMoveSize(Location.Column3, Location.Cell2);

            // Assert
            Assert.AreEqual(0u, result);
        }

        [TestMethod]
        public void IncorrectMoveToFoundationShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Column3, Location.Foundation);
            var result2 = game.GetLegalMoveSize(Location.Cell2, Location.Foundation);

            // Assert
            Assert.AreEqual(0u, result1);
            Assert.AreEqual(0u, result2);
        }

        [TestMethod]
        public void CorrectMoveToFoundationShouldBeLegal()
        {
            // Arrange
            var state = @" A♥  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Column2, Location.Foundation);
            var result2 = game.GetLegalMoveSize(Location.Cell0, Location.Foundation);

            // Assert
            Assert.AreEqual(1u, result1);
            Assert.AreEqual(1u, result2);
        }

        [TestMethod]
        public void CorrectSimpleMoveToFilledColumnShouldBeLegal()
        {
            // Arrange
            var state = @" ..  6♠  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Column7, Location.Column1);
            var result2 = game.GetLegalMoveSize(Location.Cell1, Location.Column3);

            // Assert
            Assert.AreEqual(1u, result1);
            Assert.AreEqual(1u, result2);
        }

        [TestMethod]
        public void ÎncorrectSimpleMoveToFilledColumnShouldBeIllegal()
        {
            // Arrange
            var state = @" ..  6♠  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Column6, Location.Column1);
            var result2 = game.GetLegalMoveSize(Location.Cell2, Location.Column3);
            var result3 = game.GetLegalMoveSize(Location.Column7, Location.Column1);
            var result4 = game.GetLegalMoveSize(Location.Column3, Location.Column5);

            // Assert
            Assert.AreEqual(0u, result1);
            Assert.AreEqual(0u, result2);
            Assert.AreEqual(0u, result3);
            Assert.AreEqual(0u, result4);
        }

        [TestMethod]
        public void SimpleMoveToEmptyColumnShouldBeLegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
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
            var result1 = game.GetLegalMoveSize(Location.Column2, Location.Column4);
            var result2 = game.GetLegalMoveSize(Location.Cell2, Location.Column4);

            // Assert
            Assert.AreEqual(1u, result1);
            Assert.AreEqual(1u, result2);
        }

        [TestMethod]
        public void SuperMoveToEmptyColumnShouldBeLegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  T♠
  8♦  A♦  T♦  K♣      9♠  8♠  9♥
  Q♣  2♥  T♣  J♥      Q♦  A♥    
  7♣  J♠  3♣  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.GetLegalMoveSize(Location.Column7, Location.Column4);

            // Assert
            Assert.AreEqual(2u, result1);
        }

        [TestMethod]
        public void SuperMoveToMatchingColumnShouldBeLegal()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  T♠
  8♦  A♦  T♦  K♣      9♠  8♠  9♥
  Q♣  2♥  T♣  J♥      Q♦  A♥    
  7♣  J♠  3♣              7♦    
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.GetLegalMoveSize(Location.Column7, Location.Column4);

            // Assert
            Assert.AreEqual(2u, result1);
        }

        [TestMethod]
        public void SuperMoveShouldBeTruncatedCorrectlyToMatchColumn()
        {
            // Arrange
            var state = @" ..  ..  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      J♥  6♣  T♠
  8♦  A♦  T♣  K♣      9♠  8♠  9♥
  Q♣  2♥  3♣  Q♥      Q♦  A♥    
  7♣                  J♠  7♦    
  7♠                  T♦        
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.GetLegalMoveSize(Location.Column5, Location.Column3);

            // Assert
            Assert.AreEqual(2u, result1);
        }

        [TestMethod]
        public void SuperMoveShouldBePossibleWithJustAColumn()
        {
            // Arrange
            var state = @" 9♥  T♠  K♦  2♦ || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠    
  4♥  5♣  9♣  4♣      J♥  6♣    
  8♦  A♦  T♣  K♣      9♠  8♠    
  Q♣  2♥  3♣  Q♥      Q♦  A♥    
  7♣                  J♠  7♦    
  7♠                  T♦        
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.GetLegalMoveSize(Location.Column5, Location.Column3);

            // Assert
            Assert.AreEqual(2u, result1);
        }

        [TestMethod]
        public void SuperMoveShouldBeDeniedIfNoCapacity()
        {
            // Arrange
            var state = @" 9♥  T♠  K♦  2♦ || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦  3♦  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠    
  4♠  8♣  3♥  6♥      5♦  A♠    
  4♥  5♣  9♣  4♣      J♥  6♣    
  8♦  A♦  T♣  K♣      9♠  8♠    
  Q♣  2♥  3♣  Q♥      Q♦  A♥    
  7♣                  J♠  7♦    
  7♠                  T♦        
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.GetLegalMoveSize(Location.Column5, Location.Column3);

            // Assert
            Assert.AreEqual(0u, result1);
        }

        [TestMethod]
        public void SuperMoveShouldBeDeniedIfTooLittleCapacity()
        {
            // Arrange
            var state = @" 9♥  T♠  K♦  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦  3♦  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  2♦
  4♠  8♣  3♥  6♥      5♦  A♠    
  4♥  5♣  T♣  4♣      J♥  6♣    
  8♦  A♦  3♣  K♣      9♠  8♠    
  Q♣  2♥      Q♥      Q♦  A♥    
  7♣                  J♠  7♦    
  7♠                  T♦        
  5♥                  9♣        
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.GetLegalMoveSize(Location.Column5, Location.Column3);

            // Assert
            Assert.AreEqual(0u, result1);
        }

        [TestMethod]
        public void SuperMoveShouldBePossibleWithJustCells()
        {
            // Arrange
            var state = @" 9♥  T♠  ..  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦  3♦  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  2♦
  4♠  8♣  3♥  6♥      5♦  A♠  K♦
  4♥  5♣  T♣  4♣      J♥  6♣    
  8♦  A♦  3♣  K♣      9♠  8♠    
  Q♣  2♥      Q♥      Q♦  A♥    
  7♣                  J♠  7♦    
  7♠                  T♦        
  5♥                  9♣        
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.GetLegalMoveSize(Location.Column5, Location.Column3);

            // Assert
            Assert.AreEqual(3u, result1);
        }

        [TestMethod]
        public void SuperMoveShouldHandleComplicatedCase()
        {
            // Arrange
            var state = @" 9♥  T♠  ..  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  3♥  K♥      6♦  2♠  2♦
  4♠  8♣  T♣  4♣      5♦  A♠  K♦
  4♥  5♣  3♣  K♣      J♥  6♣  3♦
  8♦  A♦      Q♥      9♠  8♠    
  Q♣  2♥              Q♦  A♥    
  7♣                  J♠  7♦    
  5♥                  T♦        
  6♠                  9♣        
  7♥                  8♥        
                      7♠        
                      6♥        ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.GetLegalMoveSize(Location.Column5, Location.Column3);

            // Assert
            Assert.AreEqual(6u, result1);
        }

        [TestMethod]
        public void WonGameShouldBeWon()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || K♣  K♠  K♥  K♦
 --------------------------------";

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
 --------------------------------
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

        [TestMethod]
        public void AceOfHeartsShouldBeAutoMovedFromCell()
        {
            // Arrange
            var state = @" ..  A♥  ..  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  3♣  J♥      Q♦  K♦    
  7♣  J♠  T♣  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            var reference = @" ..  ..  ..  .. || 2♣  ..  A♥  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  3♣  J♥      Q♦  K♦    
  7♣  J♠  T♣  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void AceOfHeartsShouldBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  3♣  J♥      Q♦  K♦    
  7♣  J♠  T♣  7♦      A♥        
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            var reference = @" ..  ..  ..  .. || 2♣  ..  A♥  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  3♣  J♥      Q♦  K♦    
  7♣  J♠  T♣  7♦                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void TwoOfClubsShouldBeAutoMovedFromCell()
        {
            // Arrange
            var state = @" ..  ..  ..  2♣ || A♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  3♣  J♥      A♥  K♦    
  7♣  J♠  T♣  7♦      Q♦        
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            var reference = @" ..  ..  ..  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  3♣  J♥      A♥  K♦    
  7♣  J♠  T♣  7♦      Q♦        
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void TwoOfClubsShouldBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || A♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  3♣  J♥      A♥  K♦    
  7♣  J♠  T♣  7♦      Q♦        
  7♠      2♣                    
  5♥                            
  6♠                            
  7♥                            ";

            var reference = @" ..  ..  ..  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  3♣  J♥      A♥  K♦    
  7♣  J♠  T♣  7♦      Q♦        
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void ThreeOfClubsShouldNotBeAutoMovedFromCell()
        {
            // Arrange
            var state = @" ..  ..  3♣  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  T♣  J♥      A♥  K♦    
  7♣  J♠      7♦      Q♦        
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(state, representation);
        }

        [TestMethod]
        public void ThreeOfClubsShouldNotBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || 2♣  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  A♠  2♦
  4♥  5♣  9♣  4♣      Q♥  6♣  9♥
  8♦  A♦  T♦  K♣      9♠  8♠  T♠
  Q♣  2♥  T♣  J♥      A♥  K♦    
  7♣  J♠      7♦      Q♦        
  7♠  3♣                        
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(state, representation);
        }

        [TestMethod]
        public void FourOfDiamondsShouldNotBeAutoMovedFromCell()
        {
            // Arrange
            var state = @" ..  4♦  ..  .. || ..  2♠  ..  3♦
 --------------------------------
  4♠  T♣  J♦  9♠      3♥  J♠  5♥
  Q♥  K♥  8♣  K♣      6♠  2♥  3♠
  4♥  8♦  3♣  6♣      5♠  A♥  9♣
  4♣  5♦  9♦  K♦      Q♣  6♦  T♥
  8♠  A♠  T♠  J♣      9♥  8♥    
  Q♦  2♣  T♦  7♠      A♣  K♠    
  7♦  J♥              Q♠        
  7♥                            
  5♣                            
  6♥                            
  7♣                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(state, representation);
        }

        [TestMethod]
        public void FourOfDiamondsShouldNotBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || ..  2♠  ..  3♦
 --------------------------------
  4♠  T♣  J♦  9♠      3♥  J♠  5♥
  Q♥  K♥  8♣  K♣      6♠  2♥  3♠
  4♥  8♦  3♣  6♣      5♠  A♥  9♣
  4♣  5♦  9♦  K♦      Q♣  6♦  T♥
  8♠  A♠  T♠  J♣      9♥  8♥    
  Q♦  2♣  T♦  7♠      A♣  K♠    
  7♦  J♥              Q♠        
  7♥                  4♦        
  5♣                            
  6♥                            
  7♣                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(state, representation);
        }

        [TestMethod]
        public void FourOfClubsShouldBeAutoMovedFromCell()
        {
            // Arrange
            var state = @" ..  4♣  ..  .. || 3♣  A♠  2♥  2♦
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  6♣  9♥
  4♥  5♣  9♣  K♣      Q♥  8♠  T♠
  8♦  A♦  T♦  J♥      9♠  K♦    
  Q♣  J♠  T♣  7♦      Q♦        
  7♣                            
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            var reference = @" ..  ..  ..  .. || 4♣  A♠  2♥  2♦
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  6♣  9♥
  4♥  5♣  9♣  K♣      Q♥  8♠  T♠
  8♦  A♦  T♦  J♥      9♠  K♦    
  Q♣  J♠  T♣  7♦      Q♦        
  7♣                            
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void FourOfClubsShouldBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || 3♣  A♠  2♥  2♦
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  6♣  9♥
  4♥  5♣  9♣  K♣      Q♥  8♠  T♠
  8♦  A♦  T♦  J♥      9♠  K♦    
  Q♣  J♠  T♣  7♦      Q♦        
  7♣          4♣                
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            var reference = @" ..  ..  ..  .. || 4♣  A♠  2♥  2♦
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  6♣  9♥
  4♥  5♣  9♣  K♣      Q♥  8♠  T♠
  8♦  A♦  T♦  J♥      9♠  K♦    
  Q♣  J♠  T♣  7♦      Q♦        
  7♣                            
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void FourOfClubsShouldNotBeAutoMovedFromCell()
        {
            // Arrange
            var state = @" ..  4♣  ..  .. || 3♣  ..  2♥  2♦
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  6♣  9♥
  4♥  5♣  9♣  K♣      Q♥  8♠  A♠
  8♦  A♦  T♦  J♥      9♠  K♦  T♠
  Q♣  J♠  T♣  7♦      Q♦        
  7♣                            
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(state, representation);
        }

        [TestMethod]
        public void FourOfClubsShouldNotBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || 3♣  ..  2♥  2♦
 --------------------------------
  4♦  T♥  J♣  9♦      3♠  J♦  5♠
  Q♠  K♠  8♥  K♥      6♦  2♠  3♦
  4♠  8♣  3♥  6♥      5♦  6♣  9♥
  4♥  5♣  9♣  K♣      Q♥  8♠  A♠
  8♦  A♦  T♦  J♥      9♠  K♦  T♠
  Q♣  J♠  T♣  7♦      Q♦        
  7♣  4♣                        
  7♠                            
  5♥                            
  6♠                            
  7♥                            ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(state, representation);
        }

        [TestMethod]
        public void TenOfClubsShouldNotBeAutoMovedFromCell()
        {
            // Arrange
            var state = @" ..  T♣  ..  .. || 9♣  6♠  8♥  8♦
 --------------------------------
  T♥  J♣  9♠  J♦  K♦            
  Q♠  7♠  K♠  K♥                
  9♥  T♠  8♠                    
  9♣  K♣  Q♥                    
  8♦  T♦  J♥                    
  Q♣  J♠  Q♦                    ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(state, representation);
        }

        [TestMethod]
        public void TenOfClubsShouldNotBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || 9♣  6♠  8♥  8♦
 --------------------------------
  T♥  J♣  9♠  J♦  K♦            
  Q♠  7♠  K♠  K♥                
  9♥  T♠  8♠  T♣                
  9♣  K♣  Q♥                    
  8♦  T♦  J♥                    
  Q♣  J♠  Q♦                    ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(state, representation);
        }

        [TestMethod]
        public void TenOfClubsShouldBeAutoMovedFromCell()
        {
            // Arrange
            var state = @" ..  T♣  ..  .. || 9♣  7♠  8♥  8♦
 --------------------------------
  T♥  J♣  9♠  J♦  K♦            
  Q♠  K♠  K♥                    
  9♥  T♠  8♠                    
  9♣  K♣  Q♥                    
  8♦  T♦  J♥                    
  Q♣  J♠  Q♦                    ";

            var reference = @" ..  ..  ..  .. || T♣  7♠  8♥  8♦
 --------------------------------
  T♥  J♣  9♠  J♦  K♦            
  Q♠  K♠  K♥                    
  9♥  T♠  8♠                    
  9♣  K♣  Q♥                    
  8♦  T♦  J♥                    
  Q♣  J♠  Q♦                    ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void TenOfClubsShouldBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" ..  ..  ..  .. || 9♣  7♠  8♥  8♦
 --------------------------------
  T♥  J♣  9♠  J♦  K♦            
  Q♠  K♠  K♥  T♣                
  9♥  T♠  8♠                    
  9♣  K♣  Q♥                    
  8♦  T♦  J♥                    
  Q♣  J♠  Q♦                    ";

            var reference = @" ..  ..  ..  .. || T♣  7♠  8♥  8♦
 --------------------------------
  T♥  J♣  9♠  J♦  K♦            
  Q♠  K♠  K♥                    
  9♥  T♠  8♠                    
  9♣  K♣  Q♥                    
  8♦  T♦  J♥                    
  Q♣  J♠  Q♦                    ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void AutoMovesShouldLeadToWinning()
        {
            // Arrange
            var state = @" 4♥  4♦  ..  .. || 5♣  5♠  3♥  3♦ 
 -------------------------------
  K♣  8♠  K♠  K♥  K♦               
  Q♥  7♦  Q♦  Q♣  Q♠               
  J♠  6♠  J♣  J♥  J♦               
  T♥  5♦  T♦  T♣  T♠               
  9♠      9♣  9♦  9♥               
  8♥      8♦  8♣                   
  7♠      7♣  7♥                   
  6♦      6♥  6♣                   
              5♥                   ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();

            while (game.AutoMoveToFoundation()) ;

            var isWon = game.IsWon;

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(isWon);
        }

        [TestMethod]
        public void ThreeOfDiamondsShouldBeAutoMovedFromColumn()
        {
            // Arrange
            var state = @" 8♥  3♠  K♣  6♥ || A♣  A♠  A♥  2♦
 --------------------------------
  T♦  J♥  4♣  9♥  T♥  9♦  5♣  Q♥
  3♦  T♣  2♥  8♣  3♣  8♠  2♠  J♠
      J♣  Q♦  7♥  K♦  7♦  K♠    
      9♠  2♣  6♠  Q♣  6♣  5♠    
      7♠  4♦  5♦  J♦  5♥  9♣    
      7♣  4♥      T♠  4♠  8♦    
      K♥  6♦          3♥        
      Q♠                        ";

            var reference = @" 8♥  3♠  K♣  6♥ || A♣  A♠  A♥  3♦
 --------------------------------
  T♦  J♥  4♣  9♥  T♥  9♦  5♣  Q♥
      T♣  2♥  8♣  3♣  8♠  2♠  J♠
      J♣  Q♦  7♥  K♦  7♦  K♠    
      9♠  2♣  6♠  Q♣  6♣  5♠    
      7♠  4♦  5♦  J♦  5♥  9♣    
      7♣  4♥      T♠  4♠  8♦    
      K♥  6♦          3♥        
      Q♠                        ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(reference, representation);
        }

        [TestMethod]
        public void ThreeAndFourOfHeartsShouldBeAutoMoved()
        {
            // Arrange
            var state = @" 3♥  T♠  8♦  T♥ || 3♣  3♠  2♥  A♦
 --------------------------------
  J♦  8♣  9♠  7♦  4♦  8♥  K♥  4♣
  6♦  Q♥  7♠  6♠      7♣  Q♣  9♣
  5♠  2♦  J♠          6♥      K♣
  Q♠  5♥  J♥          5♣      K♠
  9♦  K♦  T♣          4♥      Q♦
          9♥                  J♣
          8♠                  T♦
          7♥                    
          6♣                    
          5♦                    
          4♠                    
          3♦                    ";

            var reference = @" ..  T♠  8♦  T♥ || 3♣  3♠  4♥  A♦
 --------------------------------
  J♦  8♣  9♠  7♦  4♦  8♥  K♥  4♣
  6♦  Q♥  7♠  6♠      7♣  Q♣  9♣
  5♠  2♦  J♠          6♥      K♣
  Q♠  5♥  J♥          5♣      K♠
  9♦  K♦  T♣                  Q♦
          9♥                  J♣
          8♠                  T♦
          7♥                    
          6♣                    
          5♦                    
          4♠                    
          3♦                    ";

            // Act
            var game = Game.ParseFromUnicodeRepresentation(state);
            var result1 = game.AutoMoveToFoundation();
            var result2 = game.AutoMoveToFoundation();
            var representation = game.UnicodeRepresentation;

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(reference, representation);
        }
    }
}
