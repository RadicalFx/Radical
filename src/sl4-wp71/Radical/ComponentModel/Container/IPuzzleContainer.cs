using System;
using System.Collections.Generic;

namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Defines the contract of the Puzzle Inversion of Control container.
	/// </summary>
	public interface IPuzzleContainer : IServiceProvider, IDisposable
	{
		/// <summary>
		/// Occurs when a component is registered in this container.
		/// </summary>
		event EventHandler<ComponentRegisteredEventArgs> ComponentRegistered;

		/// <summary>
		/// Adds the given facility instance to this container.
		/// </summary>
		/// <param name="facility">The facility.</param>
		/// <returns>This container instance.</returns>
		IPuzzleContainer AddFacility( IPuzzleContainerFacility facility );

		/// <summary>
		/// Adds a new facility.
		/// </summary>
		/// <typeparam name="TFacility">The type of the facility.</typeparam>
		/// <returns>This container instance.</returns>
		IPuzzleContainer AddFacility<TFacility>() where TFacility : IPuzzleContainerFacility;

		/// <summary>
		/// Registers all the specified entries.
		/// </summary>
		/// <param name="entries">The entries to register.</param>
		/// <returns>This container instance.</returns>
		IPuzzleContainer Register( IEnumerable<IContainerEntry> entries );
		
		/// <summary>
		/// Registers the specified entry in this container.
		/// </summary>
		/// <param name="entry">The entry to register.</param>
		/// <returns>This container instance.</returns>
		IPuzzleContainer Register( IContainerEntry entry );

		/// <summary>
		/// Determines whether the given service type is registered.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <returns>
		/// 	<c>true</c> if the given service type is registered; otherwise, <c>false</c>.
		/// </returns>
		Boolean IsRegistered<TService>();

		/// <summary>
		/// Determines whether the given service type is registered.
		/// </summary>
		/// <param name="serviceType">The type of the service.</param>
		/// <returns>
		/// 	<c>true</c> if the given service type is registered; otherwise, <c>false</c>.
		/// </returns>
		Boolean IsRegistered( Type serviceType );

		/// <summary>
		/// Resolves the specified service type.
		/// </summary>
		/// <param name="serviceType">The Type of the service.</param>
		/// <returns>The resolved service instance.</returns>
		Object Resolve( Type serviceType );

		/// <summary>
		/// Resolves the specified service type.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <returns>The resolved service instance.</returns>
		TService Resolve<TService>();
	}
}
