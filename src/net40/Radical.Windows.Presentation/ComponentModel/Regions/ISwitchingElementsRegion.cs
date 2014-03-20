using System;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// A region that holds elements that can be switched, e.g. a Tab Control.
	/// </summary>
	public interface ISwitchingElementsRegion : IElementsRegion
	{
		/// <summary>
		/// Gets the content of the active.
		/// </summary>
		/// <value>
		/// The content of the active.
		/// </value>
		DependencyObject ActiveContent { get; }

		/// <summary>
		/// Gets the content of the previous active.
		/// </summary>
		/// <value>
		/// The content of the previous active.
		/// </value>
		DependencyObject PreviousActiveContent { get; }

		/// <summary>
		/// Occurs when the active content changes.
		/// </summary>
		event EventHandler<ActiveContentChangedEventArgs> ActiveContentChanged;

		/// <summary>
		/// Activates the specified content.
		/// </summary>
		/// <param name="content">The content.</param>
		void Activate( DependencyObject content );
	}
}
