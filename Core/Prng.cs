namespace Core
{
    /// <summary>
    /// Implementation of the necessary, ancient Pseudo-Random Number Generator.
    /// </summary>
    public class Prng
    {
        private const uint Two16 = 65536; //google 2^16
        private const uint Two31 = 2147483648; //google 2^31
        private uint state;

        public Prng()
        {
            this.state = 0;
        }

        internal void Initialize(uint newState)
        {
            this.state = newState;
        }

        internal uint GetNext()
        {
            this.state = (214013 * this.state + 2531011) % Two31;
            return this.state / Two16;
        }
    }
}
