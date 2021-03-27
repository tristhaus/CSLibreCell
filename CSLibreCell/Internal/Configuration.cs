using Core;
using System.Globalization;
using System.IO;
using Terminal.Gui;

namespace CSLibreCell.Internal
{
    /// <summary>
    /// Collection of effective configuration options.
    /// </summary>
    internal class Configuration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="journeyPath">The info concerning the storage location for the journey.</param>
        public Configuration(FileInfo journeyPath, CultureInfo uiCulture)
        {
            this.JourneyConfig = new Journey(journeyPath);
            this.UiCulture = uiCulture;
        }

        /// <summary>
        /// Gets the configuration concerning the journey.
        /// </summary>
        public Journey JourneyConfig { get; }

        /// <summary>
        /// Gets the configured UI culture. Can be <c>null</c>.
        /// </summary>
        public CultureInfo UiCulture { get; }

        /// <inheritdoc/>
        internal class Journey : IJourneyConfiguration
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Journey"/> class.
            /// </summary>
            /// <param name="path">The info concerning the storage location.</param>
            public Journey(FileInfo path)
            {
                this.Path = path;
            }

            /// <inheritdoc/>
            public FileInfo Path { get; }
        }

        internal static class Keys
        {
            internal static class Menu
            {
                internal static Key RandomGame => Key.F5;

                internal static Key ChooseGame => Key.F8;

                internal static Key Help => Key.F1;

                internal static Key Status => Key.F12;
            }

            internal static class Game
            {
                internal static Key Cancel => Key.Space;

                internal static Key Undo => (Key)'R';

                internal static Key Cell0 => (Key)'q';

                internal static Key Cell1 => (Key)'w';

                internal static Key Cell2 => (Key)'e';

                internal static Key Cell3 => (Key)'r';

                internal static Key Column0 => (Key)'a';

                internal static Key Column1 => (Key)'s';

                internal static Key Column2 => (Key)'d';

                internal static Key Column3 => (Key)'f';

                internal static Key Column4 => (Key)'j';

                internal static Key Column5 => (Key)'k';

                internal static Key Column6 => (Key)'l';

                internal static Key Column7 => (Key)'ö';

                internal static Key Foundation0 => (Key)'u';

                internal static Key Foundation1 => (Key)'i';

                internal static Key Foundation2 => (Key)'o';

                internal static Key Foundation3 => (Key)'p';
            }
        }
    }
}
