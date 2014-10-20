using System;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// The region service is responsible for managing regions, shell regions and
	/// modules specific regions.
	/// </summary>
	public interface IRegionService
	{
		/// <summary>
		/// Determines if this region service has knowledge of a region manager owned
		/// by the supplied owner.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <returns>True if this region service has a reference to a region manager owned by the supplied owner, otherwise false.</returns>
		Boolean HoldsRegionManager( DependencyObject owner );

		/// <summary>
		/// Gets the region manager owned by the supplied view.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <returns>A reference to the requested region manager.</returns>
		/// <exception cref="ArgumentOutOfRangeException">An ArgumentOutOfRangeException is raised if this service has no knowledge of a region manager owned by the supplied owner. Use HoldsRegionManager() to test.</exception>
		IRegionManager GetRegionManager( DependencyObject owner );

		/// <summary>
		/// Gets a known region manager. A known region manager is a region
		/// manager associated with a view type and not with a view instance.
		/// Tipically this region manager is a static region manager that exists
		/// for all the application lifecycle. A good sample of a known region
		/// manager is the Shell RegionManager that exists always.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <returns>The requested region manager</returns>
		IRegionManager GetKnownRegionManager<TView>() where TView : DependencyObject;

        /// <summary>
        /// Finds a region manager given a custom search logic.
        /// </summary>
        /// <param name="filter">A predicate executed for all the registered region managers.</param>
        /// <returns>The found region manager or null.</returns>
        IRegionManager FindRegionManager( Func<DependencyObject, IRegionManager, Boolean> filter );

		/// <summary>
		/// Registers a new region manager for the given owner.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <returns>The newly registered region manager.</returns>
		/// <exception cref="NotSupportedException">A NotSupportedException is raised if this service has already registered a region manager for the supplied owner. Use HoldsRegionManager() to test.</exception>
		IRegionManager RegisterRegionManager( DependencyObject owner );

		/// <summary>
		/// Unregisters the region manager owned by the supplied owner.
		/// </summary>
		/// <param name="owner">The owner of the region manager to unregister.</param>
		/// <exception cref="NotSupportedException">A NotSupportedException is raised if this service has no region manager registered for the supplied owner. Use HoldsRegionManager() to test.</exception>
		void UnregisterRegionManager( DependencyObject owner );

		/// <summary>
		/// Unregisters the region manager owned by the supplied owner.
		/// </summary>
		/// <param name="owner">The owner of the region manager to unregister.</param>
		/// <param name="behavior">How to manage reguion manager found in child/nested views.</param>
		/// <exception cref="NotSupportedException">A NotSupportedException is raised if this service has no region manager registered for the supplied owner. Use HoldsRegionManager() to test.</exception>
		void UnregisterRegionManager( DependencyObject owner, UnregisterBehavior behavior );
	}

	/// <summary>
	/// Determins the behavior the the region service should respect when un-registering region managers.
	/// </summary>
	public enum UnregisterBehavior 
	{
		/// <summary>
		/// Only the given region manager will be unregistered.
		/// </summary>
		Default,

		/// <summary>
		/// The logical tree will be scanned, down, looking for region managers hold by nested views, if any it will be unregistered.
		/// </summary>
		WholeLogicalTreeChain
	}
}
