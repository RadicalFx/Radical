using Radical.ComponentModel.ChangeTracking;
using Radical.ComponentModel.Factories;
using Radical.Validation;
using System;

namespace Radical.Model.Factories
{
    public class ChangeTrackingServiceFactory : IChangeTrackingServiceFactory
    {
        readonly IServiceProvider container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTrackingServiceFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ChangeTrackingServiceFactory(IServiceProvider container)
        {
            Ensure.That(container).Named("container").IsNotNull();

            this.container = container;
        }

        /// <summary>
        /// Creates a new <see cref="IChangeTrackingService"/> instance.
        /// </summary>
        /// <returns>
        /// The new <see cref="IChangeTrackingService"/>.
        /// </returns>
        public IChangeTrackingService Create()
        {
            return (IChangeTrackingService)container.GetService(typeof(IChangeTrackingService));
        }
    }
}
