using System.Collections.Generic;

namespace Core.Internal
{
    internal class Journey : IJourney
    {
        internal Journey(Stage stage, List<uint> games)
        {
            this.Stage = stage;
            this.Games = games;
        }

        public Stage Stage { get; }

        public List<uint> Games { get; }
    }
}
