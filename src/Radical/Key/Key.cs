namespace Radical
{
    using System;
    using Radical.ComponentModel;

    /// <summary>
    /// The <c>Key</c> class is an abstraction of a primary key.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes" )]
    [Serializable]
    [CLSCompliant( false )]
    public abstract class Key : IKey
    {
        #region IConvertible Members

        ///// <summary>
        ///// Returns the <see cref="T:System.TypeCode"/> for this instance.
        ///// </summary>
        ///// <returns>
        ///// The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.
        ///// </returns>
        //public abstract TypeCode GetTypeCode();

        ///// <summary>
        ///// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// A Boolean value equivalent to the value of this instance.
        ///// </returns>
        //public abstract Boolean ToBoolean( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An 8-bit unsigned integer equivalent to the value of this instance.
        ///// </returns>
        //public abstract Byte ToByte( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// A Unicode character equivalent to the value of this instance.
        ///// </returns>
        //public abstract char ToChar( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.
        ///// </returns>
        //public abstract DateTime ToDateTime( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.
        ///// </returns>
        //public abstract decimal ToDecimal( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// A double-precision floating-point number equivalent to the value of this instance.
        ///// </returns>
        //public abstract double ToDouble( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An 16-bit signed integer equivalent to the value of this instance.
        ///// </returns>
        //public abstract short ToInt16( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An 32-bit signed integer equivalent to the value of this instance.
        ///// </returns>
        //public abstract int ToInt32( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An 64-bit signed integer equivalent to the value of this instance.
        ///// </returns>
        //public abstract long ToInt64( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An 8-bit signed integer equivalent to the value of this instance.
        ///// </returns>
        //public abstract sbyte ToSByte( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// A single-precision floating-point number equivalent to the value of this instance.
        ///// </returns>
        //public abstract float ToSingle( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent <see cref="T:System.String"/> using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// A <see cref="T:System.String"/> instance equivalent to the value of this instance.
        ///// </returns>
        //public abstract string ToString( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="conversionType">The <see cref="T:System.Type"/> to which the value of this instance is converted.</param>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.
        ///// </returns>
        //public abstract object ToType( Type conversionType, IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An 16-bit unsigned integer equivalent to the value of this instance.
        ///// </returns>
        //public abstract ushort ToUInt16( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An 32-bit unsigned integer equivalent to the value of this instance.
        ///// </returns>
        //public abstract uint ToUInt32( IFormatProvider provider );

        ///// <summary>
        ///// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        ///// </summary>
        ///// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        ///// <returns>
        ///// An 64-bit unsigned integer equivalent to the value of this instance.
        ///// </returns>
        //public abstract ulong ToUInt64( IFormatProvider provider );

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="obj"/> is not the same type as this instance. </exception>
        public abstract int CompareTo( object obj );

        #endregion

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public abstract override int GetHashCode();

        #region IEquatable<IKey> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public abstract bool Equals( IKey other );

        #endregion

        public abstract Boolean IsEmpty { get; }
    }
}
