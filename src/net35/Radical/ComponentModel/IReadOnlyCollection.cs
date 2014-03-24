namespace Topics.Radical.ComponentModel
{
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// A read-only collection of items.
	/// </summary>
	/// <typeparam name="T">The type of the collection item.</typeparam>
	public interface IReadOnlyCollection<T> : ICollection, IEnumerable, IEnumerable<T>
	{
		
	}
}
