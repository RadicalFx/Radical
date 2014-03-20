using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System;

namespace Topics.Radical.Windows.Controls
{
	/// <summary>
	/// A base class for adorner in a Silverlight environment.
	/// </summary>
	public abstract class Adorner : UserControl, IDisposable
	{
		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public virtual void Dispose()
		{
			this.AdornedElement = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Adorner"/> class.
		/// </summary>
		/// <param name="adornedElement">The adorned element.</param>
		protected Adorner( UIElement adornedElement )
		{
			this.IsHitTestVisible = false;

			this.AdornedElement = adornedElement;

			//this.RenderTransformOrigin = this.AdornedElement.RenderTransformOrigin;
			//if( this.AdornedElement.RenderTransform == null )
			//{
			//    this.AdornedElement.RenderTransform = new MatrixTransform();
			//}

			//this.SetBinding( Adorner.TransformProperty, new Binding( "RenderTransform" ) { Source = this.AdornedElement } );
			//this.SetBinding( FrameworkElement.WidthProperty, new Binding( "Width" ) { Source = this.AdornedElement } );
			//this.SetBinding( FrameworkElement.HeightProperty, new Binding( "Height" ) { Source = this.AdornedElement } );
			//this.SetBinding( FrameworkElement.MarginProperty, new Binding( "Margin" ) { Source = this.AdornedElement } );
			//this.SetBinding( FrameworkElement.HorizontalAlignmentProperty, new Binding( "HorizontalAlignment" ) { Source = this.AdornedElement } );
			//this.SetBinding( FrameworkElement.VerticalAlignmentProperty, new Binding( "VerticalAlignment" ) { Source = this.AdornedElement } );
		}

		#region Transform Dependency Property

		/// <summary>
		/// The Transform dependency property
		/// </summary>
		public static readonly DependencyProperty TransformProperty = DependencyProperty.Register(
			"Transform",
			typeof( Transform ),
			typeof( Adorner ),
			new PropertyMetadata( OnTransformChanged ) );

		/// <summary>
		/// Gets or sets the transform.
		/// </summary>
		/// <value>The transform.</value>
		public Transform Transform
		{
			get { return ( Transform )this.GetValue( TransformProperty ); }
			set { this.SetValue( TransformProperty, value ); }
		}

		static void OnTransformChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			( ( Adorner )d ).OnTransformChanged( e );
		}

		/// <summary>
		/// Occures when the Transform dependency prperty changes.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnTransformChanged( DependencyPropertyChangedEventArgs e )
		{
			this.RenderTransform = e.NewValue as Transform;
		}

		#endregion

		/// <summary>
		/// Gets the adorned element.
		/// </summary>
		/// <value>The adorned element.</value>
		protected internal UIElement AdornedElement { get; private set; }

		/// <summary>
		/// Provides the behavior for the Measure pass of Silverlight layout. Classes
		/// can override this method to define their own Measure pass behavior.
		/// </summary>
		/// <param name="constraint">The available size that this object can give to child objects. Infinity (System.Double.PositiveInfinity)
		/// can be specified as a value to indicate that the object will size to whatever
		/// content is available.</param>
		/// <returns>
		/// The size that this object determines it needs during layout, based on its
		/// calculations of the allocated sizes for child objects; or based on other
		/// considerations, such as a fixed container size.
		/// </returns>
		protected override Size MeasureOverride( Size constraint )
		{
			this.Content.Measure( this.AdornedElement.RenderSize );

			return this.AdornedElement.RenderSize;
		}

		/// <summary>
		/// Provides the behavior for the Arrange pass of Silverlight layout. Classes can override this method to define their own Arrange pass behavior.
		/// </summary>
		/// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
		/// <returns>
		/// The actual size that is used after the element is arranged in layout.
		/// </returns>
		protected override Size ArrangeOverride( Size finalSize )
		{
			var fs = this.AdornedElement.RenderSize;

			var generalTransform = this.AdornedElement.TransformToVisual( Application.Current.RootVisual );
			var elementToApplicationRootCoords = generalTransform.Transform( new Point( 0, 0 ) );

			this.Content.Arrange( new Rect( elementToApplicationRootCoords, fs ) );

			return finalSize;
		}
	}
}
