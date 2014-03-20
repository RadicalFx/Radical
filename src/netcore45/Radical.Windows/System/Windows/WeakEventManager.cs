using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace System.Windows
{
	/// <summary>
	/// A WeakEventManager for Silverlight and Windows Phone.
	/// </summary>
	public abstract class WeakEventManager : DependencyObject
	{
		private struct SourceKey
		{
			WeakEventManager _manager;
			object _source;             // lookup: direct ref;  In table: WeakRef 
			int _hashcode;              // cached, in case source is GC'd

			internal SourceKey( WeakEventManager manager, object source )
			{
				_manager = manager;
				_source = source;
				_hashcode = unchecked( manager.GetHashCode() + RuntimeHelpers.GetHashCode( source ) );
			}

			internal object Source
			{
				get { return ( ( WeakReference )_source ).Target; }
			}

			internal WeakEventManager Manager
			{
				get { return _manager; }
			}

			/// <summary>
			/// Returns a hash code for this instance.
			/// </summary>
			/// <returns>
			/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
			/// </returns>
			public override int GetHashCode()
			{
#if DEBUG
				WeakReference wr = _source as WeakReference;
				object source = ( wr != null ) ? wr.Target : _source;
				if( source != null )
				{
					int hashcode = unchecked( _manager.GetHashCode() + RuntimeHelpers.GetHashCode( source ) );
					Debug.Assert( hashcode == _hashcode, "hashcodes disagree" );
				}
#endif

				return _hashcode;
			}

			/// <summary>
			/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
			/// </summary>
			/// <param name="o">The <see cref="System.Object"/> to compare with this instance.</param>
			/// <returns>
			/// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
			/// </returns>
			public override bool Equals( object o )
			{
				if( o is SourceKey )
				{
					WeakReference wr;
					SourceKey ek = ( SourceKey )o;

					if( _manager != ek._manager || _hashcode != ek._hashcode )
						return false;

					wr = this._source as WeakReference;
					object s1 = ( wr != null ) ? wr.Target : this._source;
					wr = ek._source as WeakReference;
					object s2 = ( wr != null ) ? wr.Target : ek._source;

					if( s1 != null && s2 != null )
						return ( s1 == s2 );
					else
						return ( _source == ek._source );
				}
				else
				{
					return false;
				}
			}

			/// <summary>
			/// Implements the operator ==.
			/// </summary>
			/// <param name="key1">The key1.</param>
			/// <param name="key2">The key2.</param>
			/// <returns>The result of the operator.</returns>
			public static bool operator ==( SourceKey key1, SourceKey key2 )
			{
				return key1.Equals( key2 );
			}

			/// <summary>
			/// Implements the operator !=.
			/// </summary>
			/// <param name="key1">The key1.</param>
			/// <param name="key2">The key2.</param>
			/// <returns>The result of the operator.</returns>
			public static bool operator !=( SourceKey key1, SourceKey key2 )
			{
				return !key1.Equals( key2 );
			}
		}

		readonly IDictionary<SourceKey, IList<WeakReference>> _list = new Dictionary<SourceKey, IList<WeakReference>>();

		static readonly object staticSource = new Object();
		static readonly object syncRoot = new object();
		static IDictionary<Type, WeakEventManager> managers = new Dictionary<Type, WeakEventManager>();

		/// <summary>
		/// Gets the current manager.
		/// </summary>
		/// <param name="managerType">Type of the manager.</param>
		/// <returns>The current manager.</returns>
		public static WeakEventManager GetCurrentManager( Type managerType )
		{
			if( !managers.ContainsKey( managerType ) )
			{
				lock( syncRoot )
				{
					if( !managers.ContainsKey( managerType ) )
					{
						return null;
					}
					else
					{
						return managers[ managerType ];
					}
				}
			}
			else
			{
				lock( syncRoot )
				{
					if( managers.ContainsKey( managerType ) )
					{
						return managers[ managerType ];
					}
					else
					{
						return null;
					}
				}
			}
		}

		/// <summary>
		/// Sets the current manager.
		/// </summary>
		/// <param name="managerType">Type of the manager.</param>
		/// <param name="manager">The manager.</param>
		public static void SetCurrentManager( Type managerType, WeakEventManager manager )
		{
			if( !managers.ContainsKey( managerType ) )
			{
				lock( syncRoot )
				{
					if( !managers.ContainsKey( managerType ) )
					{
						managers.Add( managerType, manager );
					}
				}
			}
		}

		/// <summary>
		/// Adds the given listener.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="listener">The listener.</param>
		protected void ProtectedAddListener( Object source, IWeakEventListener listener )
		{
			var key = new SourceKey( this, source == null ? staticSource : source );

			lock( syncRoot )
			{
				var reference = new WeakReference( listener );
				if( _list.ContainsKey( key ) )
				{
					_list[ key ].Add( reference );
				}
				else
				{
					var list = new List<WeakReference>()
					{
						reference
					};

					_list.Add( key, list );
				}

				this.StartListening( source );
			}
		}

		/// <summary>
		/// Remove the given listener.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="listener">The listener.</param>
		protected void ProtectedRemoveListener( Object source, IWeakEventListener listener )
		{
			var key = new SourceKey( this, source == null ? staticSource : source );

			if( _list != null )
			{
				lock( syncRoot )
				{
					if( _list.ContainsKey( key ) )
					{
						// Stop responding to changes
						this.StopListening( source );
						// Remove the item from the list.
						WeakReference reference = null;
						foreach( WeakReference item in _list[ key ] )
						{
							if( item.Target.Equals( listener ) )
							{
								reference = item;
							}
						}

						if( reference != null )
						{
							_list[ key ].Remove( reference );
						}
					}
				}
			}
		}

		/// <summary>
		/// Starts the listening.
		/// </summary>
		/// <param name="source">The source.</param>
		protected abstract void StartListening( Object source );

		/// <summary>
		/// Stops the listening.
		/// </summary>
		/// <param name="source">The source.</param>
		protected abstract void StopListening( Object source );

		/// <summary>
		/// Delivers the event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void DeliverEvent( Object sender, Object args )
		{
			var key = new SourceKey( this, sender == null ? staticSource : sender );

			var list = _list[ key ];
			if( list != null )
			{
				// We have the listeners. Deal with them
				foreach( var item in list )
				{
					var eventItem = item.Target as IWeakEventListener;
					if( eventItem != null && item.IsAlive )
					{
						eventItem.ReceiveWeakEvent( this.GetType(), sender, args );
					}
				}
			}
		}
	}
}