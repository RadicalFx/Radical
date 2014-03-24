using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Topics.Radical.ServiceModel.Hosting
{
	/// <summary>
	/// Provides support for a default service behavior with support for inversion of control.
	/// </summary>
	public class BasicDependencyInjectionServiceBehavior : IServiceBehavior
	{
		readonly IServiceProvider container;
		readonly Type serviceType;

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicDependencyInjectionServiceBehavior"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="serviceType">Type of the service.</param>
		public BasicDependencyInjectionServiceBehavior( IServiceProvider container, Type serviceType )
		{
			this.container = container;
			this.serviceType = serviceType;
		}

		/// <summary>
		/// Gets the instance provider.
		/// </summary>
		/// <param name="serviceType">Type of the service.</param>
		/// <returns>The instance provider.</returns>
		protected virtual IInstanceProvider GetInstanceProvider( Type serviceType ) 
		{
			return new BasicDependencyInjectionInstanceProvider( this.container, serviceType );
		}

		/// <summary>
		/// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
		/// </summary>
		/// <param name="serviceDescription">The service description.</param>
		/// <param name="serviceHostBase">The host that is currently being built.</param>
		public virtual void ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{
			var instanceProvider = this.GetInstanceProvider( this.serviceType );

			foreach( var dispatcherBase in serviceHostBase.ChannelDispatchers )
			{
				var channelDispatcher = dispatcherBase as ChannelDispatcher;
				if( channelDispatcher != null )
				{
					foreach( var ed in channelDispatcher.Endpoints )
					{
						ed.DispatchRuntime.InstanceProvider = instanceProvider;
					}
				}
			}
		}

		/// <summary>
		/// Provides the ability to pass custom data to binding elements to support the contract implementation.
		/// </summary>
		/// <param name="serviceDescription">The service description of the service.</param>
		/// <param name="serviceHostBase">The host of the service.</param>
		/// <param name="endpoints">The service endpoints.</param>
		/// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
		public virtual void AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
			Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters )
		{

		}

		/// <summary>
		/// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
		/// </summary>
		/// <param name="serviceDescription">The service description.</param>
		/// <param name="serviceHostBase">The service host that is currently being constructed.</param>
		public virtual void Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
		{

		}
	}
}
