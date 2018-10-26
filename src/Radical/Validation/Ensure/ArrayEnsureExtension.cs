namespace Radical.Validation
{
    using System;

    /// <summary>
    /// Extends the generic Ensure class to add behaviors when the inspected object
    /// is an <c>Array</c>.
    /// </summary>
    public static class ArrayEnsureExtension
    {
        /// <summary>
        /// Determines whether the specified index is inside the bounds of
        /// the inspected array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="validator">The current, extended, Ensure instance.</param>
        /// <param name="index">The index to validate.</param>
        /// <returns>The Ensure instance for fluent interface usage.</returns>
        public static IEnsure<T[]> ContainsIndex<T>( this IEnsure<T[]> validator, Int32 index )
        {
            validator.If( data => ( index < 0 || index >= data.Length ) )
                .ThenThrow( v =>
                {
                    var paramName = String.IsNullOrEmpty( v.Name ) ? "index" : v.Name;
                    var message = v.GetFullErrorMessage( "The supplied Array does not contains the given index." );

                    return new ArgumentOutOfRangeException( paramName, message );
                } );

            return validator;
        }
    }
}
