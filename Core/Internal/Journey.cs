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
    internal class Journey : IJourney
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Journey"/> class.
        /// </summary>
        /// <param name="stage">The stage.</param>
        /// <param name="games">The games in the stage, which may be an empty list.</param>
        internal Journey(Stage stage, List<uint> games)
        {
            this.Stage = stage;
            this.Games = games;
        }

        /// <inheritdoc/>
        public Stage Stage { get; }

        /// <inheritdoc/>
        public List<uint> Games { get; }
    }
}
