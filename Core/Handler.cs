using Core.Internal;
using System;
using System.Collections.Generic;

namespace Core
{
    public class Handler
    {
        private static readonly Random Random = new Random();

        private readonly IJourneyRepository journeyRepository;
        private readonly Stack<Game> gameStates = new Stack<Game>();

        private IJourney journey = new Journey(Stage.NotStarted, new List<uint>(0));
        private Game game = null;

        public Handler()
            : this(new JourneyRepository())
        {
        }

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

        public IGame Game => this.game;

        public Stage Stage => this.journey?.Stage ?? Stage.NotStarted;

        public uint? OpenGames => (uint?)this.journey?.Games?.Count;

        internal string UnicodeGameRepresentation => this.game?.UnicodeRepresentation ?? string.Empty;

        /// <summary>
        /// Attempts to execute the given command.
        /// </summary>
        /// <returns>Value indicating whether the client should reload <see cref="this.UnicodeGameRepresentation"/>.</returns>
        public bool ExecuteCommand(Command command)
        {
            switch (command.Operation)
            {
                case Operation.NewGame:
                    this.game = new Game((uint)command.GameId);
                    while (this.game.AutoMoveToFoundation()) ;

                    this.gameStates.Clear();
                    return true;

                case Operation.JourneyGame:
                    if (this.journey.Games.Count != 0)
                    {
                        var id = this.journey.Games[Random.Next(this.journey.Games.Count)];

                        this.game = new Game(id);
                        while (this.game.AutoMoveToFoundation()) ;

                        this.gameStates.Clear();
                        return true;

                    }

                    return false;

                case Operation.Move:
                    if (this.game?.IsWon == false)
                    {
                        var moveSize = this.game.GetLegalMoveSize((Location)command.Source, (Location)command.Destination);

                        if (moveSize > 0)
                        {
                            this.gameStates.Push(new Game(this.game));
                            this.game.MakeMove((Location)command.Source, (Location)command.Destination, moveSize);

                            while (this.game.AutoMoveToFoundation()) ;

                            if (this.game.IsWon)
                            {
                                this.HandleJourney();
                            }

                            return true;
                        }
                    }

                    return false;

                case Operation.Undo:
                    var oldGameStateIsAccessible = !this.game.IsWon && this.gameStates.Count > 0;
                    if (oldGameStateIsAccessible)
                    {
                        this.game = this.gameStates.Pop();
                    }

                    return oldGameStateIsAccessible;

                default:
                    throw new Exception($"enum member '{command.Operation}' missing in {nameof(ExecuteCommand)}");
            }
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

        internal enum Operation
        {
            NewGame,
            JourneyGame,
            Move,
            Undo,
        }

        public class Command
        {
            internal Operation Operation { get; private set; }

            internal uint? GameId { get; private set; }

            internal Location? Source { get; private set; }

            internal Location? Destination { get; private set; }

            public static Command NewGame(uint id)
            {
                if (id == 0)
                {
                    throw new ArgumentException("");
                }

                var command = new Command
                {
                    Operation = Operation.NewGame,
                    GameId = id,
                };

                return command;
            }

            public static Command JourneyGame()
            {
                var command = new Command
                {
                    Operation = Operation.JourneyGame,
                };

                return command;
            }

            public static Command Move(Location source, Location destination)
            {
                var command = new Command
                {
                    Operation = Operation.Move,
                    Source = source,
                    Destination = destination,
                };

                return command;
            }

            public static Command Undo()
            {
                var command = new Command
                {
                    Operation = Operation.Undo,
                };

                return command;
            }
        }
    }
}
