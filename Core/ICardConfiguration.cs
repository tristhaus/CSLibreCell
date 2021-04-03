namespace Core
{
    /// <summary>
    /// Collects configuration related to the cards used.
    /// </summary>
    public interface ICardConfiguration
    {
        /// <summary>
        /// Gets the representation of special ranks, namely, Ace, King, Queen, Jack, Ten
        /// as a concatenated, 5-char string.
        /// </summary>
        string RankRepresentations { get; }
    }
}
