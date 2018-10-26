using System;
using System.Collections.Generic;
using Radical.Validation;

namespace Radical
{
    /// <summary>
    /// Inherits the generic EqualityComparer in order to 
    /// forward the comparison to the supplied delegates.
    /// </summary>
    /// <typeparam name="T">The type of the item to compare.</typeparam>
    public sealed class DelegateEqualityComparer<T> : EqualityComparer<T>
    {
        readonly Func<T, T, Boolean> comparer;
        readonly Func<T, Int32> hashCodeFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateEqualityComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        /// <param name="hashCodeFunc">The hash code func.</param>
        public DelegateEqualityComparer( Func<T, T, Boolean> comparer, Func<T, Int32> hashCodeFunc )
        {
            Ensure.That( comparer ).Named( "comparer" ).IsNotNull();
            Ensure.That( hashCodeFunc ).Named( "hashCodeFunc" ).IsNotNull();

            this.comparer = comparer;
            this.hashCodeFunc = hashCodeFunc;
        }

        /// <summary>
        /// When overridden in a derived class, determines whether two objects of type <para name="T"/> are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public override bool Equals( T x, T y )
        {
            return this.comparer( x, y );
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.
        /// </exception>
        public override int GetHashCode( T obj )
        {
            return this.hashCodeFunc( obj );
        }
    }
}
