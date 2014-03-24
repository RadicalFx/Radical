using System;
using System.Windows;
using System.Windows.Controls;
using Topics.Radical.Windows.Controls;

namespace Topics.Radical.Windows.Behaviors
{
	sealed class BusyAdorner : OverlayAdorner
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BusyAdorner"/> class.
		/// </summary>
		/// <param name="adornedElement">The adorned element.</param>
		/// <param name="userContent">Content of the user.</param>
		public BusyAdorner( UIElement adornedElement, Object userContent )
			: base( adornedElement, userContent )
		{
			
		}
	}
}
