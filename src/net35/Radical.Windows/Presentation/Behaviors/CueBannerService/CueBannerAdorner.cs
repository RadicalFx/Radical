namespace Topics.Radical.Windows.Behaviors
{
	using System;
	using System.Windows.Documents;
	using System.Windows.Controls;
	using System.Windows;
	using System.Windows.Media;
	using Topics.Radical.Windows.Controls;

	sealed class CueBannerAdorner : OverlayAdorner
	{
		private readonly ContentPresenter userContent;

		public CueBannerAdorner( UIElement adornedElement, Object content ) :
			base( adornedElement )
		{
			this.IsHitTestVisible = false;
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
			
			this.AddVisualChild( this.userContent );
		}

		protected override UIElement Content
		{
			get { return this.userContent; }
		}
	}
}
