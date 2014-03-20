using System;
using System.Windows;
using System.Windows.Data;
using Topics.Radical.Conversions;
using System.Windows.Markup;

namespace Topics.Radical.Windows.Converters
{
	[MarkupExtensionReturnType( typeof( BooleanToVisibilityConverter ) )]
	public class NullToVisibilityConverter : IValueConverter
	{
		public NullToVisibilityConverter()
		{
			this.NullValue = Visibility.Collapsed;
		}

		public Visibility NullValue
		{
			get;
			set;
		}

		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			Boolean inverted;
			if( !Boolean.TryParse( parameter.As<String>(), out inverted ) )
			{
				inverted = false;
			}

			//bool flag = false;
			//if( value is bool )
			//{
			//    flag = ( bool )value;
			//}
			//else if( value is bool? )
			//{
			//    var nullable = ( bool? )value;
			//    flag = nullable.HasValue ? nullable.Value : false;
			//}

			if( inverted )
			{
				return ( value == null ? Visibility.Visible : this.NullValue );
			}
			else
			{
				return ( value == null ? this.NullValue : Visibility.Visible );
			}
		}

		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}