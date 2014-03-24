namespace Topics.Radical.ComponentModel.ChangeTracking
{
	/// <summary>
	/// The <c>IMemento</c> interface defines that an entity explicitly
	/// requires a memento service.
	/// </summary>
	public interface IMemento
	{
		/// <summary>
		/// Gets or sets the change tracking service to use as memento
		/// features provider.
		/// </summary>
		/// <value>The change tracking service.</value>
		IChangeTrackingService Memento { get; set; }
	}
}
