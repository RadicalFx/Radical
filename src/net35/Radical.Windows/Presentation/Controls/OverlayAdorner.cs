namespace Topics.Radical.Windows.Controls
{
	using System.Windows.Documents;
	using System.Windows;
	using System.Windows.Media;

	public abstract class OverlayAdorner : Adorner
	{
		protected OverlayAdorner( UIElement adornedElement )
			: base( adornedElement )
		{

		}

		protected abstract UIElement Content
		{
			get;
		}

		protected override Visual GetVisualChild( int index )
		{
			return this.Content;
		}

		protected override int VisualChildrenCount
		{
			get { return 1; }
		}

		protected override Size MeasureOverride( Size constraint )
		{
			this.Content.Measure( this.AdornedElement.RenderSize );

			return this.AdornedElement.RenderSize;
		}

		protected override Size ArrangeOverride( Size finalSize )
		{
			this.Content.Arrange( new Rect( finalSize ) );

			return finalSize;
		}
	}
}