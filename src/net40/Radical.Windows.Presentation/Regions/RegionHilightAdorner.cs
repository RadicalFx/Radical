using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Regions
{
	sealed class RegionHilightAdorner : Topics.Radical.Windows.Controls.OverlayAdorner
	{
		private readonly ContentPresenter userContent;

		readonly Brush brush;

		/// <summary>
		/// Initializes a new instance of the <see cref="RegionHilightAdorner"/> class.
		/// </summary>
		/// <param name="adornedElement">The adorned element.</param>
		/// <param name="region">The region.</param>
		/// <param name="brush">The brush.</param>
		public RegionHilightAdorner( UIElement adornedElement, IRegion region, Brush brush )
			: base( adornedElement )
		{
			this.IsHitTestVisible = false;

			this.brush = brush;

			this.userContent = new ContentPresenter();

			this.userContent.Content = new TextBlock()
			{
				FontStyle = FontStyles.Italic,
				Text = String.Format( "{0}, {1}", region.GetType().Name, region.Name ),
				VerticalAlignment = VerticalAlignment.Bottom,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness( 0, 0, 4, 4 ),
				TextTrimming = TextTrimming.CharacterEllipsis,
				TextWrapping = TextWrapping.Wrap,
				FontSize = 10,
				Foreground = this.brush
			};

			this.AddVisualChild( this.userContent );
		}

		/// <summary>
		/// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
		/// </summary>
		/// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
		protected override void OnRender( DrawingContext drawingContext )
		{
			var pen = new Pen( brush, 2 );
			var rect = new Rect( new Point( 0, 0 ), this.DesiredSize );

			drawingContext.DrawRectangle( null, pen, rect );

			base.OnRender( drawingContext );
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <value>The content.</value>
		protected override UIElement Content
		{
			get { return this.userContent; }
		}
	}
}
