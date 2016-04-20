using System;

namespace Topics.Radical.ComponentModel
{
    /// <summary>
    /// A domain entity that can be identified by a primary key.
    /// </summary>
    /// <typeparam name="T">Ths System.Type of the primary key data.</typeparam>
    public interface IUniqueEntity<T> : IUniqueEntity where T : IComparable, IComparable<T>
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        new IKey<T> Key { get; }
    }
}