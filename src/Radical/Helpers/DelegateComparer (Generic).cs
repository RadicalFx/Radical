using System;
using System.Collections.Generic;
using Radical.Validation;

namespace Radical
{
    /// <summary>
    /// Implements the generic IComparer in roder to 
    /// forward comparison to a external delegate.
    /// </summary>
    /// <typeparam name="T">The type of the item to compare.</typeparam>
    public sealed class DelegateComparer<T> : IComparer<T>
    {
        readonly Func<T, T, Int32> comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public DelegateComparer( Func<T, T, Int32> comparer )
        {
            Ensure.That( comparer ).Named( "comparer" ).IsNotNull();

            this.comparer = comparer;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value
        /// Condition
        /// Less than zero
        /// <paramref name="x"/> is less than <paramref name="y"/>.
        /// Zero
        /// <paramref name="x"/> equals <paramref name="y"/>.
        /// Greater than zero
        /// <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public int Compare( T x, T y )
        {
            return this.comparer( x, y );
        }
    }
}
