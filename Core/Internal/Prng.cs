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
    /// Implementation of the necessary, ancient Pseudo-Random Number Generator.
    /// </summary>
    internal class Prng
    {
        private const uint Two16 = 65536; //google 2^16
        private const uint Two31 = 2147483648; //google 2^31
        private uint state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Prng"/> class.
        /// </summary>
        public Prng()
        {
            this.state = 0;
        }

        /// <summary>
        /// Initializes the state of the PRNG to the given number.
        /// </summary>
        /// <param name="newState">The new state to set.</param>
        internal void Initialize(uint newState)
        {
            this.state = newState;
        }

        /// <summary>
        /// Gets the next random number and advances the state of the PRNG.
        /// </summary>
        /// <returns>The next random number.</returns>
        internal uint GetNext()
        {
            this.state = (214013 * this.state + 2531011) % Two31;
            return this.state / Two16;
        }
    }
}
