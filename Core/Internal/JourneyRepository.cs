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
        private const string Filename = "journey.cslibrecell";
        private readonly DirectoryInfo appDir;

        /// <summary>
        /// Initializes a new instance of the <see cref="JourneyRepository"/> class.
        /// </summary>
        internal JourneyRepository()
        {
            this.appDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CSLibreCell"));
        }

        /// <inheritdoc/>
        public IJourney Read()
        {
            var filepath = Path.Combine(appDir.FullName, Filename);
            var fileinfo = new FileInfo(filepath);

            if (!fileinfo.Exists)
            {
                return null;
            }

            byte[] buffer;

            using (var fs = new FileStream(fileinfo.FullName, FileMode.Open))
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
            if (!this.appDir.Exists)
            {
                this.appDir.Create();
            }

            var filepath = Path.Combine(appDir.FullName, Filename);

            using (var fs = new FileStream(filepath, FileMode.Create))
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
