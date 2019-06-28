namespace Radical
{
    using System;

    /// <summary>
    /// Adds behaviors to the Generic <c>Nullable</c> class.
    /// </summary>
    public static class NullableExtensions
    {
        /// <summary>
        /// If the input value is null returns the supplied default value.
        /// </summary>
        /// <typeparam name="T">The type of the Nullable type.</typeparam>
        /// <param name="value">The value to test.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The input value, if not null, otherwise the supplied default value.
        /// </returns>
        public static T ValueOr<T>( this Nullable<T> value, T defaultValue ) where T : struct
        {
            return ValueOr( value, defaultValue, null );
        }

        /// <summary>
        /// If the input value is null returns the supplied default value.
        /// </summary>
        /// <typeparam name="T">The type of the Nullable type.</typeparam>
        /// <param name="value">The value to test.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="ifValue">A delegate that will be invoked, if the supplied nullable 
        /// value has a value, in order to give the caller an opportunity to customize the return value;
        /// if this delegate is null the value of the nullable type is returned, otherwise is returned
        /// the return value of the supplied delegate.</param>
        /// <returns>
        /// The input value, if not null, otherwise the supplied default value.
        /// </returns>
        public static T ValueOr<T>( this Nullable<T> value, T defaultValue, Func<T, T> ifValue ) where T : struct
        {
            if( value.HasValue )
            {
                if( ifValue != null )
                {
                    return ifValue( value.Value );
                }

                return value.Value;
            }

            return defaultValue;
        }
    }
}
