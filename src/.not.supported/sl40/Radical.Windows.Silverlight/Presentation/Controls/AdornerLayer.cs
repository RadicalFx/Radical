using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Topics.Radical.Windows.Controls
{
	/// <summary>
	/// Defines an adorner layer for Silverlight and Windows Phone.
	/// </summary>
	public class AdornerLayer : Grid
	{
		/// <summary>
		/// Gets the adorner layer.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		public static AdornerLayer GetAdornerLayer( UIElement element )
		{
			return element.FindParent<AdornerLayer>();
		}

		/// <summary>
		/// Gets the adorners for the given element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns></returns>
		public IEnumerable<Adorner> GetAdorners( UIElement element )
		{
			return this.Children
				.OfType<Adorner>()
				.Where( a => a.AdornedElement == element );
		}

		/// <summary>
		/// Removes the specified adorner.
		/// </summary>
		/// <param name="adorner">The adorner.</param>
		public void Remove( Adorner adorner )
		{
			this.Children.Remove( adorner );
		}

		/// <summary>
		/// Adds the specified adorner.
		/// </summary>
		/// <param name="adorner">The adorner.</param>
		public void Add( Adorner adorner )
		{
			this.Children.Add( adorner );
		}
	}
}
