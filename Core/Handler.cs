using Core.Internal;
using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// A handler for the journey, <see cref="IGame"/>, and commands there-to.
    /// </summary>
    public class Handler
    {
        private static readonly Random Random = new Random();

        private readonly IJourneyRepository journeyRepository;
        private readonly Stack<Game> gameStates = new Stack<Game>();

        private IJourney journey = new Journey(Stage.NotStarted, new List<uint>(0));
        private Game game = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler"/> class using the proper journey repository.
        /// </summary>
        public Handler()
            : this(new JourneyRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler"/> class using the supplied journey repository.
        /// </summary>
        /// <param name="journeyRepository">The supplied journey repository.</param>
        internal Handler(IJourneyRepository journeyRepository)
        {
            this.journeyRepository = journeyRepository;

            this.journey = this.journeyRepository.Read();

            if (this.journey == null)
            {
                this.journey = this.CreateNewJourney();
                this.journeyRepository.Write(this.journey);
            }
        }

        /// <summary>
        /// Gets the contained game, if present.
        /// </summary>
        public IGame Game => this.game;

        /// <summary>
        /// Gets the stage of the journey.
        /// </summary>
        public Stage Stage => this.journey?.Stage ?? Stage.NotStarted;

        /// <summary>
        /// Gets the count of yet unsolved games, if present.
        /// </summary>
        public uint? OpenGames => (uint?)this.journey?.Games?.Count;

        /// <summary>
        /// Gets a value indicating whether there is a previous move that can be undone.
        /// </summary>
        public bool CanUndo => gameStates.Count > 0 && !this.game.IsWon;

        /// <summary>
        /// Gets the Unicode representation of the game, if one is present.
        /// </summary>
        internal string UnicodeGameRepresentation => this.game?.UnicodeRepresentation ?? string.Empty;

        /// <summary>
        /// Starts a new game defined by its ID.
        /// </summary>
        /// <param name="gameId">The ID of the game to start.</param>
        /// <returns>Value indicating whether the client should reload game information.</returns>
        public bool NewGame(uint gameId)
        {
            this.InitAndPrepareGame(gameId);
            return true;
        }

        /// <summary>
        /// Starts a new game from the journey, if any are available.
        /// </summary>
        /// <returns>Value indicating whether the client should reload game information.</returns>
        public bool JourneyGame()
        {
            if (this.journey.Games.Count != 0)
            {
                var gameId = this.journey.Games[Random.Next(this.journey.Games.Count)];

                this.InitAndPrepareGame(gameId);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to make a move in the current <see cref="Game"/> indicated by the locations.
        /// </summary>
        /// <param name="source">The source location of the move.</param>
        /// <param name="destination">The destination location of the move.</param>
        /// <returns>Value indicating whether the client should reload game information.</returns>
        public bool Move(Location source, Location destination)
        {
            if (this.game?.IsWon == false)
            {
                var moveSize = this.game.GetLegalMoveSize(source, destination);

                if (moveSize > 0)
                {
                    this.gameStates.Push(new Game(this.game));
                    this.game.MakeMove(source, destination, moveSize);

                    while (this.game.AutoMoveToFoundation()) ;

                    if (this.game.IsWon)
                    {
                        this.HandleJourney();
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to undo the last move in the current <see cref="Game"/>.
        /// </summary>
        /// <returns>Value indicating whether the client should reload game information.</returns>
        public bool Undo()
        {
            var oldGameStateIsAccessible = !this.game.IsWon && this.gameStates.Count > 0;
            if (oldGameStateIsAccessible)
            {
                this.game = this.gameStates.Pop();
            }

            return oldGameStateIsAccessible;
        }

        private void HandleJourney()
        {
            if (this.journey.Games.Contains(this.game.Id))
            {
                this.journey.Games.Remove(this.game.Id);

                if (this.journey.Games.Count == 0)
                {
                    switch (this.journey.Stage)
                    {
                        case Stage.First32000:
                            {
                                this.journey = new Journey(Stage.Second32000, Internal.Game.GetWinnableGames(32001, 64000));

                                break;
                            }

                        case Stage.Second32000:
                            {
                                this.journey = new Journey(Stage.Finished, new List<uint>(0));

                                break;
                            }

                        case Stage.NotStarted:
                        case Stage.Finished:
                        default:
                            throw new Exception($"impossible journey state {this.journey.Stage} in {nameof(HandleJourney)}");
                    }
                }

                this.journeyRepository.Write(this.journey);
            }
        }

        private IJourney CreateNewJourney()
        {
            return new Journey(Stage.First32000, Internal.Game.GetWinnableGames(1, 32000));
        }

        private void InitAndPrepareGame(uint gameId)
        {
            this.game = new Game(gameId);
            while (this.game.AutoMoveToFoundation()) ;

            this.gameStates.Clear();
        }
    }
}
