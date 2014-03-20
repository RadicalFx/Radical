using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace Topics.Radical.Windows.Converters
{
	public class BooleanConverter : IValueConverter
	{
		public Object TrueValue { get; set; }

		public Object FalseValue { get; set; }

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			bool flag = false;
			if( value is bool )
			{
				flag = (bool)value;
			}
			else if( value is Fact )
			{
				flag = ( Fact )value;
			}
			else if( value is bool? )
			{
				var nullable = ( bool? )value;
				flag = nullable.HasValue ? nullable.Value : false;
			}

			return flag ? this.TrueValue : this.FalseValue;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if( Object.Equals( value, this.TrueValue ) )
			{
				return true;
			}
			else if( Object.Equals( value, this.FalseValue ) )
			{
				return false;
			}
			else
			{
				return value;
			}
		}
	}
}
