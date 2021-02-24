namespace Core
{
    /// <summary>
    /// Implementation of the necessary, ancient Pseudo-Random Number Generator.
    /// </summary>
    internal class Prng
    {
        private const uint Two16 = 65536; //google 2^16
        private const uint Two31 = 2147483648; //google 2^31
        private uint state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Prng"/> class.
        /// </summary>
        public Prng()
        {
            this.state = 0;
        }

        /// <summary>
        /// Initializes the state of the PRNG to the given number.
        /// </summary>
        /// <param name="newState">The new state to set.</param>
        internal void Initialize(uint newState)
        {
            this.state = newState;
        }

        /// <summary>
        /// Gets the next random number and advances the state of the PRNG.
        /// </summary>
        /// <returns>The next random number.</returns>
        internal uint GetNext()
        {
            this.state = (214013 * this.state + 2531011) % Two31;
            return this.state / Two16;
        }
    }
}
