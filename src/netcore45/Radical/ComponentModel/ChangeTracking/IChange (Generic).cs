namespace Topics.Radical.ComponentModel.ChangeTracking
{
	/// <summary>
	/// Represents a change occurred to an object.
	/// </summary>
	public interface IChange<T> : IChange
	{
		/// <summary>
		/// Gets the cached value.
		/// </summary>
		/// <value>The cached value.</value>
		T CachedValue { get; }
	}
}