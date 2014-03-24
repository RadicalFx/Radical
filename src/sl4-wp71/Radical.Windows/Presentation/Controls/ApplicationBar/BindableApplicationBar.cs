//using System;
//using System.Collections;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Markup;
//using System.Windows.Media;
//using Microsoft.Phone.Controls;
//using Microsoft.Phone.Shell;

//namespace Topics.Radical.Windows.Controls
//{
//	/// <summary>
//	/// A phone application bar with support for data binding.
//	/// </summary>
//	[ContentProperty( "Buttons" )]
//	public class BindableApplicationBar : ItemsControl, IApplicationBar
//	{
//		private readonly ApplicationBar wrappedApplicationBar;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="BindableApplicationBar"/> class.
//		/// </summary>
//		public BindableApplicationBar()
//		{
//			this.wrappedApplicationBar = new ApplicationBar();
//			this.wrappedApplicationBar.StateChanged += ( s, e ) =>
//			{
//				this.OnStateChanged( e );
//			};

//			this.Loaded += ( s, e ) =>
//			{
//				var page = this.FindParent<PhoneApplicationPage>();
//				if( page != null )
//				{
//					page.ApplicationBar = this.wrappedApplicationBar;
//				}
//			};
//		}

//		/// <summary>
//		/// Called when the value of the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> property changes.
//		/// </summary>
//		/// <param name="e">A <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> that contains the event data</param>
//		protected override void OnItemsChanged( NotifyCollectionChangedEventArgs e )
//		{
//			base.OnItemsChanged( e );

//			/*
//			 * questo potrebbe essere decisamente 
//			 * ottimizzato
//			 */

//			this.wrappedApplicationBar.Buttons.Clear();
//			this.wrappedApplicationBar.MenuItems.Clear();

//			foreach( var obj in this.Items.OfType<BindableApplicationBarIconButton>() )
//			{
//				this.wrappedApplicationBar.Buttons.Add( obj.GetButton() );
//			}

//			foreach( var obj in this.Items.OfType<BindableApplicationBarMenuItem>() )
//			{
//				this.wrappedApplicationBar.MenuItems.Add( obj.GetMenuItem() );
//			}
//		}

//		/// <summary>
//		/// The IsVisible property.
//		/// </summary>
//		public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register
//		(
//			"IsVisible",
//			typeof( bool ),
//			typeof( BindableApplicationBar ),
//			new PropertyMetadata( true, OnIsVisibleChanged )
//		);

//		private static void OnIsVisibleChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
//		{
//			( ( BindableApplicationBar )d ).wrappedApplicationBar.IsVisible = ( bool )e.NewValue;
//		}

//		/// <summary>
//		/// Gets or sets the visibility of the application bar.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.Boolean"/>.</returns>
//		public bool IsVisible
//		{
//			get { return ( bool )this.GetValue( IsVisibleProperty ); }
//			set { this.SetValue( IsVisibleProperty, value ); }
//		}

//		/// <summary>
//		/// The IsMenuEnabled property.
//		/// </summary>
//		public static readonly DependencyProperty IsMenuEnabledProperty = DependencyProperty.Register
//		(
//			"IsMenuEnabled",
//			typeof( bool ),
//			typeof( BindableApplicationBar ),
//			new PropertyMetadata( true, OnIsMenuEnabledChanged )
//		);

//		private static void OnIsMenuEnabledChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
//		{
//			( ( BindableApplicationBar )d ).wrappedApplicationBar.IsMenuEnabled = ( bool )e.NewValue;
//		}

//		/// <summary>
//		/// Gets or sets whether the user can open the menu.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.Boolean"/>.</returns>
//		public bool IsMenuEnabled
//		{
//			get { return ( bool )this.GetValue( IsMenuEnabledProperty ); }
//			set { this.SetValue( IsMenuEnabledProperty, value ); }
//		}

//		/// <summary>
//		/// Gets or sets the bar opacity.
//		/// </summary>
//		/// <value>The bar opacity.</value>
//		public double BarOpacity
//		{
//			get { return wrappedApplicationBar.Opacity; }
//			set { wrappedApplicationBar.Opacity = value; }
//		}

//		/// <summary>
//		/// Gets or sets the background color for the application bar.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.Windows.Media.Color"/>.</returns>
//		public Color BackgroundColor
//		{
//			get { return wrappedApplicationBar.BackgroundColor; }
//			set { wrappedApplicationBar.BackgroundColor = value; }
//		}

//		/// <summary>
//		/// Gets or sets the foreground color of the application bar.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.Windows.Media.Color"/>.</returns>
//		public Color ForegroundColor
//		{
//			get { return wrappedApplicationBar.ForegroundColor; }
//			set { wrappedApplicationBar.ForegroundColor = value; }
//		}

//		/// <summary>
//		/// The list that is used to populate the buttons on the application bar.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.Collections.IList"/>.</returns>
//		public IList Buttons
//		{
//			get
//			{
//				return this.Items;
//				//return this.Items.OfType<BindableApplicationBarIconButton>().ToList();
//			}
//		}

//		/// <summary>
//		/// The list of items shown in the application bar’s menu when it is opened.
//		/// </summary>
//		/// <value></value>
//		/// <returns>Type:
//		/// <see cref="T:System.Collections.IList"/>.</returns>
//		public IList MenuItems
//		{
//			get
//			{
//				return this.Items;
//				//return this.Items.OfType<BindableApplicationBarMenuItem>().ToList();
//			}
//		}

//		/// <summary>
//		/// Occurs when the user opens or closes the menu.
//		/// </summary>
//		public event EventHandler<ApplicationBarStateChangedEventArgs> StateChanged;

//		/// <summary>
//		/// Raises the <see cref="E:StateChanged"/> event.
//		/// </summary>
//		/// <param name="e">The <see cref="Microsoft.Phone.Shell.ApplicationBarStateChangedEventArgs"/> instance containing the event data.</param>
//		protected virtual void OnStateChanged( ApplicationBarStateChangedEventArgs e )
//		{
//			var h = this.StateChanged;
//			if( h != null )
//			{
//				h( this, e );
//			}
//		}

//#if WINDOWS_PHONE_71

//		/// <summary>
//		/// Gets the distance that the Application Bar extends into a page when the <see cref="P:Microsoft.Phone.Shell.IApplicationBar.Mode"/> property is set to <see cref="F:Microsoft.Phone.Shell.ApplicationBarMode.Default"/>.
//		/// </summary>
//		/// <returns>The distance that the Application Bar extends into a page.</returns>
//		public double DefaultSize
//		{
//			get { return 150; }
//		}

//		/// <summary>
//		/// Gets the distance that the Application Bar extends into a page when the <see cref="P:Microsoft.Phone.Shell.IApplicationBar.Mode"/> property is set to <see cref="F:Microsoft.Phone.Shell.ApplicationBarMode.Minimized"/>.
//		/// </summary>
//		/// <returns>The distance that the Application Bar extends into a page.</returns>
//		public double MiniSize
//		{
//			get { return 150; }
//		}

//		/// <summary>
//		/// Gets or sets the size of the Application Bar.
//		/// </summary>
//		/// <returns>One of the enumeration values that indicates the size of the Application Bar.</returns>
//		public ApplicationBarMode Mode
//		{
//			get;
//			set;
//		}

//#endif
//	}
//}
