using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTest
{
    [TestClass]
    public class HandlerTest
    {

        [TestMethod]
        public void GameHandlerShouldCreateGameAndMakeMove()
        {
            // Arrange
            var handler = new Handler();
            handler.ExecuteCommand(Handler.Command.NewGame(30828));

            var reference1 = @" ..  ..  ..  .. || ..  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦  7♠  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥  5♥  6♦  2♠  3♦
  3♣  8♣  3♥  6♥  6♠  5♦  A♠  2♦
  4♥  5♣  9♣  4♣  A♣  Q♥  6♣  9♥
  8♦  A♦  T♦  K♣  7♥  A♥  8♠  T♠
  Q♣  2♥  T♣  J♥  K♦  Q♦  9♠  2♣
  7♣  J♠  4♠  7♦                ";

            var reference2 = @" ..  4♠  ..  .. || ..  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦  7♠  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥  5♥  6♦  2♠  3♦
  3♣  8♣  3♥  6♥  6♠  5♦  A♠  2♦
  4♥  5♣  9♣  4♣  A♣  Q♥  6♣  9♥
  8♦  A♦  T♦  K♣  7♥  A♥  8♠  T♠
  Q♣  2♥  T♣  J♥  K♦  Q♦  9♠  2♣
  7♣          7♦      J♠        ";

            // Act
            var state1 = handler.UnicodeGameRepresentation;
            var refresh1 = handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Column5));
            var refresh2 = handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Cell1));
            var refresh3 = handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Cell1));
            var state2 = handler.UnicodeGameRepresentation;

            // Assert
            Assert.AreEqual(reference1, state1);
            Assert.IsTrue(refresh1);
            Assert.IsTrue(refresh2);
            Assert.IsFalse(refresh3);
            Assert.AreEqual(reference2, state2);
        }

        [TestMethod]
        public void GameHandlerShouldCreateGameMakeMoveAndUndo()
        {
            // Arrange
            var handler = new Handler();
            handler.ExecuteCommand(Handler.Command.NewGame(30828));

            var reference1 = @" ..  ..  ..  .. || ..  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦  7♠  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥  5♥  6♦  2♠  3♦
  3♣  8♣  3♥  6♥  6♠  5♦  A♠  2♦
  4♥  5♣  9♣  4♣  A♣  Q♥  6♣  9♥
  8♦  A♦  T♦  K♣  7♥  A♥  8♠  T♠
  Q♣  2♥  T♣  J♥  K♦  Q♦  9♠  2♣
  7♣  J♠  4♠  7♦                ";

            var reference2 = @" ..  4♠  ..  .. || ..  ..  ..  ..
 --------------------------------
  4♦  T♥  J♣  9♦  7♠  3♠  J♦  5♠
  Q♠  K♠  8♥  K♥  5♥  6♦  2♠  3♦
  3♣  8♣  3♥  6♥  6♠  5♦  A♠  2♦
  4♥  5♣  9♣  4♣  A♣  Q♥  6♣  9♥
  8♦  A♦  T♦  K♣  7♥  A♥  8♠  T♠
  Q♣  2♥  T♣  J♥  K♦  Q♦  9♠  2♣
  7♣          7♦      J♠        ";

            // Act
            var state1 = handler.UnicodeGameRepresentation;
            var refresh1 = handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Column5));
            var refresh2 = handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Cell1));
            var state2 = handler.UnicodeGameRepresentation;
            var refresh3 = handler.ExecuteCommand(Handler.Command.Undo());
            var refresh4 = handler.ExecuteCommand(Handler.Command.Undo());
            var refresh5 = handler.ExecuteCommand(Handler.Command.Undo());
            var state3 = handler.UnicodeGameRepresentation;

            // Assert
            Assert.AreEqual(reference1, state1);
            Assert.IsTrue(refresh1);
            Assert.IsTrue(refresh2);
            Assert.AreEqual(reference2, state2);
            Assert.IsTrue(refresh3);
            Assert.IsTrue(refresh4);
            Assert.IsFalse(refresh5);
            Assert.AreEqual(reference1, state3);
        }
    }
}
