using System;
using System.Windows;
using System.Windows.Controls;
using Topics.Radical.Windows.Controls;

namespace Topics.Radical.Windows.Controls
{
	/// <summary>
	/// A generic adorner capable of presenting content 
	/// on top of the adorned element.
	/// </summary>
	public class OverlayAdorner : Adorner
	{
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();

			( ( ContentPresenter )this.Content ).Content = null;
			this.Content = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OverlayAdorner"/> class.
		/// </summary>
		/// <param name="adornedElement">The adorned element.</param>
		/// <param name="userContent">Content of the user.</param>
		public OverlayAdorner( UIElement adornedElement, Object userContent )
			: base( adornedElement )
		{
			this.IsHitTestVisible = true;
			this.Content = new ContentPresenter() { Content = userContent };
		}
	}
}
