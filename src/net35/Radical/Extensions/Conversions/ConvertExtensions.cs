namespace Topics.Radical.Conversions
{
    using System;

    /// <summary>
    /// Add behaviors to convert from one type to another.
    /// </summary>
    public static class ConvertExtensions
    {
        /// <summary>
        /// Returns the source value as a generic observable object.
        /// </summary>
        /// <typeparam name="T">The type of the source value.</typeparam>
        /// <param name="value">The source value.</param>
        /// <returns>The observable instance that wraps the source value.</returns>
        public static Observable<T> AsObservable<T>( this T value )
        {
            return new Observable<T>( value );
        }

        /// <summary>
        /// Convertrs the source string value to a boolean.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>A boolean rapresentation of the source string.</returns>
        [Obsolete]
        public static Int32 ToInt32( this String value )
        {
            return Convert.ToInt32( value );
        }

        /// <summary>
        /// Converts the source string value to a boolean.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>A boolean rapresentation of the source string.</returns>
        [Obsolete]
        public static Boolean ToBoolean( this String value )
        {
            return Convert.ToBoolean( value );
        }

        /// <summary>
        /// Converts the source string value to a decimal.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>A decimal rapresentation of the source string.</returns>
        [Obsolete]
        public static Decimal ToDecimal( this String value )
        {
            return Convert.ToDecimal( value );
        }

        /// <summary>
        /// Casts the source double value to a decimal.
        /// </summary>
        /// <param name="value">The source value.</param>
        /// <returns>A decimal rapresentation of the source double.</returns>
        [Obsolete]
        public static Decimal ToDecimal( this Double value )
        {
            return ( Decimal )value;
        }
    }
}
