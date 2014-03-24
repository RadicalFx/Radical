using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Input;

namespace Topics.Radical.Windows.Behaviors
{
	public class TextBoxCommandBehavior : RadicalBehavior<TextBox>, ICommandSource
	{
		#region Dependency Property: Command

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
			"Command",
			typeof( ICommand ),
			typeof( TextBoxCommandBehavior ),
			new PropertyMetadata( null ) );

		public ICommand Command
		{
			get { return ( ICommand )this.GetValue( CommandProperty ); }
			set { this.SetValue( CommandProperty, value ); }
		}

		#endregion

		public object CommandParameter
		{
			get;
			set;
		}

		public IInputElement CommandTarget
		{
			get { return this.AssociatedObject; }
		}

		KeyEventHandler onPreviewKeyDown;

		public TextBoxCommandBehavior()
		{
			onPreviewKeyDown = ( s, e ) =>
			{
				var d = ( DependencyObject )s;

				var cmd = this.Command;
				var prm = this.CommandParameter;

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
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			this.AssociatedObject.PreviewKeyDown += onPreviewKeyDown;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			this.AssociatedObject.PreviewKeyDown -= onPreviewKeyDown;
		}
	}
}
