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

namespace Core
{
    /// <summary>
    /// The stages of a journey.
    /// </summary>
    public enum Stage
    {
        /// <summary>
        /// The journey has not been begun.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The first part: beating the winnable games from 1 to 32000.
        /// </summary>
        First32000,

        /// <summary>
        /// The second part: beating the winnable games from 32001 to 64000.
        /// </summary>
        Second32000,

        /// <summary>
        /// The journey has been completed.
        /// </summary>
        Finished
    }
}

