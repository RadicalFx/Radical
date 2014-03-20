using System;
using System.Windows;
using System.Windows.Data;
using Topics.Radical.Conversions;
using System.Windows.Markup;

namespace Topics.Radical.Windows.Converters
{
#if !SILVERLIGHT
	[MarkupExtensionReturnType( typeof( BooleanToVisibilityConverter ) )]
#endif
	/// <summary>
	/// A value converter to convert from bool to Visibility and back.
	/// </summary>
	public class BooleanToVisibilityConverter : IValueConverter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BooleanToVisibilityConverter"/> class.
		/// </summary>
		public BooleanToVisibilityConverter()
		{
			this.FalseValue = Visibility.Collapsed;
			this.TrueValue = Visibility.Visible;
		}

		/// <summary>
		/// Gets or sets the false value.
		/// </summary>
		/// <value>
		/// The false value.
		/// </value>
		public Visibility FalseValue
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the true value.
		/// </summary>
		/// <value>
		/// The true value.
		/// </value>
		public Visibility TrueValue
		{
			get;
			set;
		}

		/// <summary>
		/// Modifies the source data before passing it to the target for display in the UI.
		/// </summary>
		/// <param name="value">The source data being passed to the target.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		/// <returns>
		/// The value to be passed to the target dependency property.
		/// </returns>
		public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			Boolean inverted;
			if( !Boolean.TryParse( parameter.As<String>(), out inverted ) )
			{
				inverted = false;
			}

			bool flag = false;
			if( value is bool )
			{
				flag = ( bool )value;
			}
			else if( value is bool? )
			{
				var nullable = ( bool? )value;
				flag = nullable.HasValue ? nullable.Value : false;
			}

			if( inverted )
			{
				return ( flag ? this.FalseValue : this.TrueValue );
			}
			else
			{
				return ( flag ? this.TrueValue : this.FalseValue );
			}
		}

		/// <summary>
		/// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
		/// </summary>
		/// <param name="value">The target data being passed to the source.</param>
		/// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
		/// <param name="parameter">An optional parameter to be used in the converter logic.</param>
		/// <param name="culture">The culture of the conversion.</param>
		/// <returns>
		/// The value to be passed to the source object.
		/// </returns>
		public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			return ( ( value is Visibility ) && ( ( ( Visibility )value ) == Visibility.Visible ) );
		}
	}
}