using Radical.Validation;
using System;

namespace Radical
{
    /// <summary>
    /// Adds beheviors to arrays of data.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Determines whether two arrays are structurally, structure and content, equals.
        /// </summary>
        /// <typeparam name="T">The type of the array data.</typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="other">The array to compare to.</param>
        /// <returns>
        ///     <c>true</c> if the two arrays are structurally equals; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSameAs<T>( this T[] source, T[] other )
            where T : IComparable
        {
            return IsSameAs( source, other, ( s, o ) =>
            {
                return s.CompareTo( o ) == 0;
            } );
        }

        /// <summary>
        /// Determines whether two arrays are structurally, structure and 
        /// content, equals using the supplied item comparer to compare
        /// items equality.
        /// </summary>
        /// <typeparam name="T">The type of the array data.</typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="other">The array to compare to.</param>
        /// <param name="itemComparer">The item comparer.</param>
        /// <returns>
        ///     <c>true</c> if the two arrays are structurally equals; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSameAs<T>( this T[] source, T[] other, Func<T, T, bool> itemComparer )
        {
            Ensure.That( source ).Named( "source" ).IsNotNull();
            Ensure.That( itemComparer ).Named( "itemComparer" ).IsNotNull();

            if( other == null )
            {
                return false;
            }

            if( Object.ReferenceEquals( source, other ) ) 
            {
                return true;
            }

            if( source.Length != other.Length )
            {
                return false;
            }

            for( int i = 0; i < source.Length; i++ )
            {
                if( !itemComparer( source[ i ], other[ i ] ) )
                {
                    return false;
                }
            }

            return true;
        }
    }
}
