namespace Radical.ComponentModel.ChangeTracking
{
    /// <summary>
    /// Represents a change occurred to an object.
    /// </summary>
    /// <typeparam name="T">The type of the object that owns the change.</typeparam>
    public interface IChange<out T> : IChange
    {
        /// <summary>
        /// Gets the cached value.
        /// </summary>
        /// <value>The cached value.</value>
        T CachedValue { get; }
    }
}