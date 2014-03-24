using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Topics.Radical.Windows.Controls;

namespace Topics.Radical.Windows.Behaviors
{
	sealed class BusyAdorner : OverlayAdorner
	{
		private readonly ContentPresenter userContentPresenter;
		
		public BusyAdorner( UIElement adornedElement, Object userContent )
			: base( adornedElement )
		{
			this.userContentPresenter = BusyStatusManager.WrapUserContent( userContent );
			this.AddVisualChild( this.userContentPresenter );
		}

		protected override UIElement Content
		{
			get { return this.userContentPresenter; }
		}

		protected override void OnRender( DrawingContext drawingContext )
		{
			var brush = new SolidColorBrush( Color.FromArgb( 100, 220, 220, 220 ) );
			var rect = new Rect( new Point( 0, 0 ), this.DesiredSize );

			drawingContext.DrawRectangle( brush, null, rect );

			base.OnRender( drawingContext );
		}
	}
}
