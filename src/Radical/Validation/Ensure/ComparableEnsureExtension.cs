using System;

namespace Radical.Validation
{
    /// <summary>
    /// Determines the equality behavior when comparing values.
    /// </summary>
    public enum Or
    {
        /// <summary>
        /// Values can be equal.
        /// </summary>
        Equal,

        /// <summary>
        /// Values cannot be equal.
        /// </summary>
        NotEqual
    }

    /// <summary>
    /// Determines the boundary evaluation behavior.
    /// </summary>
    [Flags]
    public enum Boundary
    {
        /// <summary>
        /// Lower bound is included.
        /// </summary>
        IncludeLower = 1,

        /// <summary>
        /// Lower bound is excluded.
        /// </summary>
        ExcludeLower = 2,

        /// <summary>
        /// Higher bound is included.
        /// </summary>
        IncludeHigher = 4,

        /// <summary>
        /// Higher bound is excluded.
        /// </summary>
        ExcludeHigher = 8,

        /// <summary>
        /// Bounds are excluded.
        /// </summary>
        ExcludeBounds = ExcludeLower | ExcludeHigher,
        
        /// <summary>
        /// Bounds are included.
        /// </summary>
        IncludeBounds = IncludeLower | IncludeHigher
    }

    /// <summary>
    /// Extends the generic Ensure class to add behaviors when the inspected object
    /// is an <c>IComparable(of T)</c>.
    /// </summary>
    public static class ComparableEnsureExtension
    {
        /// <summary>
        /// Extends the Ensure class when the inspected object is a generic <c>IComparabe</c> 
        /// and can be used to ensure that the inspected value is greater then an expected value.
        /// </summary>
        /// <typeparam name="T">The inspected value type.</typeparam>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <param name="expected">The expected value to compare to.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is smaller then the expected value.</exception>
        public static IEnsure<T> IsGreaterThen<T>( this IEnsure<T> validator, T expected ) where T : IComparable<T>
        {
            return ComparableEnsureExtension.IsGreaterThen( validator, expected, Or.NotEqual );
        }

        /// <summary>
        /// Extends the Ensure class when the inspected object is a generic <c>IComparabe</c>
        /// and can be used to ensure that the inspected value is greater then or equale to
        /// an expected value.
        /// </summary>
        /// <typeparam name="T">The inspected value type.</typeparam>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <param name="expected">The expected value to compare to.</param>
        /// <param name="boundaryBehavior">The boundary behavior in order to 
        /// be able to specify an OrEqual behavior.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is smaler then the expected value.</exception>
        public static IEnsure<T> IsGreaterThen<T>( this IEnsure<T> validator, T expected, Or boundaryBehavior ) where T : IComparable<T>
        {
            validator.If( s =>
            {
                var result = s.CompareTo( expected );

                switch( boundaryBehavior )
                {
                    case Or.Equal:
                        return result < 0;

                    case Or.NotEqual:
                        return result <= 0;

                    default:
                        throw new NotSupportedException();
                }
            } )
            .ThenThrow( v =>
            {
                var msg = String.Format( "The inspected value should be greater then{0} the given one.", boundaryBehavior == Or.Equal ? " or equal to" : "" );
                return new ArgumentOutOfRangeException( v.Name, v.GetFullErrorMessage( msg ) );
            } );

            return validator;
        }

        /// <summary>
        /// Extends the Ensure class when the inspected object is a generic <c>IComparabe</c>
        /// and can be used to ensure that the inspected value is smaller then an expected value.
        /// </summary>
        /// <typeparam name="T">The inspected value type.</typeparam>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <param name="expected">The expected value to compare to.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is greater then the expected value.</exception>
        public static IEnsure<T> IsSmallerThen<T>( this IEnsure<T> validator, T expected ) where T : IComparable<T>
        {
            return ComparableEnsureExtension.IsSmallerThen( validator, expected, Or.NotEqual );
        }

        /// <summary>
        /// Extends the Ensure class when the inspected object is a generic <c>IComparabe</c>
        /// and can be used to ensure that the inspected value is smaller then an expected value.
        /// </summary>
        /// <typeparam name="T">The inspected value type.</typeparam>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <param name="expected">The expected value to compare to.</param>
        /// <param name="boundaryBehavior">The boundary behavior in order to
        /// be able to specify an OrEqual behavior.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is greater then the expected value.</exception>
        public static IEnsure<T> IsSmallerThen<T>( this IEnsure<T> validator, T expected, Or boundaryBehavior ) where T : IComparable<T>
        {
            validator.If( s =>
            {
                var result = s.CompareTo( expected );

                switch( boundaryBehavior )
                {
                    case Or.Equal:
                        return result > 0;

                    case Or.NotEqual:
                        return result >= 0;

                    default:
                        throw new NotSupportedException();
                }
            } )
            .ThenThrow( v =>
            {
                var msg = String.Format( "The inspected value should be smaller then{0} the given one.", boundaryBehavior == Or.Equal ? " or equal to" : "" );
                return new ArgumentOutOfRangeException( v.Name, v.GetFullErrorMessage( msg ) );
            } );

            return validator;
        }

        /// <summary>
        /// Extends the Ensure class when the inspected object is a generic <c>IComparabe</c>
        /// and can be used to ensure that the inspected value is within a given range.
        /// </summary>
        /// <typeparam name="T">The inspected value type.</typeparam>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <param name="lowerBoundary">The lower boundary.</param>
        /// <param name="higherBoundary">The higher boundary.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is outside the expected range.</exception>
        public static IEnsure<T> IsWithin<T>( this IEnsure<T> validator, T lowerBoundary, T higherBoundary ) where T : IComparable<T>
        {
            return ComparableEnsureExtension.IsWithin( validator, lowerBoundary, higherBoundary, Boundary.ExcludeBounds );
        }

        /// <summary>
        /// Extends the Ensure class when the inspected object is a generic <c>IComparabe</c>
        /// and can be used to ensure that the inspected value is within a given range.
        /// </summary>
        /// <typeparam name="T">The inspected value type.</typeparam>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <param name="lowerBoundary">The lower boundary.</param>
        /// <param name="higherBoundary">The higher boundary.</param>
        /// <param name="boundaryBehavior">The boundary behavior in order to
        /// be able to specify an OrEqual behavior.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is outside the expected range.</exception>
        public static IEnsure<T> IsWithin<T>( this IEnsure<T> validator, T lowerBoundary, T higherBoundary, Boundary boundaryBehavior ) where T : IComparable<T>
        {
            var includeLower = ( boundaryBehavior & Boundary.IncludeLower ) == Boundary.IncludeLower;
            var includeHigher = ( boundaryBehavior & Boundary.IncludeHigher ) == Boundary.IncludeHigher;

            validator.IsGreaterThen( lowerBoundary, includeLower ? Or.Equal : Or.NotEqual )
                .IsSmallerThen( higherBoundary, includeHigher ? Or.Equal : Or.NotEqual );

            return validator;
        }
    }
}
