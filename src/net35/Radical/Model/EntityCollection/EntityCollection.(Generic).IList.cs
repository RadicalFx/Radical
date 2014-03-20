using System;
using System.Collections;

namespace Topics.Radical.Model
{
	partial class EntityCollection<T>
	{
		/// <summary>
		/// Gets a item indicating whether the <see cref="T:System.Collections.IList"></see> has a fixed size.
		/// </summary>
		/// <item></item>
		/// <returns>true if the <see cref="T:System.Collections.IList"></see> has a fixed size; otherwise, false.</returns>
		Boolean IList.IsFixedSize
		{
			get
			{
				this.EnsureNotDisposed();
				return false;
			}
		}


		/// <summary>
		/// Gets a item indicating whether the <see cref="T:System.Collections.IList"></see> is read-only.
		/// </summary>
		/// <item></item>
		/// <returns>true if the <see cref="T:System.Collections.IList"></see> is read-only; otherwise, false.</returns>
		Boolean IList.IsReadOnly
		{
			get
			{
				this.EnsureNotDisposed();
				return this.IsReadOnly;
			}
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// The position into which the new element was inserted.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		Int32 IList.Add( object value )
		{
			this.EnsureNotDisposed();
			this.Add( ( T )value );

			return this.Count - 1;
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific item.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
		/// </returns>
		Boolean IList.Contains( object value )
		{
			this.EnsureNotDisposed();
			return this.Contains( ( T )value );
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>.</param>
		/// <returns>
		/// The index of <paramref name="value"/> if found in the list; otherwise, -1.
		/// </returns>
		Int32 IList.IndexOf( object value )
		{
			this.EnsureNotDisposed();
			return this.IndexOf( ( T )value );
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to insert into the <see cref="T:System.Collections.IList"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		/// <exception cref="T:System.NullReferenceException">
		/// 	<paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
		void IList.Insert( Int32 index, object value )
		{
			this.EnsureNotDisposed();
			this.Insert( index, ( T )value );
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The <see cref="T:System.Object"/> to remove from the <see cref="T:System.Collections.IList"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
		void IList.Remove( object value )
		{
			this.EnsureNotDisposed();
			this.Remove( ( T )value );
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Object"/> at the specified index.
		/// </summary>
		/// <item></item>
		object IList.this[ Int32 index ]
		{
			get
			{
				this.EnsureNotDisposed();
				return this.GetValueAt( index );
			}
			set
			{
				this.EnsureNotDisposed();
				this.SetValueAt( index, ( T )value );
			}
		}
	}
}
