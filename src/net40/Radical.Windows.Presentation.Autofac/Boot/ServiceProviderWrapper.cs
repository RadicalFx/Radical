using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Topics.Radical.Windows.Presentation.Boot
{
	class ServiceProviderWrapper : IServiceProvider
	{
		IContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceProviderWrapper" /> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public ServiceProviderWrapper( IContainer container )
		{
			this.container = container;
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
			if ( this.container.IsRegistered( serviceType ) )
			{
				return this.container.Resolve( serviceType );
			}

			return null;
		}
	}
}
