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

namespace Core.Internal
{
    /// <summary>
    /// Provides information on the journey through the games.
    /// </summary>
    public interface IJourney
    {
        /// <summary>
        /// Gets the stage of the journey.
        /// </summary>
        Stage Stage { get; }

        /// <summary>
        /// Gets the list of games in the current stage, which can be empty.
        /// </summary>
        List<uint> Games { get; }
    }
}
