using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Collections;
using Topics.Radical.Validation;
using System.Reflection;
using System.Windows.Markup;
using Topics.Radical.Reflection;

namespace Topics.Radical.Windows.Converters
{
	/// <summary>
	/// Given a source list returns the First element of the list or null if
	/// the list is empty.
	/// </summary>
	[MarkupExtensionReturnType( typeof( LinqFirstOrDefaultConverter ) )]
	public class LinqFirstOrDefaultConverter : AbstractSingletonConverter
	{
        //static WeakReference singleton = new WeakReference( null );

        //public override object ProvideValue( IServiceProvider serviceProvider )
        //{
        //    IValueConverter converter = ( IValueConverter )singleton.Target;
        //    if( converter == null )
        //    {
        //        converter = this;
        //        singleton.Target = converter;
        //    }

        //    return converter;
        //}

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public override object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			var list = Ensure.That( value )
					.WithMessage( "Supplied value is null." )
					.IsNotNull()
					.WithMessage( "Supplied value is not an IEnumerable type." )
					.IsTrue( v => v.GetType().Is<IEnumerable>() )
					.GetValue<IEnumerable>();

			return list.OfType<Object>().FirstOrDefault();
		}

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public override object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
            throw new NotImplementedException();
		}
	}
}