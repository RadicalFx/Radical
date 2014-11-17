﻿using System;
using System.Windows.Data;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Behaviors;
using System.Windows.Markup;

namespace Topics.Radical.Windows.Converters
{
	/// <summary>
	/// A value converter to convert from bool to BusyStatus and back.
	/// </summary>
	public class BooleanBusyStatusConverter : IValueConverter
	{
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
			Ensure.That( value ).IsNotNull().IsTrue( o => o is Boolean );
			Ensure.That( targetType ).IsNotNull().Is( typeof( BusyStatus ) );

			var actual = ( Boolean )value;
			var status = actual ? BusyStatus.Busy : BusyStatus.Idle;

			return status;
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
