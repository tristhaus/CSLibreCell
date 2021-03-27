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

        /// <summary>
        /// Gets the configuration of the keyboard keys.
        /// </summary>
        internal Keys KeysConfig { get; } = new Keys();

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

        /// <summary>
        /// Collection of keyboard keys configuration.
        /// </summary>
        internal class Keys
        {
            /// <summary>
            /// Gets the configuration of keyboard keys for the menu.
            /// </summary>
            internal Menu Menu { get; } = new Menu();

            /// <summary>
            /// Gets the configuration of keyboard keys for the game.
            /// </summary>
            internal Game Game { get; } = new Game();
        }

        /// <summary>
        /// Collection of keyboard keys configuration for the menu.
        /// </summary>
        internal class Menu
        {
            /// <summary>
            /// Gets or sets the key to start a random game.
            /// </summary>
            internal Key RandomGame { get; set; } = Key.F5;

            /// <summary>
            /// Gets or sets the key to start a chosen game.
            /// </summary>
            internal Key ChooseGame { get; set; } = Key.F8;

            /// <summary>
            /// Gets or sets the key to display the help dialog.
            /// </summary>
            internal Key Help { get; set; } = Key.F1;

            /// <summary>
            /// Gets or sets the key to display the status dialog.
            /// </summary>
            internal Key Status { get; set; } = Key.F12;
        }

        /// <summary>
        /// Collection of keyboard keys configuration for the game.
        /// </summary>
        internal class Game
        {
            /// <summary>
            /// Gets or sets the key to cancel the currently entered move.
            /// </summary>
            internal Key Cancel { get; set; } = Key.Space;

            /// <summary>
            /// Gets or sets the key to undo the last move.
            /// </summary>
            internal Key Undo { get; set; } = (Key)'R';

            /// <summary>
            /// Gets or sets the key to select the first cell.
            /// </summary>
            internal Key Cell0 { get; set; } = (Key)'q';

            /// <summary>
            /// Gets or sets the key to select the second cell.
            /// </summary>
            internal Key Cell1 { get; set; } = (Key)'w';

            /// <summary>
            /// Gets or sets the key to select the third cell.
            /// </summary>
            internal Key Cell2 { get; set; } = (Key)'e';

            /// <summary>
            /// Gets or sets the key to select the fourth cell.
            /// </summary>
            internal Key Cell3 { get; set; } = (Key)'r';

            /// <summary>
            /// Gets or sets the key to select the first column.
            /// </summary>
            internal Key Column0 { get; set; } = (Key)'a';

            /// <summary>
            /// Gets or sets the key to select the second column.
            /// </summary>
            internal Key Column1 { get; set; } = (Key)'s';

            /// <summary>
            /// Gets or sets the key to select the third column.
            /// </summary>
            internal Key Column2 { get; set; } = (Key)'d';

            /// <summary>
            /// Gets or sets the key to select the fourth column.
            /// </summary>
            internal Key Column3 { get; set; } = (Key)'f';

            /// <summary>
            /// Gets or sets the key to select the fifth column.
            /// </summary>
            internal Key Column4 { get; set; } = (Key)'j';

            /// <summary>
            /// Gets or sets the key to select the sixth column.
            /// </summary>
            internal Key Column5 { get; set; } = (Key)'k';

            /// <summary>
            /// Gets or sets the key to select the seventh column.
            /// </summary>
            internal Key Column6 { get; set; } = (Key)'l';

            /// <summary>
            /// Gets or sets the key to select the eighth column.
            /// </summary>
            internal Key Column7 { get; set; } = (Key)';';

            /// <summary>
            /// Gets or sets the key to select the first section of the foundation.
            /// </summary>
            internal Key Foundation0 { get; set; } = (Key)'u';

            /// <summary>
            /// Gets or sets the key to select the second section of the foundation.
            /// </summary>
            internal Key Foundation1 { get; set; } = (Key)'i';

            /// <summary>
            /// Gets or sets the key to select the third section of the foundation.
            /// </summary>
            internal Key Foundation2 { get; set; } = (Key)'o';

            /// <summary>
            /// Gets or sets the key to select the fourth section of the foundation.
            /// </summary>
            internal Key Foundation3 { get; set; } = (Key)'p';
        }
    }
}
