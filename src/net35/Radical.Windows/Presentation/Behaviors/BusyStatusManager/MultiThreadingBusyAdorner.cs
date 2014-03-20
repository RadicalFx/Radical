using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Topics.Radical.Windows.Controls;

namespace Topics.Radical.Windows.Behaviors
{
	sealed class MultiThreadingBusyAdorner : OverlayAdorner
	{
		readonly BackgroundVisualHost _busyHost = null;
		readonly ContentPresenter userContentPresenter;

		MemoryStream ms = null;

		public MultiThreadingBusyAdorner( UIElement adornedElement, Object userContent )
			: base( adornedElement )
		{
			this.userContentPresenter = BusyStatusManager.WrapUserContent( userContent );

			this._busyHost = new BackgroundVisualHost( () =>
			{
				var s = ( ContentPresenter )XamlReader.Load( ms );

				this.ms.Dispose();
				this.ms = null;

				return s;
			} );

			this.AddLogicalChild( this._busyHost );
			this.AddVisualChild( this._busyHost );
		}

		protected override UIElement Content
		{
			get { return this._busyHost; }
		}

		protected override void OnRender( DrawingContext drawingContext )
		{
			var brush = new SolidColorBrush( Color.FromArgb( 100, 220, 220, 220 ) );
			var rect = new Rect( new Point( 0, 0 ), this.DesiredSize );

			drawingContext.DrawRectangle( brush, null, rect );

			base.OnRender( drawingContext );
		}

		internal void Setup()
		{
			this.ms = new MemoryStream();
			XamlWriter.Save( this.userContentPresenter, ms );
			ms.Flush();
			ms.Position = 0;

			this._busyHost.Setup();
		}

		internal void Teardown()
		{
			this._busyHost.Teardown();

			//this.ms.Dispose();
			//this.ms = null;
		}
	}
}
