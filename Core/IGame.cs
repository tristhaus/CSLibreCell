using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Represents information about a currently loaded game.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Gets the ID of the game.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Gets a value indicating whether the game is already won.
        /// </summary>
        bool IsWon { get; }

        /// <summary>
        /// Gets a value indicating whether the game cannot be won.
        /// </summary>
        bool IsImpossibleToWin { get; }

        /// <summary>
        /// Gets the collection of cells (upper left-hand area), which can hold zero or one card.
        /// </summary>
        IReadOnlyList<Card> Cells { get; }

        /// <summary>
        /// Gets the collection of foundations (upper right-hand area), which hold cards no longer in play.
        /// </summary>
        IReadOnlyList<Card> Foundations { get; }

        /// <summary>
        /// Gets the collection of columns (lower area), which can hold zero to 19 cards each (in the standard 8-column variant).
        /// </summary>
        IReadOnlyList<IReadOnlyList<Card>> Columns { get; }
    }
}
