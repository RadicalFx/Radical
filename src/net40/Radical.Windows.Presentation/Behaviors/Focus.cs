using System;
using System.Windows;
using System.Windows.Input;
using Topics.Radical.Windows.Behaviors;

namespace Topics.Radical.Windows.Presentation.Behaviors
{
	/// <summary>
	/// 
	/// </summary>
	public class Focus : RadicalBehavior<FrameworkElement>
	{
		#region Dependency Property: ControlledBy

		/// <summary>
		/// The controlled by property
		/// </summary>
		public static readonly DependencyProperty ControlledByProperty = DependencyProperty.Register(
			"ControlledBy",
			typeof( String ),
			typeof( Focus ),
			new FrameworkPropertyMetadata( null, new PropertyChangedCallback( OnControlledByChanged ) ) { BindsTwoWayByDefault = true } );

		/// <summary>
		/// Gets or sets the controlled by.
		/// </summary>
		/// <value>
		/// The controlled by.
		/// </value>
		public String ControlledBy
		{
			get { return ( String )this.GetValue( ControlledByProperty ); }
			set { this.SetValue( ControlledByProperty, value ); }
		}

		static void OnControlledByChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
		{
			( ( Focus )sender ).OnFocusChanged( ( String )e.NewValue );
		}

		private void OnFocusChanged( String focusKey )
		{
			if ( String.Equals( this.UsingKey, focusKey ) && this.AssociatedObject.Focusable && !this.AssociatedObject.IsFocused )
			{
				this.AssociatedObject.Focus();
				Keyboard.Focus( this.AssociatedObject );
			}
		}

		#endregion

		/// <summary>
		/// Called when [attached].
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.GotFocus += OnGotFocus;
			this.AssociatedObject.LostFocus += OnLostFocus;
		}

		/// <summary>
		/// Called when [detaching].
		/// </summary>
		protected override void OnDetaching()
		{
			this.AssociatedObject.GotFocus -= OnGotFocus;
			this.AssociatedObject.LostFocus -= OnLostFocus;

			base.OnDetaching();
		}

		void OnGotFocus( object sender, RoutedEventArgs e )
		{
			this.ControlledBy = this.UsingKey;
		}

		void OnLostFocus( object sender, RoutedEventArgs e )
		{
			if ( this.ControlledBy == this.UsingKey ) 
			{
				/*
				 * if, when we loose focus, the FocusedElementKey is still
				 * pointing to us means that the user has moved the focus 
				 * to an element not managed by a behvior like this one, thus
				 * we set the FocusedElementKey to null so to detach and 
				 * correctly react if the focus gets back.
				 */
				this.ControlledBy = null;
			}
		}

		/// <summary>
		/// Gets or sets the using key.
		/// </summary>
		/// <value>
		/// The using key.
		/// </value>
		public String UsingKey { get; set; }
	}
}
