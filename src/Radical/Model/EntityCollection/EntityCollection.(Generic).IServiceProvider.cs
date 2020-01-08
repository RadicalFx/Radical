using System;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        object IServiceProvider.GetService(Type service)
        {
            if (site != null)
            {
                EnsureNotDisposed();
                return site.GetService(service);
            }

            return null;
        }
    }
}