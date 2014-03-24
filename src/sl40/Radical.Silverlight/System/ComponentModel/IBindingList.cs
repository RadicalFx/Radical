using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.Generic;

namespace System.ComponentModel
{
	public delegate void ListChangedEventHandler( object sender, ListChangedEventArgs e );

	public enum ListChangedType
	{
		Reset,
		ItemAdded,
		ItemDeleted,
		ItemMoved,
		ItemChanged,
		PropertyDescriptorAdded,
		PropertyDescriptorDeleted,
		PropertyDescriptorChanged
	}




	public class ListChangedEventArgs : EventArgs
	{
		// Fields
		private ListChangedType listChangedType;
		private int newIndex;
		private int oldIndex;
		private PropertyDescriptor propDesc;

		// Methods
		public ListChangedEventArgs( ListChangedType listChangedType, PropertyDescriptor propDesc )
		{
			this.listChangedType = listChangedType;
			this.propDesc = propDesc;
		}

		public ListChangedEventArgs( ListChangedType listChangedType, int newIndex )
			: this( listChangedType, newIndex, -1 )
		{
		}

		public ListChangedEventArgs( ListChangedType listChangedType, int newIndex, PropertyDescriptor propDesc )
			: this( listChangedType, newIndex )
		{
			this.propDesc = propDesc;
			this.oldIndex = newIndex;
		}

		public ListChangedEventArgs( ListChangedType listChangedType, int newIndex, int oldIndex )
		{
			this.listChangedType = listChangedType;
			this.newIndex = newIndex;
			this.oldIndex = oldIndex;
		}

		// Properties
		public ListChangedType ListChangedType
		{
			get
			{
				return this.listChangedType;
			}
		}

		public int NewIndex
		{
			get
			{
				return this.newIndex;
			}
		}

