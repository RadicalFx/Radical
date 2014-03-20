using System;
using System.Windows;
using System.Windows.Controls;
using Topics.Radical.Validation;
using System.Windows.Input;
using System.Windows.Media;

namespace Topics.Radical.Windows.Behaviors
{
	public static class GridViewColumnManager
	{
		#region Attached Property: Fill

		public static readonly DependencyProperty FillProperty = DependencyProperty.RegisterAttached(
									  "Fill",
									  typeof( Boolean ),
									  typeof( GridViewColumnManager ),
									  new FrameworkPropertyMetadata( false ) );

		public static Boolean GetFill( GridViewColumn owner )
		{
			return ( Boolean )owner.GetValue( FillProperty );
		}

		public static void SetFill( GridViewColumn owner, Boolean value )
		{
			owner.SetValue( FillProperty, value );
		}

		#endregion

		#region Attached Property: SortProperty

		public static readonly DependencyProperty SortPropertyProperty = DependencyProperty.RegisterAttached(
									  "SortProperty",
									  typeof( String ),
									  typeof( GridViewColumnManager ),
									  new FrameworkPropertyMetadata( null ) );

		public static String GetSortProperty( GridViewColumn owner )
		{
			return ( String )owner.GetValue( SortPropertyProperty );
		}

		public static void SetSortProperty( GridViewColumn owner, String value )
		{
			owner.SetValue( SortPropertyProperty, value );
		}

		#endregion
	}
}
