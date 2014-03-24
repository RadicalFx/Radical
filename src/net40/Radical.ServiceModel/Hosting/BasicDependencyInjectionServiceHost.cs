using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Topics.Radical.ServiceModel.Hosting
{
	/// <summary>
	/// Provides a host for services.
	/// </summary>
	public class BasicDependencyInjectionServiceHost : ServiceHost
	{
		readonly Type serviceType;
		readonly IServiceProvider serviceProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicDependencyInjectionServiceHost"/> class.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		/// <param name="serviceType">Type of the service.</param>
		public BasicDependencyInjectionServiceHost(IServiceProvider serviceProvider, Type serviceType )
			: base( serviceType )
		{
			this.serviceType = serviceType;
			this.serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicDependencyInjectionServiceHost"/> class.
		/// </summary>
		/// <param name="serviceProvider">The service provider.</param>
		/// <param name="serviceType">Type of the service.</param>
		/// <param name="baseAddresses">The base addresses.</param>
		public BasicDependencyInjectionServiceHost( IServiceProvider serviceProvider, Type serviceType, params Uri[] baseAddresses )
			: base( serviceType, baseAddresses )
		{
			this.serviceType = serviceType;
			this.serviceProvider = serviceProvider;
		}

		/// <summary>
		/// Gets the instance provider service behavior.
		/// </summary>
		/// <param name="serviceType">Type of the service.</param>
		/// <returns>The instance provider service behavior.</returns>
		protected virtual IServiceBehavior GetInstanceProviderServiceBehavior( Type serviceType ) 
		{
			return new BasicDependencyInjectionServiceBehavior( this.serviceProvider, this.serviceType );
		}

		/// <summary>
		/// Invoked during the transition of a communication object into the opening state.
		/// </summary>
		protected override sealed void OnOpening()
		{
			var behavior = this.GetInstanceProviderServiceBehavior( this.serviceType );
			this.Description.Behaviors.Add( behavior );

			base.OnOpening();
		}
	}
}
