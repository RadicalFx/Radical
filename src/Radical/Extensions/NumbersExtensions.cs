namespace Radical
{
    using System;

    /// <summary>
    /// Adds behaviors for numeric types.
    /// </summary>
    public static class NumbersExtensions
    {
        /// <summary>
        /// Determines whether the specified value is even.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     <c>true</c> if the specified value is even; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsEven( this Int32 value )
        {
            return value % 2 == 0;
        }
    }
}
