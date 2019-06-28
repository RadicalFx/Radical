using System;
using System.Text.RegularExpressions;

namespace Radical.Validation
{
    /// <summary>
    /// Extends the generic Ensure class to add behaviors when the inspected object
    /// is a <c>string</c>.
    /// </summary>
    public static class StringEnsureExtension
    {
        /// <summary>
        /// Extends the Ensure class when the inspected object is a <c>string</c> and can
        /// be used to ensure that the inspected string is not an empty string.
        /// </summary>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is an empty string.</exception>
        public static IEnsure<string> IsNotEmpty( this IEnsure<string> validator )
        {
            //var value = validator.GetValue<string>();

            //if ( value != null && value.Length == 0 ) 
            //{
            //    throw new ArgumentOutOfRangeException( validator.Name, validator.GetFullErrorMessage( "The inspected string value should be not empty." ) );
            //}

            validator.If( s => s != null && s.Length == 0 )
                .ThenThrow( e =>
                {
                    return new ArgumentOutOfRangeException( e.Name, e.GetFullErrorMessage( "The inspected string value should be not empty." ) );
                } );

            return validator;
        }

        /// <summary>
        /// Extends the Ensure class when the inspected object is a <c>string</c> and can
        /// be used to ensure that the inspected string is not an empty string and not a null string.
        /// </summary>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is an empty string.</exception>
        /// <exception cref="ArgumentNullException">An <c>ArgumentNullException</c>
        /// is raised if the current inspected object is a null string.</exception>
        public static IEnsure<string> IsNotNullNorEmpty( this IEnsure<string> validator )
        {
            return validator.IsNotNull().IsNotEmpty();
        }

        /// <summary>
        /// Extends the Ensure class when the inspected object is a <c>string</c> and can
        /// be used to ensure that the inspected string matches the given regular expression.
        /// </summary>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <param name="regExPattern">The regular expression pattern.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="FormatException">A <c>FormatException</c>
        /// is raised if the current inspected object does not match the given regular expression.
        /// </exception>
        public static IEnsure<string> Matches( this IEnsure<string> validator, string regExPattern )
        {
            validator.If( s =>
            {
                bool match = Regex.IsMatch( validator.Value, regExPattern );

                return !match;
            } )
            .ThenThrow( v =>
            {
                return new FormatException( v.GetFullErrorMessage( "The inspected string value does not match the given format." ) );
            } );

            return validator;
        }
    }
}
