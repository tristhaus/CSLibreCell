using System.Collections.Generic;

namespace Core.Internal
{
    /// <summary>
    /// Provides information on the journey through the games.
    /// </summary>
    public interface IJourney
    {
        /// <summary>
        /// Gets the stage of the journey.
        /// </summary>
        Stage Stage { get; }

        /// <summary>
        /// Gets the list of games in the current stage, which can be empty.
        /// </summary>
        List<uint> Games { get; }
    }
}
