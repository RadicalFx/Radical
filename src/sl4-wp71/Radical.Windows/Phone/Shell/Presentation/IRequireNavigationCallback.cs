using System;
using System.Windows.Navigation;

namespace Topics.Radical.Windows.Phone.Shell.Presentation
{
	/// <summary>
	/// Determines the a view model requires notifications callback.
	/// </summary>
	public interface IRequireNavigationCallback
	{
		/// <summary>
		/// Raises the <see cref="E:Navigating"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
		void OnNavigating( NavigatingCancelEventArgs e );

		/// <summary>
		/// Raises the <see cref="E:Navigated"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
		void OnNavigated( NavigationEventArgs e );
	}
}
