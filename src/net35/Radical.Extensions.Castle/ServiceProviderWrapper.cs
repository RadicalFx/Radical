using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;

namespace Castle
{
    /// <summary>
    /// Wraps the <see cref="IWindsorContainer"/> in order to provide
    /// a legacy <see cref="IServiceProvider"/> instance.
    /// </summary>
    public class ServiceProviderWrapper : IServiceProvider, IDisposable
    {
		void IDisposable.Dispose()
		{
			if( this.Container != null ) 
			{
				this.Container.Dispose();
				this.Container = null;
			}
		}

		/// <summary>
		/// Gets the container.
		/// </summary>
		/// <value>
		/// The container.
		/// </value>
		public IWindsorContainer Container { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceProviderWrapper" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ServiceProviderWrapper( IWindsorContainer container )
        {
            this.Container = container;
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />.
        /// -or-
        /// null if there is no service object of type <paramref name="serviceType" />.
        /// </returns>
        public object GetService( Type serviceType )
        {
            if ( this.Container.Kernel.HasComponent( serviceType ) )
            {
                return this.Container.Resolve( serviceType );
            }

            return null;
        }
	}
}
