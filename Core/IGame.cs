/*
 * This file is part of CSLibreCell.
 * 
 * CSLibreCell is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * CSLibreCell is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with CSLibreCell.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

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
