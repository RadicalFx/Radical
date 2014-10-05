using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Defines that a ViewModel expects navigation notifications from the page view.
	/// </summary>
	public interface IExpectNavigationCallback
	{
		/// <summary>
		/// Called in order to notify the view model that the navigation service
		/// is ready to navigate away from the current view.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
		void OnNavigatingAway( NavigatingCancelEventArgs e );

		/// <summary>
		/// Called in order to notify the view model that the navigation service
		/// has navigated to the current view.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
		void OnNavigatedTo( NavigationEventArgs e );
	}
}
