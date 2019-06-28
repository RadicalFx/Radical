
namespace Radical.ComponentModel
{
    /// <summary>
    /// A domain entity that can be identified by a primary key.
    /// </summary>
    public interface IUniqueEntity
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        IKey Key { get; }
    }
}