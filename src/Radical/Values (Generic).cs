using System;

namespace Radical
{
    /// <summary>
    /// A generic class for tuples support.
    /// </summary>
    /// <typeparam name="T1">The type of the first value.</typeparam>
    /// <typeparam name="T2">The type of the second value.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "2" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "1" )]
    public sealed class Values<T1, T2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Values&lt;T1, T2&gt;"/> class.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        public Values( T1 value1, T2 value2 )
        {
            this.Value1 = value1;
            this.Value2 = value2;
        }

        /// <summary>
        /// Gets or sets the first value.
        /// </summary>
        /// <value>The first value.</value>
        public T1 Value1 { get; private set; }

        /// <summary>
        /// Gets or sets the second value.
        /// </summary>
        /// <value>The second value.</value>
        public T2 Value2 { get; private set; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hcb = new Helpers.HashCodeBuilder( typeof( Values<T1, T2> ).GetHashCode() );
            if( !Object.ReferenceEquals( null, this.Value1 ) )
            {
                hcb.AddObject( this.Value1 );
            }

            if( !Object.ReferenceEquals( null, this.Value2 ) )
            {
                hcb.AddObject( this.Value2 );
            }

            return hcb.CombinedHash32;
        }
    }
}
