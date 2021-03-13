using System;
using System.Collections.Generic;

namespace Core
{
    public class Handler
    {
        private Game game = null;
        private readonly Stack<Game> gameStates = new Stack<Game>();

        public IGame Game => this.game;

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

                case Operation.Move:
                    if (!this.game.IsWon)
                    {
                        var moveSize = this.game.GetLegalMoveSize((Location)command.Source, (Location)command.Destination);

                        if (moveSize > 0)
                        {
                            this.gameStates.Push(new Game(this.game));
                            this.game.MakeMove((Location)command.Source, (Location)command.Destination, moveSize);

                            while (this.game.AutoMoveToFoundation()) ;

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

        internal enum Operation
        {
            NewGame,
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
