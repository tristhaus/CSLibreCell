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
