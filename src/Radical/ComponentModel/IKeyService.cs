namespace Radical.ComponentModel
{
    /// <summary>
    /// Key generation service, an implermentor of this interface
    /// is required to generate domain specific keys.
    /// </summary>
    public interface IKeyService
    {
        /// <summary>
        /// Generates a new empty empty.
        /// </summary>
        /// <returns>The generated empty key.</returns>
        IKey GenerateEmpty();
    }
}