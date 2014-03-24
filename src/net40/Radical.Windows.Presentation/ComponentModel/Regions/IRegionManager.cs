using System;
using System.Collections.Generic;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// A region manager is responsible for managing region lifetime and arrangement.
	/// </summary>
	public interface IRegionManager
	{
		/// <summary>
		/// Registers the supplied region in this region manager.
		/// </summary>
		/// <param name="region">The region to register.</param>
		void RegisterRegion( IRegion region );

		/// <summary>
		/// Gets the <see cref="IRegion"/> with the specified name.
		/// </summary>
		/// <value>THe requested region.</value>
		/// <exception cref="ArgumentOutOfRangeException">An ArgumentOutOfRangeException is raised if a region with the specifed name cannot be found.</exception>
		IRegion this[ String name ] { get; }

		/// <summary>
		/// Gets all the registered the regions.
		/// </summary>
		/// <returns>All the registered the regions.</returns>
		IEnumerable<IRegion> GetAllRegisteredRegions();

		/// <summary>
		/// Gets the region registered with the given name.
		/// </summary>
		/// <param name="name">The name of the region.</param>
		/// <returns>The searched region, or an ArgumentOutOfRangeException if no region is registered with the given name.</returns>
		IRegion GetRegion( String name );

		/// <summary>
		/// Gets the region.
		/// </summary>
		/// <typeparam name="TRegion">The type of the region.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns>The searched region, or an ArgumentOutOfRangeException if no region is registered with the given name.</returns>
		TRegion GetRegion<TRegion>( String name ) 
			where TRegion : IRegion;

		/// <summary>
		/// Tries to get the region.
		/// </summary>
		/// <param name="regionName">Name of the region.</param>
		/// <param name="region">The region.</param>
		/// <returns>
		///   <c>True</c> if the region has been found; otherwise <c>false</c>.
		/// </returns>
		Boolean TryGetRegion( String regionName, out IRegion region );

		/// <summary>
		/// Tries to get the region.
		/// </summary>
		/// <typeparam name="TRegion">The type of the region.</typeparam>
		/// <param name="regionName">Name of the region.</param>
		/// <param name="region">The region.</param>
		/// <returns><c>True</c> if the region has been found; otherwise <c>false</c>.</returns>
		Boolean TryGetRegion<TRegion>( String regionName, out TRegion region ) 
			where TRegion : IRegion;

		/// <summary>
		/// Shutdowns this region manager, the shutdown process is invoked by the region service at close time.
		/// </summary>
		void Shutdown();
	}
}