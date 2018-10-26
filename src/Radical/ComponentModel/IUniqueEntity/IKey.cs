namespace Radical.ComponentModel
{
    using System;

    /// <summary>
    /// Represents an abstraction of a primary key.
    /// </summary>
    public interface IKey : IEquatable<IKey>, IComparable //, IConvertible
    {
        ///// <summary>
        ///// Serializes this instance.
        ///// </summary>
        ///// <returns>A string that represents this instance data.</returns>
        //String Serialize();

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </value>
        Boolean IsEmpty { get; }
    }

    /// <summary>
    /// Represents an abstraction of a primary key.
    /// </summary>
    /// <typeparam name="T">The type (System.Type) of the primary key.</typeparam>
    public interface IKey<T> : IKey, IComparable<IKey<T>>, IEquatable<IKey<T>>
        where T : IComparable, IComparable<T>
    {
        /// <summary>
        /// Gets real value holded by this instance.
        /// </summary>
        /// <value>The value of the primary key.</value>
        T Value { get; }
    }
}