		public int OldIndex
		{
			get
			{
				return this.oldIndex;
			}
		}

		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propDesc;
			}
		}
	}


	public interface IBindingList : IList, ICollection, IEnumerable
	{
		// Events
		event ListChangedEventHandler ListChanged;

		// Methods
		void AddIndex( PropertyDescriptor property );
		object AddNew();
		void ApplySort( PropertyDescriptor property, ListSortDirection direction );
		int Find( PropertyDescriptor property, object key );
		void RemoveIndex( PropertyDescriptor property );
		void RemoveSort();

		// Properties
		bool AllowEdit { get; }
		bool AllowNew { get; }
		bool AllowRemove { get; }
		bool IsSorted { get; }
		ListSortDirection SortDirection { get; }
		PropertyDescriptor SortProperty { get; }
		bool SupportsChangeNotification { get; }
		bool SupportsSearching { get; }
		bool SupportsSorting { get; }
	}

	public interface IBindingListView : IBindingList, IList, ICollection, IEnumerable
	{
		// Methods
		void ApplySort( ListSortDescriptionCollection sorts );
		void RemoveFilter();

		// Properties
		string Filter { get; set; }
		ListSortDescriptionCollection SortDescriptions { get; }
		bool SupportsAdvancedSorting { get; }
		bool SupportsFiltering { get; }
	}

	public class ListSortDescription
	{
		// Fields
		private PropertyDescriptor property;
		private ListSortDirection sortDirection;

		// Methods
		public ListSortDescription( PropertyDescriptor property, ListSortDirection direction )
		{
			this.property = property;
			this.sortDirection = direction;
		}

		// Properties
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.property;
			}
			set
			{
				this.property = value;
			}
		}

		public ListSortDirection SortDirection
		{
			get
			{
				return this.sortDirection;
			}
			set
			{
				this.sortDirection = value;
			}
		}
	}


	public class ListSortDescriptionCollection : IList, ICollection, IEnumerable
	{
		// Fields
		private List<Object> sorts;

		// Methods
		public ListSortDescriptionCollection()
		{
			this.sorts = new List<object>();
		}

		public ListSortDescriptionCollection( ListSortDescription[] sorts )
		{
			this.sorts = new List<object>();
			if( sorts != null )
			{
				for( int i = 0; i < sorts.Length; i++ )
				{
					this.sorts.Add( sorts[ i ] );
				}
			}
		}

		public bool Contains( object value )
		{
			return this.sorts.Contains( value );
		}

		public void CopyTo( Array array, int index )
		{
			//WARN: todo...
			//this.sorts.CopyTo( array, index );
		}

		public int IndexOf( object value )
		{
			return this.sorts.IndexOf( value );
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.sorts.GetEnumerator();
		}

		int IList.Add( object value )
		{
			throw new InvalidOperationException( "CantModifyListSortDescriptionCollection" );
		}

		void IList.Clear()
		{
			throw new InvalidOperationException( "CantModifyListSortDescriptionCollection" );
		}

		void IList.Insert( int index, object value )
		{
			throw new InvalidOperationException( "CantModifyListSortDescriptionCollection" );
		}

		void IList.Remove( object value )
		{
			throw new InvalidOperationException( "CantModifyListSortDescriptionCollection" );
		}

		void IList.RemoveAt( int index )
		{
			throw new InvalidOperationException( "CantModifyListSortDescriptionCollection" );
		}

		// Properties
		public int Count
		{
			get
			{
				return this.sorts.Count;
			}
		}

		public ListSortDescription this[ int index ]
		{
			get
			{
				return ( ListSortDescription )this.sorts[ index ];
			}
			set
			{
				throw new InvalidOperationException( "CantModifyListSortDescriptionCollection" );
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		object IList.this[ int index ]
		{
			get
			{
				return this[ index ];
			}
			set
			{
				throw new InvalidOperationException( "CantModifyListSortDescriptionCollection" );
			}
		}
	}

	public interface ITypedList
	{
		// Methods
		PropertyDescriptorCollection GetItemProperties( PropertyDescriptor[] listAccessors );
		string GetListName( PropertyDescriptor[] listAccessors );
	}

	public class PropertyDescriptorCollection : IList
	{
		// Methods
		public PropertyDescriptorCollection( PropertyDescriptor[] properties )
		{ }

		int IList.Add( object value )
		{
			throw new NotImplementedException();
		}

		void IList.Clear()
		{
			throw new NotImplementedException();
		}

		bool IList.Contains( object value )
		{
			throw new NotImplementedException();
		}

		int IList.IndexOf( object value )
		{
			throw new NotImplementedException();
		}

		void IList.Insert( int index, object value )
		{
			throw new NotImplementedException();
		}

		bool IList.IsFixedSize
		{
			get { throw new NotImplementedException(); }
		}

		bool IList.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		void IList.Remove( object value )
		{
			throw new NotImplementedException();
		}

		void IList.RemoveAt( int index )
		{
			throw new NotImplementedException();
		}

		object IList.this[ int index ]
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		void ICollection.CopyTo( Array array, int index )
		{
			throw new NotImplementedException();
		}

		int ICollection.Count
		{
			get { throw new NotImplementedException(); }
		}

		bool ICollection.IsSynchronized
		{
			get { throw new NotImplementedException(); }
		}

		object ICollection.SyncRoot
		{
			get { throw new NotImplementedException(); }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}


	public interface ICancelAddNew
	{
		// Methods
		void CancelNew( int itemIndex );
		void EndNew( int itemIndex );
	}


	public interface IRaiseItemChangedEvents
	{
		// Properties
		bool RaisesItemChangedEvents { get; }
	}


	public interface ICustomTypeDescriptor
	{
		// Methods
		//AttributeCollection GetAttributes();
		string GetClassName();
		string GetComponentName();
		TypeConverter GetConverter();
		//EventDescriptor GetDefaultEvent();
		PropertyDescriptor GetDefaultProperty();
		object GetEditor( Type editorBaseType );
		//EventDescriptorCollection GetEvents();
		//EventDescriptorCollection GetEvents( Attribute[] attributes );
		PropertyDescriptorCollection GetProperties();
		PropertyDescriptorCollection GetProperties( Attribute[] attributes );
		object GetPropertyOwner( PropertyDescriptor pd );
	}


}
