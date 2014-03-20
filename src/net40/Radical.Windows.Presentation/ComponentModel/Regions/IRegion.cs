using System;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// A region identifies a pluggable area in the user interface, 
	/// where shell and modules can plug their own UI.
	/// </summary>
	public interface IRegion
	{
		/// <summary>
		/// Gets the name of the region.
		/// </summary>
		/// <value>The name.</value>
		String Name { get; }

		/// <summary>
		/// Gets the view that hosts this region.
		/// </summary>
		DependencyObject HostingView { get; }

		/// <summary>
		/// The number of milliseconds to wait before loading the region content.
		/// </summary>
		Int32 AsyncLoadDelay { get; set; }

#if SILVERLIGHT

		/// <summary>
		/// Occurs when this region is ready.
		/// </summary>
		event EventHandler Ready;
#endif

		/// <summary>
		/// Shutdowns this region, the shutdown process is invoked by the hosting region manager at close time.
		/// </summary>
		void Shutdown();
	}
}
