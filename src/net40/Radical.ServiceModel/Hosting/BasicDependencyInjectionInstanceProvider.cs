using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Topics.Radical.ComponentModel;
using Topics.Radical.Reflection;
using Topics.Radical.Linq;

namespace Topics.Radical.ServiceModel.Hosting
{
	/// <summary>
	/// Provides support for a default instance provider with support for inversion of control.
	/// </summary>
	public class BasicDependencyInjectionInstanceProvider : IInstanceProvider
	{
		readonly Type serviceType;
		readonly Type serviceContract;
		readonly IServiceProvider container;

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicDependencyInjectionInstanceProvider"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="serviceType">Type of the service.</param>
		public BasicDependencyInjectionInstanceProvider( IServiceProvider container, Type serviceType )
		{
			this.container = container;
			this.serviceType = serviceType;
			this.serviceContract = this.GetServiceContract( this.serviceType );
		}

		/// <summary>
		/// Gets the service contract.
		/// </summary>
		/// <param name="serviceType">Type of the service.</param>
		/// <returns>The service contract.</returns>
		protected virtual Type GetServiceContract( Type serviceType )
		{
			var interfaces = serviceType.GetInterfaces();
			if( interfaces.Any() )
			{
				var contract = interfaces
					.Where( t => t.IsAttributeDefined<ContractAttribute>() )
					.SingleOr( () => interfaces.First() );

				return contract;
			}

			return serviceType;
		}

		/// <summary>
		/// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
		/// </summary>
		/// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
		/// <returns>A user-defined service object.</returns>
		public object GetInstance( InstanceContext instanceContext )
		{
			return this.GetInstance( instanceContext, null );
		}

		/// <summary>
		/// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
		/// </summary>
		/// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
		/// <param name="message">The message that triggered the creation of a service object.</param>
		/// <returns>The service object.</returns>
		public virtual object GetInstance( InstanceContext instanceContext, Message message )
		{
			return this.container.GetService( this.serviceContract );
		}

		/// <summary>
		/// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
		/// </summary>
		/// <param name="instanceContext">The service's instance context.</param>
		/// <param name="instance">The service object to be recycled.</param>
		public virtual void ReleaseInstance( InstanceContext instanceContext, object instance )
		{

		}
	}
}
