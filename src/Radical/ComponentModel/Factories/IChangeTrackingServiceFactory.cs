using System;
using Radical.ComponentModel.ChangeTracking;

namespace Radical.ComponentModel.Factories
{
    /// <summary>
    /// Provides a way to programmatically create
    /// <see cref="IChangeTrackingService"/> instances.
    /// </summary>
    [Obsolete("ChangeTrackingServiceFactory has been obsoleted and will be removed in v3.0.0")]
    public interface IChangeTrackingServiceFactory
    {
        /// <summary>
        /// Creates a new <see cref="IChangeTrackingService"/> instance.
        /// </summary>
        /// <returns>The new <see cref="IChangeTrackingService"/>.</returns>
        IChangeTrackingService Create();
    }
}
