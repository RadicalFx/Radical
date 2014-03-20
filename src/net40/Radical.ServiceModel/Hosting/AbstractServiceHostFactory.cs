using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Topics.Radical.ServiceModel.Hosting
{
	/// <summary>
	/// Factory that provides instances of <see cref="AbstractServiceHostFactory"/> in managed
	/// hosting environments where the host instance is created dynamically in response
	/// to incoming messages.
	/// </summary>
	public abstract class AbstractServiceHostFactory : ServiceHostFactory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractServiceHostFactory"/> class.
		/// </summary>
		protected AbstractServiceHostFactory()
		{

		}

		/// <summary>
		/// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
		/// </summary>
		/// <param name="serviceType">Specifies the type of service to host.</param>
		/// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
		/// <returns>
		/// A <see cref="ServiceHost"/> for the type of service specified with a specific base address.
		/// </returns>
		protected abstract ServiceHost OnCreateServiceHost( Type serviceType, Uri[] baseAddresses );

		/// <summary>
		/// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
		/// </summary>
		/// <param name="serviceType">Specifies the type of service to host.</param>
		/// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
		/// <returns>
		/// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
		/// </returns>
		protected override sealed ServiceHost CreateServiceHost( Type serviceType, Uri[] baseAddresses )
		{
			var host = this.OnCreateServiceHost( serviceType, baseAddresses );

			return host;
		}

		/// <summary>
		/// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> with specific base addresses and initializes it with specified data.
		/// </summary>
		/// <param name="constructorString">The initialization data passed to the <see cref="T:System.ServiceModel.ServiceHostBase"/> instance being constructed by the factory.</param>
		/// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
		/// <returns>
		/// A <see cref="T:System.ServiceModel.ServiceHost"/> with specific base addresses.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="baseAddresses"/> is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">There is no hosting context provided or <paramref name="constructorString"/> is null or empty.</exception>
		public override sealed ServiceHostBase CreateServiceHost( string constructorString, Uri[] baseAddresses )
		{
			return base.CreateServiceHost( constructorString, baseAddresses );
		}
	}
}
