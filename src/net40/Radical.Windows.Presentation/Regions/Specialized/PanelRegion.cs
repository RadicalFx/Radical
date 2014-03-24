using System;
using System.Windows;
using System.Windows.Controls;
using Topics.Radical.Conversions;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Regions
{
	/// <summary>
	/// An elements region hosted by a Panel.
	/// </summary>
	public sealed class PanelRegion : ElementsRegion<Panel>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PanelRegion"/> class.
		/// </summary>
		public PanelRegion()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelRegion"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public PanelRegion( String name )
		{
			this.Name = name;
		}

		/// <summary>
		/// Called after the add operation.
		/// </summary>
		/// <param name="view">The view.</param>
		protected override void OnAdd( DependencyObject view )
		{
			this.Element.Children.Add( ( UIElement )view );
		}

		/// <summary>
		/// Called before the remove operation.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="reason">The reason.</param>
		protected override void OnRemove( DependencyObject view, RemoveReason reason )
		{
			view.As<UIElement>(e=>
			{
				if( this.Element.Children.Contains( e ) )
				{
					this.Element.Children.Remove( e );
				}
			});
		}
	}
}
