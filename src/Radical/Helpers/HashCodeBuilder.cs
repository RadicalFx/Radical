namespace Radical.Helpers
{
    /// <summary>
    /// An helper class to generate hash codes based on a value set.
    /// </summary>
    public class HashCodeBuilder
    {
        long combinedHashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashCodeBuilder"/> class.
        /// </summary>
        /// <param name="initialHashCode">The initial hash code.</param>
        public HashCodeBuilder(long initialHashCode)
        {
            this.combinedHashCode = initialHashCode;
        }

        /// <summary>
        /// Adds the given value to the generated has code.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddObject(object value)
        {
            var h = value.GetHashCode();
            this.combinedHashCode = ((this.combinedHashCode << 5) + this.combinedHashCode) ^ h;
        }

        /// <summary>
        /// Gets the combined hash as an <c>int</c> value.
        /// </summary>
        /// <value>The combined hash code.</value>
        public int CombinedHash32
        {
            get { return this.combinedHashCode.GetHashCode(); }
        }

        /// <summary>
        /// Gets the combined hash code.
        /// </summary>
        /// <value>The combined hash code.</value>
        public long CombinedHash
        {
            get { return this.combinedHashCode; }
        }
    }
}