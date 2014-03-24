using System;
using System.Windows;
using System.Windows.Input;
using prv = System.Windows.Controls.Primitives;

namespace Topics.Radical.Windows.Behaviors
{
	/// <summary>
	/// Defines attached commands for the ButtonBase class.
	/// </summary>
	public static class ButtonBase
	{
		/// <summary>
		/// The Command property.
		/// </summary>
		public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
									  "Command",
									  typeof( ICommand ),
									  typeof( ButtonBase ),
									  new PropertyMetadata( null, OnCommandChanged ) );


		/// <summary>
		/// Gets the command.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <returns>The current command.</returns>
		public static ICommand GetCommand( prv.ButtonBase owner )
		{
			return ( ICommand )owner.GetValue( CommandProperty );
		}

		/// <summary>
		/// Sets the command.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="value">The value.</param>
		public static void SetCommand( prv.ButtonBase owner, ICommand value )
		{
			owner.SetValue( CommandProperty, value );
		}

		/// <summary>
		/// The CommandParameter property.
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
									  "CommandParameter",
									  typeof( Object ),
									  typeof( ButtonBase ),
									  new PropertyMetadata( null ) );


		/// <summary>
		/// Gets the command parameter.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <returns>The parameter value.</returns>
		public static Object GetCommandParameter( prv.ButtonBase owner )
		{
			return owner.GetValue( CommandParameterProperty );
		}

		/// <summary>
		/// Sets the command parameter.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="value">The value.</param>
		public static void SetCommandParameter( prv.ButtonBase owner, Object value )
		{
			owner.SetValue( CommandParameterProperty, value );
		}

		private static void OnCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var button = d as prv.ButtonBase;
			if( button != null )
			{
				button.Click += ( s, args ) => 
				{
					var prm = ButtonBase.GetCommandParameter( button );
					var cmd = ButtonBase.GetCommand( button );
					if( cmd != null )
					{
						cmd.Execute( prm );
					}
				};
			}
		}

		
	}
}
