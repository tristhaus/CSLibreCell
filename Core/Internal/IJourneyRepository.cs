namespace Core.Internal
{
    /// <summary>
    /// Provides methods to read a journey from e.g. disk and write to the same source.
    /// </summary>
    internal interface IJourneyRepository
    {
        /// <summary>
        /// Reads the journey from the medium associated with the implementation.
        /// </summary>
        /// <returns>The read journey.</returns>
        IJourney Read();

        /// <summary>
        /// Writes the journey to the  medium associated with the implementation.
        /// </summary>
        /// <param name="journey">The journey to persist.</param>
        void Write(IJourney journey);
    }
}
