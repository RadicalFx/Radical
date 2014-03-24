//using System;
//using System.Windows;
//using System.Windows.Input;
//using Microsoft.Phone.Shell;

//namespace Topics.Radical.Windows.Controls
//{
//	/// <summary>
//	/// An element for the BindableApplicationBar.
//	/// </summary>
//	public abstract class BindableApplicationBarElement : FrameworkElement
//	{
//		/// <summary>
//		/// The currently wrapped bar menu item.
//		/// </summary>
//		protected readonly IApplicationBarMenuItem wrappedItem;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="BindableApplicationBarElement"/> class.
//		/// </summary>
//		/// <param name="wrappedItem">The wrapped item.</param>
//		protected BindableApplicationBarElement( IApplicationBarMenuItem wrappedItem )
//		{
//			this.wrappedItem = wrappedItem;
//			this.wrappedItem.Click += ( s, e ) =>
//			{
//				this.OnClick( e );

//				if( this.Command != null && this.Command.CanExecute( this.CommandParameter ) )
//				{
//					this.Command.Execute( this.CommandParameter );
//				}
//			};
//		}

//		/// <summary>
//		/// The Command property.
//		/// </summary>
//		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register
//		(
//			"Command",
//			typeof( ICommand ),
//			typeof( BindableApplicationBarElement ),
//			new PropertyMetadata( null, OnCommandChanged )
//		);

//		private static void OnCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
//		{
//			/*
//			 * Attenzione che generiamo un leak se il Command
//			 * viene cambiato a "runtime", l'ideale sarebbe tenere
//			 * una reference all'handler in una dependency property
//			 * privata al fine di poter sganciare l'handler dal 
//			 * command "uscente"
//			 */
//			var item = ( BindableApplicationBarElement )d;
//			var cmd = item.Command;
//			if( cmd != null )
//			{
//				cmd.CanExecuteChanged += ( s, args ) =>
//				{
//					var state = cmd.CanExecute( item.CommandParameter );
//					item.IsEnabled = state;
//				};
//			}
//		}

//		/// <summary>
//		/// Gets or sets the command.
//		/// </summary>
//		/// <value>
//		/// The command.
//		/// </value>
//		public ICommand Command
//		{
//			get { return ( ICommand )GetValue( CommandProperty ); }
//			set { SetValue( CommandProperty, value ); }
//		}

//		/// <summary>
//		/// The CommandParameter property.
//		/// </summary>
//		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register
//		(
//			"CommandParameter",
//			typeof( object ),
//			typeof( BindableApplicationBarElement ),
//			null
//		);

//		/// <summary>
//		/// Gets or sets the command parameter.
//		/// </summary>
//		/// <value>
//		/// The command parameter.
//		/// </value>
//		public object CommandParameter
//		{
//			get { return GetValue( CommandParameterProperty ); }
//			set { SetValue( CommandParameterProperty, value ); }
//		}

//		/// <summary>
//		/// The IsEnabled property.
//		/// </summary>
//		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register
//		(
//			"IsEnabled",
//			typeof( bool ),
//			typeof( BindableApplicationBarElement ),
//			new PropertyMetadata( true, OnEnabledChanged )
//		);

//		private static void OnEnabledChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
//		{
//			( ( BindableApplicationBarElement )d ).wrappedItem.IsEnabled = ( Boolean )e.NewValue;
//		}

//		/// <summary>
//		/// Gets or sets the enabled status of the menu item.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.Boolean"/>.</returns>
//		public bool IsEnabled
//		{
//			get { return ( bool )GetValue( IsEnabledProperty ); }
//			set { SetValue( IsEnabledProperty, value ); }
//		}

//		/// <summary>
//		/// The Text property.
//		/// </summary>
//		public static readonly DependencyProperty TextProperty = DependencyProperty.Register
//		(
//			"Text",
//			typeof( string ),
//			typeof( BindableApplicationBarElement ),
//			new PropertyMetadata( OnTextChanged )
//		);

//		private static void OnTextChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
//		{
//			( ( BindableApplicationBarElement )d ).wrappedItem.Text = e.NewValue.ToString();
//		}

//		/// <summary>
//		/// The string to display on the menu item.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.String"/>.</returns>
//		public string Text
//		{
//			get { return ( string )GetValue( TextProperty ); }
//			set { SetValue( TextProperty, value ); }
//		}

//		/// <summary>
//		/// Occurs when the user taps on the menu item.
//		/// </summary>
//		public event EventHandler Click;

//		/// <summary>
//		/// Raises the <see cref="E:Click"/> event.
//		/// </summary>
//		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
//		protected virtual void OnClick( EventArgs e )
//		{
//			var h = this.Click;
//			if( h != null )
//			{
//				h( this, e );
//			}
//		}
//	}
//}
