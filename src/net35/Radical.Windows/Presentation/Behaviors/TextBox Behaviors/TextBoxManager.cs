using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Topics.Radical.Windows.Input;
using Topics.Radical.Linq;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Behaviors
{
	public static class TextBoxManager
	{
		static KeyEventHandler onPreviewKeyDown;
		static RoutedEventHandler onGotFocus;
		static RoutedEventHandler onLoaded;
		static RoutedEventHandler onUnloaded;

		static TextBoxManager()
		{
			onPreviewKeyDown = ( s, e ) =>
			{
				var d = ( DependencyObject )s;

				var cmd = GetCommand( d );
				var prm = GetCommandParameter( d );

				if( cmd.CanExecute( prm ) )
				{
					var gestures = cmd.GetGestures();
					var senderGestures = gestures.Where( gesture => gesture.Matches( d, e ) );
					
					if( ( ( gestures.None() && e.Key == System.Windows.Input.Key.Enter ) || senderGestures.Any() ) )
					{
						var k = e.Key;
						var m = ModifierKeys.None;

						if( senderGestures.Any() )
						{
							var gesture = senderGestures.First();
							var keygesture = gesture as KeyGesture;
							if( keygesture != null )
							{
								k = keygesture.Key;
								m = keygesture.Modifiers;
							}
						}

						var args = new TextBoxCommandArgs( k, m, prm );
						cmd.Execute( args );
						e.Handled = true;
					}
				}
			};

			onLoaded = ( s, e ) =>
			{
				var textBox = ( TextBox )s;

				textBox.PreviewKeyDown += onPreviewKeyDown;
				textBox.Unloaded += onUnloaded;
			};

			onUnloaded = ( s, e ) =>
			{
				var textBox = ( TextBox )s;

				//Vedi il CueBannerService per i dettagli
				//textBox.Loaded -= onLoaded;

				textBox.Unloaded -= onUnloaded;
				textBox.PreviewKeyDown -= onPreviewKeyDown;
			};

			onGotFocus = ( s, e ) =>
			{
				var source = ( s as TextBox );
				source.SelectAll();
			};
		}

		#region Attached Property: Command

		public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
									  "Command",
									  typeof( ICommand ),
									  typeof( TextBoxManager ),
									  new FrameworkPropertyMetadata( null, OnCommandChanged ) );


		public static ICommand GetCommand( DependencyObject owner )
		{
			return ( ICommand )owner.GetValue( CommandProperty );
		}

		public static void SetCommand( DependencyObject owner, ICommand value )
		{
			owner.SetValue( CommandProperty, value );
		}

		#endregion

		#region Attached Property: CommandParameter

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
									  "CommandParameter",
									  typeof( Object ),
									  typeof( TextBoxManager ),
									  new FrameworkPropertyMetadata( null ) );


		public static Object GetCommandParameter( DependencyObject owner )
		{
			return ( Object )owner.GetValue( CommandParameterProperty );
		}

		public static void SetCommandParameter( DependencyObject owner, Object value )
		{
			owner.SetValue( CommandParameterProperty, value );
		}

		#endregion

		private static void OnCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			if( !DesignTimeHelper.GetIsInDesignMode() )
			{
				var textBox = d as TextBox;
				if( textBox != null )
				{
					textBox.Loaded += onLoaded;
				}
			}
		}

		#region Attached Property: AutoSelectText

		public static readonly DependencyProperty AutoSelectTextProperty = DependencyProperty.RegisterAttached(
									  "AutoSelectText",
									  typeof( Boolean ),
									  typeof( TextBoxManager ),
									  new FrameworkPropertyMetadata( false, OnAutoSelectTextChanged ) );


		public static Boolean GetAutoSelectText( TextBox owner )
		{
			return ( Boolean )owner.GetValue( AutoSelectTextProperty );
		}

		public static void SetAutoSelectText( TextBox owner, Boolean value )
		{
			owner.SetValue( AutoSelectTextProperty, value );
		}

		#endregion

		private static void OnAutoSelectTextChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var source = ( d as TextBox );
			if( source != null )
			{
				if( ( bool )e.NewValue )
				{
					source.GotFocus += onGotFocus;
				}
				else
				{
					source.GotFocus -= onGotFocus;
				}
			}
		}
	}
}