using System;
using System.Net;
using System.Windows;

namespace System.ComponentModel
{
	public sealed class EventHandlerList : IDisposable
	{
		// Fields
		private ListEntry head;

		// Methods
		public EventHandlerList()
		{
		}

		public void AddHandler( object key, Delegate value )
		{
			ListEntry entry = this.Find( key );
			if( entry != null )
			{
				entry.handler = Delegate.Combine( entry.handler, value );
			}
			else
			{
				this.head = new ListEntry( key, value, this.head );
			}
		}

		public void AddHandlers( EventHandlerList listToAddFrom )
		{
			for( ListEntry entry = listToAddFrom.head; entry != null; entry = entry.next )
			{
				this.AddHandler( entry.key, entry.handler );
			}
		}

		public void Dispose()
		{
			this.head = null;
		}

		private ListEntry Find( object key )
		{
			ListEntry head = this.head;
			while( head != null )
			{
				if( head.key == key )
				{
					return head;
				}
				head = head.next;
			}
			return head;
		}

		public void RemoveHandler( object key, Delegate value )
		{
			ListEntry entry = this.Find( key );
			if( entry != null )
			{
				entry.handler = Delegate.Remove( entry.handler, value );
			}
		}

		// Properties
		public Delegate this[ object key ]
		{
			get
			{
				ListEntry entry = this.Find( key );
				//if( ( this.parent == null ) || this.parent.CanRaiseEventsInternal )
				//{
				//    entry = this.Find( key );
				//}
				if( entry != null )
				{
					return entry.handler;
				}
				return null;
			}
			set
			{
				ListEntry entry = this.Find( key );
				if( entry != null )
				{
					entry.handler = value;
				}
				else
				{
					this.head = new ListEntry( key, value, this.head );
				}
			}
		}

		// Nested Types
		private sealed class ListEntry
		{
			// Fields
			internal Delegate handler;
			internal object key;
			internal EventHandlerList.ListEntry next;

			// Methods
			public ListEntry( object key, Delegate handler, EventHandlerList.ListEntry next )
			{
				this.next = next;
				this.key = key;
				this.handler = handler;
			}
		}
	}
}
