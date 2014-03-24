namespace Topics.Radical.Windows.Behaviors
{
	using System;
	using System.Windows.Documents;
	using System.Windows.Controls;
	using System.Windows;
	using System.Windows.Media;
	using Topics.Radical.Windows.Controls;

	sealed class ContentOverlayAdorner : OverlayAdorner
	{
		private readonly ContentPresenter userContent;

		public ContentOverlayAdorner( UIElement adornedElement, Object content ) :
			base( adornedElement )
		{
			this.IsHitTestVisible = true;
			this.userContent = new ContentPresenter();

			var cueBannerText = content as String;
			if( cueBannerText != null )
			{
				this.userContent.Content = new TextBlock()
				{
					FontStyle = FontStyles.Italic,
					Text = cueBannerText,
					VerticalAlignment = VerticalAlignment.Center,
					Margin = new Thickness( 6, 0, 0, 0 ),
					TextTrimming = TextTrimming.CharacterEllipsis,
					Opacity = 0.7
				};
			}
			else
			{
				this.userContent.Content = content;
			}

            //WARN: if this is in a template the...
			this.AddVisualChild( this.userContent );
		}

		protected override UIElement Content
		{
			get { return this.userContent; }
		}

        protected override void OnRender( DrawingContext drawingContext )
        {
            if ( this.Background != null )
            {
                var rect = new Rect( new Point( 0, 0 ), this.DesiredSize );

                drawingContext.DrawRectangle( this.Background, null, rect );
            }

            base.OnRender( drawingContext );
        }

        public Brush Background { get; set; }
    }
}
