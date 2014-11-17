using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using Topics.Radical.Windows.Controls;
using System.Collections.Generic;
using Topics.Radical.Validation;
using Topics.Radical.Conversions;

namespace Topics.Radical.Windows.Behaviors
{
	/// <summary>
	/// A behavior to manage the busy status of the application.
	/// </summary>
	public static class BusyStatusManager
	{
		/// <summary>
		/// The content attached property.
		/// </summary>
		public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
			"Content",
			typeof( Object ),
			typeof( BusyStatusManager ),
			new PropertyMetadata( null, OnPropertyChanged ) );


		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <param name="control">The control.</param>
		/// <returns>The current content.</returns>
		public static Object GetContent( UIElement control )
		{
			return control.GetValue( ContentProperty );
		}

		/// <summary>
		/// Sets the content.
		/// </summary>
		/// <param name="control">The control.</param>
		/// <param name="value">The value.</param>
		public static void SetContent( UIElement control, Object value )
		{
			control.SetValue( ContentProperty, value );
		}

		/// <summary>
		/// The staus attached property.
		/// </summary>
		public static readonly DependencyProperty StatusProperty = DependencyProperty.RegisterAttached(
							  "Status",
							  typeof( BusyStatus ),
							  typeof( BusyStatusManager ),
							  new PropertyMetadata( BusyStatus.Idle, OnPropertyChanged ) );

		/// <summary>
		/// Gets the status.
		/// </summary>
		/// <param name="control">The control.</param>
		/// <returns>The current status.</returns>
		public static BusyStatus GetStatus( UIElement control )
		{
			return ( BusyStatus )control.GetValue( StatusProperty );
		}

		/// <summary>
		/// Sets the status.
		/// </summary>
		/// <param name="control">The control.</param>
		/// <param name="value">The value.</param>
		public static void SetStatus( UIElement control, BusyStatus value )
		{
			control.SetValue( StatusProperty, value );
		}

		static readonly DependencyProperty handledProperty = DependencyProperty.RegisterAttached(
			"handled",
			typeof( Boolean ),
			typeof( BusyStatusManager ),
			new PropertyMetadata( false ) );

		static Boolean Gethandled( DependencyObject control )
		{
			return ( Boolean )control.GetValue( handledProperty );
		}

		static void Sethandled( DependencyObject control, Boolean value )
		{
			control.SetValue( handledProperty, value );
		}

		static readonly DependencyProperty IsLoadedProperty = DependencyProperty.RegisterAttached(
									  "IsLoaded",
									  typeof( Boolean ),
									  typeof( BusyStatusManager ),
									  new PropertyMetadata( false ) );


		static Boolean GetIsLoaded( FrameworkElement owner )
		{
			return ( Boolean )owner.GetValue( IsLoadedProperty );
		}

		static void SetIsLoaded( FrameworkElement owner, Boolean value )
		{
			owner.SetValue( IsLoadedProperty, value );
		}

		static void HandleStatusChanged( FrameworkElement element )
		{
			AdornerLayer layer = AdornerLayer.GetAdornerLayer( element );

			Ensure.That( layer )
				.Named( () => layer )
				.WithMessage( "BusyStatusManager: cannot find any AdornerLayer on the given element." )
				.IsNotNull();

			var content = GetContent( element );
			var status = GetStatus( element );

			switch( status )
			{
				case BusyStatus.Idle:

					element.As<Control>( c => c.IsEnabled = true );

					if( content != null )
					{
						var adorners = layer.GetAdorners( element );

						if( adorners == null )
						{
							Debug.WriteLine( "BusyStatusManager: cannot find any Adorner on the given element." );
						}
						else
						{
							var la = adorners.OfType<BusyAdorner>().FirstOrDefault();
							if( la != null )
							{
								layer.Remove( la );
								la.Dispose();
							}
						}
					}

					break;

				case BusyStatus.Busy:

					element.As<Control>( c => c.IsEnabled = false );

					if( content != null )
					{
						var ba = new BusyAdorner( element, GetContent( element ) );
						layer.Add( ba );
					}
					break;

				default:
					throw new NotSupportedException();
			}
		}

		static void HandleContentChanged( FrameworkElement element )
		{
			throw new NotSupportedException( "BusyStatusManager: Content property cannot be changed at runtime." );
		}

		static void OnPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			if( !DesignTimeHelper.GetIsInDesignMode() )
			{
				var control = ( FrameworkElement )d;

				var isLoaded = BusyStatusManager.GetIsLoaded( control );

				if( !isLoaded && !BusyStatusManager.Gethandled( control ) )
				{
					Sethandled( control, true );
					control.Loaded += ( s, rea ) =>
					{
						BusyStatusManager.SetIsLoaded( control, true );
						BusyStatusManager.HandleStatusChanged( control );
					};
				}
				else if( isLoaded && e.Property == StatusProperty )
				{
					BusyStatusManager.HandleStatusChanged( control );
				}
				else if( isLoaded && e.Property == ContentProperty )
				{
					BusyStatusManager.HandleContentChanged( control );
				}
			}
		}
	}
}
