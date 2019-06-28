namespace Radical
{
    using Radical.ComponentModel;
    using System;

    /// <summary>
    /// A concrete implemantation of the <see cref="IKey"/> interface.
    /// </summary>
    /// <typeparam name="T">The System.Type of the data of this key value.</typeparam>
    [Serializable]
    [CLSCompliant( false )]
    public class Key<T> :
        Key,
        IKey<T>,
        IComparable<IKey<T>>
        where T : IComparable, IComparable<T>
    {
        private T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Key&lt;T&gt;"/> class.
        /// </summary>
        public Key()
        {
            value = default( T );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Key&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Key( T value )
        {
            if( value == null )
            {
                this.value = default( T );
            }
            else
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.string"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.string"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return ( this.value == null ) ? "" : this.value.ToString();
        }

        /// <summary>
        /// Gets real value holded by this instance.
        /// </summary>
        /// <value>The value of the primary key.</value>
        public T Value
        {
            get { return this.value; }
        }

        public override bool IsEmpty
        {
            get { return Object.Equals( this.Value, default( T ) ); }
        }

        /// <summary>
        /// Performs an implicit conversion from T to <see cref="Radical.Key&lt;T&gt;"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Key<T>( T value )
        {
            return new Key<T>( value );
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Radical.Key&lt;T&gt;"/> to T.
        /// </summary>
        /// <param name="pk">The pk.</param>
        /// <returns>The result of the conversion.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pk" )]
        public static implicit operator T( Key<T> pk )
        {
            if( pk == null )
            {
                return default( T );
            }
            else
            {
                return pk.value;
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="pk1">The PK1.</param>
        /// <param name="pk2">The PK2.</param>
        /// <returns>The result of the operator.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pk" )]
        public static bool operator ==( Key<T> pk1, Key<T> pk2 )
        {
            return Key<T>.Equals( pk1, pk2 );
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="pk1">The PK1.</param>
        /// <param name="pk2">The PK2.</param>
        /// <returns>The result of the operator.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pk" )]
        public static bool operator !=( Key<T> pk1, Key<T> pk2 )
        {
            return !Key<T>.Equals( pk1, pk2 );
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1800:DoNotCastUnnecessarily" )]
        public override bool Equals( object obj )
        {
            IKey<T> pk = obj as IKey<T>;
            return Key<T>.Equals( this, pk );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public override bool Equals( IKey other )
        {
            IKey<T> pk = other as IKey<T>;
            return Key<T>.Equals( this, pk );
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals( IKey<T> other )
        {
            return Key<T>.Equals( this, other );
        }

        /// <summary>
        /// Determines if the suppplied keys represents the same values.
        /// </summary>
        /// <param name="leftValue">The right side key value.</param>
        /// <param name="rightValue">The left side key value.</param>
        /// <returns><c>True</c> if the two instances are the same, otherwise <c>false</c>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes" )]
        public static bool Equals( IKey<T> leftValue, IKey<T> rightValue )
        {
            bool pk1IsNull = Object.ReferenceEquals( null, leftValue );
            bool pk2IsNull = Object.ReferenceEquals( null, rightValue );

            if( pk1IsNull && pk2IsNull )
            {
                return true;
            }
            else if( pk1IsNull || pk2IsNull )
            {
                return false;
            }
            else if( Object.ReferenceEquals( leftValue, rightValue ) )
            {
                return true;
            }
            else
            {
                /*
                 * Qui abbiamo la certezza che
                 * nessuna delle 2 chiavi è null
                 */
                return leftValue.CompareTo( rightValue ) == 0;
            }
        }

        static readonly Guid hashCode = new Guid( "cdf1f8d5-01bd-4d59-883e-d8198d8c3b93" );

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return hashCode.GetHashCode() ^ ( this.Value == null ? hashCode.GetHashCode() : this.Value.GetHashCode() );
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="obj"/> is not the same type as this instance. </exception>
        public override int CompareTo( object obj )
        {
            IKey<T> other = obj as IKey<T>;
            return this.CompareTo( other );
        }

        #region IComparable<Key<T>> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo( IKey<T> other )
        {
            if( Object.ReferenceEquals( other, null ) )
            {
                return 1;
            }

            bool thisValueIsNull = this.Value == null;
            bool otherValueIsNull = other.Value == null;

            if( thisValueIsNull && otherValueIsNull )
            {
                return 0;
            }
            else if( thisValueIsNull && !otherValueIsNull )
            {
                return -1;
            }
            else if( otherValueIsNull )
            {
                return 1;
            }
            else
            {
                return this.Value.CompareTo( other.Value );
            }
        }

        #endregion
    }
}
