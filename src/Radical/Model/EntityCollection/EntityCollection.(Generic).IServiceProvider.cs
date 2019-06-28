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
        object IServiceProvider.GetService( Type service )
        {
            if( this.site != null )
            {
                this.EnsureNotDisposed();
                return this.site.GetService( service );
            }

            return null;
        }
    }
}