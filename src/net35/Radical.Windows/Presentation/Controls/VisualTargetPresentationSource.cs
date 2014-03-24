using System;
using System.Windows;
using System.Windows.Media;

namespace Topics.Radical.Windows.Controls
{
	public class VisualTargetPresentationSource : PresentationSource
	{
		private VisualTarget _visualTarget;
		private bool _isDisposed = false;

		public VisualTargetPresentationSource(HostVisual hostVisual)
		{
			_visualTarget = new VisualTarget(hostVisual);
			this.AddSource();
		}

		public Size DesiredSize { get; private set; }

		public override Visual RootVisual
		{
			get { return this._visualTarget.RootVisual; }
			set
			{
				Visual oldRoot = _visualTarget.RootVisual;

				// Set the root visual of the VisualTarget.  This visual will
				// now be used to visually compose the scene.
				this._visualTarget.RootVisual = value;

				// Tell the PresentationSource that the root visual has
				// changed.  This kicks off a bunch of stuff like the
				// Loaded event.
				this.RootChanged( oldRoot, value );

				// Kickoff layout...
				UIElement rootElement = value as UIElement;
				if ( rootElement != null )
				{
					rootElement.Measure( new Size( Double.PositiveInfinity, Double.PositiveInfinity ) );
					rootElement.Arrange( new Rect( rootElement.DesiredSize ) );

					this.DesiredSize = rootElement.DesiredSize;
				}
				else
				{
					this.DesiredSize = new Size( 0, 0 );
				}
			}
		}

		protected override CompositionTarget GetCompositionTargetCore()
		{
			return this._visualTarget;
		}

		public override bool IsDisposed
		{
			get { return this._isDisposed; }
		}

		internal void Dispose()
		{
			this.RemoveSource();
			this._isDisposed = true;
		}
	}
}
