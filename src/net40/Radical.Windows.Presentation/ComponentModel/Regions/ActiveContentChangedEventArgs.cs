using System;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// The ActiveContentChanged events arguments.
	/// </summary>
	public class ActiveContentChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ActiveContentChangedEventArgs"/> class.
		/// </summary>
		/// <param name="activeContent">Content of the active.</param>
		/// <param name="previousActiveContent">Content of the previous active.</param>
		public ActiveContentChangedEventArgs( DependencyObject activeContent, DependencyObject previousActiveContent )
		{
			this.ActiveContent = activeContent;
			this.PreviousActiveContent = previousActiveContent;
		}

		/// <summary>
		/// Gets the content of the active.
		/// </summary>
		/// <value>
		/// The content of the active.
		/// </value>
		public DependencyObject ActiveContent
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the content of the previous active.
		/// </summary>
		/// <value>
		/// The content of the previous active.
		/// </value>
		public DependencyObject PreviousActiveContent
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a value indicating whether there is an active content.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if there is an active content; otherwise, <c>false</c>.
		/// </value>
		public Boolean HasActiveContent
		{
			get { return this.ActiveContent != null; }
		}
	}
}
