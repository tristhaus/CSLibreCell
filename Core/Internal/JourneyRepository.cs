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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Internal
{
    /// <summary>
    /// Disk-based repository for a journey.
    /// </summary>
    internal class JourneyRepository : IJourneyRepository
    {
        private readonly IJourneyConfiguration journeyConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JourneyRepository"/> class.
        /// </summary>
        /// <param name="journeyConfiguration">The configuration concerning the storage of the journey.</param>
        internal JourneyRepository(IJourneyConfiguration journeyConfiguration)
        {
            this.journeyConfiguration = journeyConfiguration;
        }

        /// <inheritdoc/>
        public IJourney Read()
        {
            if (!journeyConfiguration.Path.Exists)
            {
                return null;
            }

            byte[] buffer;

            using (var fs = new FileStream(journeyConfiguration.Path.FullName, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }

            if (buffer.Length == 0)
            {
                return null;
            }

            var stageBytes = new ArraySegment<byte>(buffer, 0, 4);
            var stage = (Stage)ByteConverter.LSBBytesToUint(stageBytes);

            var games = new List<uint>();
            for (int i = 4; i < buffer.Length; i += 4)
            {
                var gameBytes = new ArraySegment<byte>(buffer, i, 4);

                var gameId = ByteConverter.LSBBytesToUint(gameBytes);

                games.Add(gameId);
            }

            return new Journey(stage, games);
        }

        /// <inheritdoc/>
        public void Write(IJourney journey)
        {
            if (!this.journeyConfiguration.Path.Directory.Exists)
            {
                this.journeyConfiguration.Path.Directory.Create();
            }

            using (var fs = new FileStream(this.journeyConfiguration.Path.FullName, FileMode.Create))
            {
                uint stage = Convert.ToUInt32((int)journey.Stage);
                var stageBytes = ByteConverter.UintToLSBBytes(stage);

                fs.Write(stageBytes, 0, stageBytes.Length);

                var gameBytes = journey.Games
                    .SelectMany(x => ByteConverter.UintToLSBBytes(x))
                    .ToArray();

                fs.Write(gameBytes, 0, gameBytes.Length);
            }
        }
    }
}
