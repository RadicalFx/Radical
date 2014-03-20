using System;
using Microsoft.Phone.Controls;
using System.Windows;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Phone.Shell.Services
{
	/// <summary>
	/// Defines an application service used to manage the navigation in a Windows Phone application.
	/// </summary>
	[Contract]
	public interface INavigationService : IApplicationService
	{
		/// <summary>
		/// Gets the current page if any.
		/// </summary>
		/// <value>The current page.</value>
		PhoneApplicationPage CurrentPage { get; }

		/// <summary>
		/// Gets a value indicating whether this instance can navigate back.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance can navigate back; otherwise, <c>false</c>.
		/// </value>
		bool CanNavigateBack { get; }

		/// <summary>
		/// Gets a value indicating whether this instance can navigate forward.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance can navigate forward; otherwise, <c>false</c>.
		/// </value>
		bool CanNavigateForward { get; }

		/// <summary>
		/// Navigates back.
		/// </summary>
		void NavigateBack();

		/// <summary>
		/// Navigates forward.
		/// </summary>
		void NavigateForward();

		/// <summary>
		/// Navigates to the specified destination URI.
		/// </summary>
		/// <param name="destination">The destination URI.</param>
		/// <returns><c>True</c> if the navigation succeeds; otherwise <c>false</c>.</returns>
		Boolean Navigate( Uri destination );
	}
}
