using Topics.Radical.ComponentModel.ChangeTracking;

namespace Topics.Radical.ComponentModel.Factories
{
	/// <summary>
	/// Provides a way to programmatically create
	/// <see cref="IChangeTrackingService"/> instances.
	/// </summary>
	public interface IChangeTrackingServiceFactory
	{
		/// <summary>
		/// Creates a new <see cref="IChangeTrackingService"/> instance.
		/// </summary>
		/// <returns>The new <see cref="IChangeTrackingService"/>.</returns>
		IChangeTrackingService Create();
	}
}
