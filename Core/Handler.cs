﻿using System;

namespace Core
{
    public class Handler
    {
        private Game game = null;

        public string UnicodeGameRepresentation => this.game?.UnicodeRepresentation ?? string.Empty;

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
                    return true;

                case Operation.Move:
                    var isLegal = this.game.IsMoveLegal((Location)command.Source, (Location)command.Destination);

                    if (isLegal)
                    {
                        this.game.MakeMove((Location)command.Source, (Location)command.Destination);
                    }
            
                    return isLegal;
            
                default:
                    throw new Exception($"enum member '{command.Operation}' missing in {nameof(ExecuteCommand)}");
            }
        }

        internal enum Operation
        {
            NewGame,
            Move,
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
        }
    }
}