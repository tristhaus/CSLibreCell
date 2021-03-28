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

namespace Core.Internal
{
    /// <summary>
    /// Provides methods to read a journey from e.g. disk and write to the same source.
    /// </summary>
    internal interface IJourneyRepository
    {
        /// <summary>
        /// Reads the journey from the medium associated with the implementation.
        /// </summary>
        /// <returns>The read journey.</returns>
        IJourney Read();

        /// <summary>
        /// Writes the journey to the  medium associated with the implementation.
        /// </summary>
        /// <param name="journey">The journey to persist.</param>
        void Write(IJourney journey);
    }
}
