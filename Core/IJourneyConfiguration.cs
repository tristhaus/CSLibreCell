using System.IO;

namespace Core
{
    /// <summary>
    /// The configuration for the journey storage.
    /// </summary>
    public interface IJourneyConfiguration
    {
        /// <summary>
        /// Gets the path for the storage.
        /// </summary>
        FileInfo Path { get; }
    }
}
