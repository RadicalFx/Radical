using System;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">The service.</param>
        /// <returns>The requested service, or <c>null</c> if this collection is not sited.</returns>
        object IServiceProvider.GetService(Type serviceType)
        {
            if (site != null)
            {
                EnsureNotDisposed();
                return site.GetService(serviceType);
            }

            return null;
        }
    }
}