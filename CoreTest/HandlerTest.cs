using Core;
using Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CoreTest
{
    [TestClass]
    public class HandlerTest
    {

        [TestMethod]
        public void GameHandlerShouldCreateGameAndMakeMove()
        {
            // Arrange
            List<uint> games = new List<uint>
            {
                123,
                456,
                789,
            };
            var memoryJourney = new MemoryJourney(Stage.First32000, games);
            var journeyMemoryRepository = new JourneyMemoryRepository(memoryJourney);

            var handler = new Handler(journeyMemoryRepository);
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
            List<uint> games = new List<uint>
            {
                123,
                456,
                789,
            };
            var memoryJourney = new MemoryJourney(Stage.First32000, games);
            var journeyMemoryRepository = new JourneyMemoryRepository(memoryJourney);

            var handler = new Handler(journeyMemoryRepository);
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

        [TestMethod]
        public void GameHandlerShouldRunEntireGame100AndWin()
        {
            // Arrange
            // Arrange
            List<uint> games = new List<uint>
            {
                123,
                456,
                789,
            };
            var memoryJourney = new MemoryJourney(Stage.First32000, games);
            var journeyMemoryRepository = new JourneyMemoryRepository(memoryJourney);
            var handler = new Handler(journeyMemoryRepository);

            // Act
            handler.ExecuteCommand(Handler.Command.NewGame(100));
            PlayGame100(handler);

            // Assert
            Assert.IsTrue(handler.Game.IsWon);
        }

        [TestMethod]
        public void GameHandlerShouldRunEntireGame32100AndWin()
        {
            // Arrange
            // Arrange
            List<uint> games = new List<uint>
            {
                123,
                456,
                789,
            };
            var memoryJourney = new MemoryJourney(Stage.First32000, games);
            var journeyMemoryRepository = new JourneyMemoryRepository(memoryJourney);
            var handler = new Handler(journeyMemoryRepository);

            // Act
            handler.ExecuteCommand(Handler.Command.NewGame(32100));
            PlayGame32100(handler);

            // Assert
            Assert.IsTrue(handler.Game.IsWon);
        }

        [TestMethod]
        public void GameHandlerShouldTakeGameFromJourney()
        {
            // Arrange
            List<uint> games = new List<uint>
            {
                123,
                456,
                789,
            };
            var memoryJourney = new MemoryJourney(Stage.First32000, games);
            var journeyMemoryRepository = new JourneyMemoryRepository(memoryJourney);

            var handler = new Handler(journeyMemoryRepository);
            handler.ExecuteCommand(Handler.Command.JourneyGame());

            // Act
            var id = handler.Game.Id;
            var wasAccessed = memoryJourney.WasAccessed;

            // Assert
            Assert.IsTrue(games.Contains(id));
            Assert.IsTrue(wasAccessed);
        }

        [TestMethod]
        public void GameHandlerShouldStartNewJourneyIfNoneIsProvided()
        {
            // Arrange
            var journeyMemoryRepository = new JourneyMemoryRepository(journey: null);

            // Act
            var handler = new Handler(journeyMemoryRepository);

            // Assert
            Assert.IsTrue(journeyMemoryRepository.HasCalledRead);
            Assert.IsTrue(journeyMemoryRepository.HasCalledWrite);
            Assert.AreEqual(Stage.First32000, journeyMemoryRepository.Contained.Stage);
            Assert.AreEqual(31999, journeyMemoryRepository.Contained.Games.Count);
            Assert.AreEqual(Stage.First32000, handler.Stage);
            Assert.AreEqual(31999u, handler.OpenGames);
        }

        [TestMethod]
        public void GameHandlerShouldTransitionFromFirstToSecondStage()
        {
            // Arrange
            List<uint> games = new List<uint>
            {
                100,
            };
            var memoryJourney = new MemoryJourney(Stage.First32000, games);
            var journeyMemoryRepository = new JourneyMemoryRepository(memoryJourney);

            // Act
            var handler = new Handler(journeyMemoryRepository);
            handler.ExecuteCommand(Handler.Command.NewGame(100));
            PlayGame100(handler);

            // Assert
            Assert.IsTrue(journeyMemoryRepository.HasCalledRead);
            Assert.IsTrue(journeyMemoryRepository.HasCalledWrite);
            Assert.AreEqual(Stage.Second32000, journeyMemoryRepository.Contained.Stage);
            Assert.AreEqual(32000, journeyMemoryRepository.Contained.Games.Count);
            Assert.AreEqual(Stage.Second32000, handler.Stage);
            Assert.AreEqual(32000u, handler.OpenGames);
        }

        [TestMethod]
        public void GameHandlerShouldTransitionFromSecondStageToFinished()
        {
            // Arrange
            List<uint> games = new List<uint>
            {
                32100,
            };
            var memoryJourney = new MemoryJourney(Stage.Second32000, games);
            var journeyMemoryRepository = new JourneyMemoryRepository(memoryJourney);

            // Act
            var handler = new Handler(journeyMemoryRepository);
            handler.ExecuteCommand(Handler.Command.NewGame(32100));
            PlayGame32100(handler);

            // Assert
            Assert.IsTrue(journeyMemoryRepository.HasCalledRead);
            Assert.IsTrue(journeyMemoryRepository.HasCalledWrite);
            Assert.AreEqual(Stage.Finished, journeyMemoryRepository.Contained.Stage);
            Assert.AreEqual(0, journeyMemoryRepository.Contained.Games.Count);
            Assert.AreEqual(Stage.Finished, handler.Stage);
            Assert.AreEqual(0u, handler.OpenGames);
        }

        [TestMethod]
        public void GameHandlerShouldCorrectlyIndicateCanUndo()
        {
            // Arrange
            List<uint> games = new List<uint>
            {
                32100,
            };
            var memoryJourney = new MemoryJourney(Stage.Second32000, games);
            var journeyMemoryRepository = new JourneyMemoryRepository(memoryJourney);

            // Act
            var handler = new Handler(journeyMemoryRepository);
            var result1 = handler.CanUndo;
            handler.ExecuteCommand(Handler.Command.NewGame(32100));
            var result2 = handler.CanUndo;
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column5));
            var result3 = handler.CanUndo;
            handler.ExecuteCommand(Handler.Command.Undo());
            var result4 = handler.CanUndo;
            PlayGame32100(handler);
            var isWon = handler.Game.IsWon;
            var result5 = handler.CanUndo;

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);
            Assert.IsTrue(isWon);
            Assert.IsFalse(result5);
        }

        /// <summary>
        /// Helper method that will execute the moves for game #100.
        /// </summary>
        /// <param name="handler">The handler to pass the moves to.</param>
        private static void PlayGame100(Handler handler)
        {
            handler.ExecuteCommand(Handler.Command.Move(Location.Column6, Location.Column1));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column6, Location.Cell0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column6, Location.Column4));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Foundation));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column4));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Cell1));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column6, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column6, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column7, Location.Cell2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column7, Location.Cell3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Column2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Foundation));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column3, Location.Column5));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column3, Location.Cell2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column3, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Cell2, Location.Column3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Cell3, Location.Column3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Column4));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Cell2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Column3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Cell3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Column2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Cell0));
        }

        /// <summary>
        /// Helper method that will execute the moves for game #32100.
        /// </summary>
        /// <param name="handler">The handler to pass the moves to.</param>
        private static void PlayGame32100(Handler handler)
        {
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column5));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column4));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Cell0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Column5));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Cell0, Location.Column2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column3, Location.Column2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column3, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column3, Location.Cell0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column3, Location.Cell1));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column7, Location.Cell2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column6, Location.Column3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column7, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column7, Location.Column4));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column7, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column7, Location.Column4));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Foundation));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Column5));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Cell3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Cell0, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column1, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column3, Location.Column7));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column6, Location.Column3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column0, Location.Column5));
            handler.ExecuteCommand(Handler.Command.Move(Location.Cell1, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Column0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Column1));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Column3));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column4, Location.Cell0));
            handler.ExecuteCommand(Handler.Command.Move(Location.Cell2, Location.Column4));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Column4));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Cell1));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Column6));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column5, Location.Cell2));
            handler.ExecuteCommand(Handler.Command.Move(Location.Column2, Location.Cell0));
        }
        
        /// <summary>
        /// In-memory implementation for testing and side-channel checking.
        /// </summary>
        private class JourneyMemoryRepository : IJourneyRepository
        {
            private IJourney journey;

            public JourneyMemoryRepository(MemoryJourney journey)
            {
                this.journey = journey;
            }

            internal IJourney Contained => this.journey;

            internal bool HasCalledRead { get; private set; }

            internal bool HasCalledWrite { get; private set; }

            public IJourney Read()
            {
                this.HasCalledRead = true;
                return this.journey;
            }

            public void Write(IJourney journey)
            {
                this.HasCalledWrite = true;
                this.journey = journey;
            }
        }

        /// <summary>
        /// In-memory implementation for testing and side-channel checking.
        /// </summary>
        private class MemoryJourney : IJourney
        {
            private readonly List<uint> games;

            public MemoryJourney(Stage stage, List<uint> games)
            {
                this.Stage = stage;
                this.games = games;
                this.WasAccessed = false;
            }

            public Stage Stage { get; }

            public List<uint> Games
            {
                get
                {
                    this.WasAccessed = true;
                    return this.games;
                }
            }

            public bool WasAccessed { get; private set; }
        }
    }
}
