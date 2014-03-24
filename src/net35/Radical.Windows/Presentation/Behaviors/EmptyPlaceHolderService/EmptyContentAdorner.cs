using System;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Topics.Radical.Windows.Controls;

namespace Topics.Radical.Windows.Behaviors
{
	class EmptyContentAdorner : OverlayAdorner
	{
		private readonly ContentPresenter userContent;

		public EmptyContentAdorner( UIElement adornedElement, Object content ) :
			base( adornedElement )
		{
			this.IsHitTestVisible = false;
			this.userContent = new ContentPresenter();

			var emptyText = content as String;
			if( emptyText != null )
			{
				this.userContent.Content = new TextBlock()
				{
					FontStyle = FontStyles.Italic,
					Text = emptyText,
					VerticalAlignment = VerticalAlignment.Top,
					HorizontalAlignment = HorizontalAlignment.Center,
					Margin = new Thickness( 0, 25, 0, 0 ),
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
