namespace Core
{
    /// <summary>
    /// The stages of a journey.
    /// </summary>
    public enum Stage
    {
        /// <summary>
        /// The journey has not been begun.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The first part: beating the winnable games from 1 to 32000.
        /// </summary>
        First32000,

        /// <summary>
        /// The second part: beating the winnable games from 32001 to 64000.
        /// </summary>
        Second32000,

        /// <summary>
        /// The journey has been completed.
        /// </summary>
        Finished
    }
}

