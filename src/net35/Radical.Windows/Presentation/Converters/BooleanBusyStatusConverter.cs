using System;
using System.Windows.Data;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Behaviors;
using System.Windows.Markup;

namespace Topics.Radical.Windows.Converters
{
	[MarkupExtensionReturnType( typeof( BooleanBusyStatusConverter ) )]
	public sealed class BooleanBusyStatusConverter : AbstractSingletonConverter
	{
        //static WeakReference singleton = new WeakReference( null );

        ///// <summary>
        ///// When implemented in a derived class, returns an object that is set as the value of the target property for this markup extension.
        ///// </summary>
        ///// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        ///// <returns>
        ///// The object value to set on the property where the extension is applied.
        ///// </returns>
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
			Ensure.That( value ).IsNotNull().IsTrue( o => o is Boolean );
			Ensure.That( targetType ).IsNotNull().Is( typeof( BusyStatus ) );

			var actual = ( Boolean )value;
			var status = actual ? BusyStatus.Busy : BusyStatus.Idle;

			return status;
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
			Ensure.That( value ).IsNotNull().IsTrue( o => o is BusyStatus );
			Ensure.That( targetType ).IsNotNull().Is( typeof( Boolean ) );

			var status = ( BusyStatus )value;
			switch( status )
			{
				case BusyStatus.Idle:
					return false;

				case BusyStatus.Busy:
					return true;
			}

			throw new InvalidOperationException();
		}
	}
}
