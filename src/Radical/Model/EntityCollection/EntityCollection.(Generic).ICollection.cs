using System;
using System.Collections;
using System.Collections.Generic;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {
        bool ICollection<T>.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        /// <summary>
        /// Gets a item indicating whether access to the <see cref="System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <item></item>
        /// <returns>true if access to the <see cref="System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
        bool ICollection.IsSynchronized
        {
            get
            {
                EnsureNotDisposed();
                return false;
            }
        }

        static readonly object syncRoot = new object();

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="System.Collections.ICollection"/>.</returns>
        object ICollection.SyncRoot
        {
            get
            {
                EnsureNotDisposed();
                return syncRoot;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="System.Collections.ICollection"/> to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="System.Collections.ICollection"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="array"/> is null. </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than zero. </exception>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="array"/> is multidimensional.-or- <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.-or- The number of elements in the source <see cref="System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
        /// <exception cref="System.ArgumentException">The type of the source <see cref="System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
        void ICollection.CopyTo(Array array, int index)
        {
            EnsureNotDisposed();
            Storage.CopyTo((T[])array, index);
        }
    }
}
