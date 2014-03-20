using System;
using System.Windows;
using System.Windows.Documents;
using System.Diagnostics;

namespace Topics.Radical.Windows.Behaviors
{
	public static class Overlay
	{
		public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
									  "Content",
									  typeof( Object ),
									  typeof( Overlay ),
									  new FrameworkPropertyMetadata( null, OnContentPropertyChanged ) );


		public static Object GetContent( UIElement control )
		{
			return control.GetValue( ContentProperty );
		}

		public static void SetContent( UIElement control, Object value )
		{
			control.SetValue( ContentProperty, value );
		}

        //#region Attached Property: Visibility

        //public static readonly DependencyProperty VisibilityProperty = DependencyProperty.RegisterAttached(
        //                              "Visibility",
        //                              typeof( Visibility ),
        //                              typeof( Overlay ),
        //                              new FrameworkPropertyMetadata( Visibility.Visible, OnVisibilityChanged ) );


        //public static Visibility GetVisibility( DependencyObject owner )
        //{
        //    return ( Visibility )owner.GetValue( VisibilityProperty );
        //}

        //public static void SetVisibility( DependencyObject owner, Visibility value )
        //{
        //    owner.SetValue( VisibilityProperty, value );
        //}

        //#endregion

        //private static void OnVisibilityChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        //{
            
        //}

		static readonly DependencyProperty OverlayAdornerProperty = DependencyProperty.RegisterAttached(
									  "OverlayAdorner",
									  typeof( ContentOverlayAdorner ),
									  typeof( Overlay ),
									  new FrameworkPropertyMetadata( null, OnContentPropertyChanged ) );


		static ContentOverlayAdorner GetOverlayAdorner( UIElement control )
		{
			return ( ContentOverlayAdorner )control.GetValue( OverlayAdornerProperty );
		}

		static void SetOverlayAdorner( UIElement control, ContentOverlayAdorner value )
		{
			control.SetValue( OverlayAdornerProperty, value );
		}

		static void OnContentPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs args )
		{
			if( !DesignTimeHelper.GetIsInDesignMode() )
			{
				var control = ( FrameworkElement )d;
				if( !control.IsLoaded )
				{
					control.Loaded += ( s, e ) => 
					{
						var layer = AdornerLayer.GetAdornerLayer( control );
						Debug.WriteLineIf( layer == null, "Overlay: cannot find any AdornerLayer on the given element." );

						if( layer != null )
						{
							var content = GetContent( control );
							if( content != null )
							{
								var a = new ContentOverlayAdorner( control, content );
								layer.Add( a );

								SetOverlayAdorner( control, a );
							}
						}
					};

					control.Unloaded += ( s, e ) => 
					{
						var layer = AdornerLayer.GetAdornerLayer( control );
						Debug.WriteLineIf( layer == null, "Overlay: cannot find any AdornerLayer on the given element." );

						if( layer != null )
						{
							var a = GetOverlayAdorner( control );
							if( a != null )
							{
								layer.Remove( a );

								SetOverlayAdorner( control, null );
							}
						}
					};
				}
			}
		}
	}
}
