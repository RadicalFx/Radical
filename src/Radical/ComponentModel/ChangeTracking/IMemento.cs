namespace Radical.ComponentModel.ChangeTracking
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

    /// <summary>
    /// Determines how to register the entity with the <see cref="IChangeTrackingService"/>
    /// </summary>
    public enum ChangeTrackingRegistration
    {
        /// <summary>
        /// The entity will be registered as transient.
        /// </summary>
        AsTransient = 0,

        /// <summary>
        /// The entity will be registered as persistent.
        /// </summary>
        AsPersistent = 1
    }

    /// <summary>
    /// Determines the type oo the transient registration.
    /// </summary>
    public enum TransientRegistration
    {
        /// <summary>
        /// The transient entity will be registered in the change tracking service
        /// as a transaprent entity, this means that if the transient entity has no
        /// changes it will not be considered as changed but will be ignored.
        /// </summary>
        AsTransparent = 0,

        /// <summary>
        /// The transient entity will be registered in the change tracking service
        /// as a persistable entity, this means that if the transient entity has no
        /// changes it will be considered as changed and won't be ignored.
        AsPersistable = 1
    }
}
