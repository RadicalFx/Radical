using System;
using System.Windows;
namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Handles the process to inject views into regions.
	/// </summary>
	public interface IRegionInjectionHandler
	{
		/// <summary>
		/// Gets or sets the inject handler used to inject the given view into the given region.
		/// </summary>
		/// <value>
		/// The inject handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
        Action<Func<DependencyObject>, IRegion> Inject { get; set; }

		/// <summary>
		/// Gets the views interested in the region identified by the given name.
		/// </summary>
		/// <param name="regionName">Name of the region.</param>
		/// <returns>
		/// A list of view types.
		/// </returns>
		System.Collections.Generic.IEnumerable<Type> GetViewsInterestedIn( string regionName );

		/// <summary>
		/// Registers the view as interested in the region that has the given region name.
		/// </summary>
		/// <param name="regionName">Name of the region.</param>
		/// <param name="viewType">Type of the view.</param>
		void RegisterViewAsInterestedIn( string regionName, Type viewType );

		/// <summary>
		/// Registers the views as interested in the region that has the given region name.
		/// </summary>
		/// <param name="regionName">Name of the region.</param>
		/// <param name="views">The views.</param>
		void RegisterViewsAsInterestedIn( string regionName, System.Collections.Generic.IEnumerable<Type> views );
	}
}
