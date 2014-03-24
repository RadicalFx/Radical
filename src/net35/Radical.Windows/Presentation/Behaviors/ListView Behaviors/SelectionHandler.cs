using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using Topics.Radical.ComponentModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections;
using Topics.Radical.Linq;

namespace Topics.Radical.Windows.Behaviors
{
	sealed class SelectionHandler
	{
		ListView owner;
		IList selectedItems;

		NotifyCollectionChangedEventHandler ncceh;
		ListChangedEventHandler lceh;
		SelectionChangedEventHandler sceh;

		public SelectionHandler()
		{
			sceh = ( s, e ) =>
			{
				/*
				 * La ListView ci notifica che la selezione
				 * è cambiata.
				 * 
				 * Per prima cosa ci sganciamo temporaneamente
				 * dalla gestione delle notifiche in modo da non
				 * innescare un meccanismo ricorsivo infinito
				 */
				this.Unwire();

				var bag = this.GetSelectedItemsBag();
				e.RemovedItems.Enumerate( obj =>
				{
					var item = this.GetRealItem( obj );
					if( bag.Contains( item ) )
					{
						bag.Remove( item );
					}
				} );

				e.AddedItems.Enumerate( obj =>
				{
					var item = this.GetRealItem( obj );
					if( !bag.Contains( item ) )
					{
						bag.Add( item );
					}
				} );

				this.Wire();
			};

			ncceh = ( s, e ) =>
			{
				this.Unwire();

				switch( e.Action )
				{
					case NotifyCollectionChangedAction.Add:
						{
							this.AddToListViewSelection( e.NewItems );
						}
						break;

					case NotifyCollectionChangedAction.Remove:
						{
							this.RemoveFromListViewSelection( e.OldItems );
						}
						break;

					case NotifyCollectionChangedAction.Reset:
						{
							this.ClearListViewSelection();
							this.AddToListViewSelection( this.GetSelectedItemsBag() );
						}
						break;

					case NotifyCollectionChangedAction.Move:
					case NotifyCollectionChangedAction.Replace:
						//NOP
						break;

					default:
						throw new NotSupportedException();
				}

				this.Wire();
			};

			lceh = ( s, e ) =>
			{
				this.Unwire();

				switch( e.ListChangedType )
				{
					case ListChangedType.ItemAdded:
						{
							var bag = ( IEntityView )this.selectedItems;
							var item = bag[ e.NewIndex ];

							this.AddToListViewSelection( new[] { item } );
						}
						break;

					case ListChangedType.Reset:
						{
							this.ClearListViewSelection();
							this.AddToListViewSelection( this.selectedItems );
						}
						break;

					case ListChangedType.ItemDeleted:
						{
							this.RemoveFromListViewSelectionAtIndex( e.NewIndex );
						}
						break;

					case ListChangedType.ItemChanged:
					case ListChangedType.ItemMoved:
					case ListChangedType.PropertyDescriptorAdded:
					case ListChangedType.PropertyDescriptorChanged:
					case ListChangedType.PropertyDescriptorDeleted:
						//NOP
						break;

					default:
						throw new NotSupportedException();
				}

				this.Wire();
			};
		}

		void ClearListViewSelection()
		{
			switch( this.owner.SelectionMode )
			{
				case SelectionMode.Extended:
				case SelectionMode.Multiple:
					this.owner.SelectedItems.Clear();
					break;
				case SelectionMode.Single:
					this.owner.SelectedItem = null;
					break;

				default:
					throw new NotSupportedException( String.Format( "Unsupported ListView SelectionMode: {0}", this.owner.SelectionMode ) );
			}
		}

		void AddToListViewSelection( IEnumerable items )
		{
			switch( this.owner.SelectionMode )
			{
				case SelectionMode.Extended:
				case SelectionMode.Multiple:
					items.Enumerate( o => this.owner.SelectedItems.Add( o ) );
					break;
				case SelectionMode.Single:
					this.owner.SelectedItem = items.OfType<Object>().FirstOrDefault();
					break;

				default:
					throw new NotSupportedException( String.Format( "Unsupported ListView SelectionMode: {0}", this.owner.SelectionMode ) );
			}
		}

		void RemoveFromListViewSelection( IEnumerable items )
		{
			switch( this.owner.SelectionMode )
			{
				case SelectionMode.Extended:
				case SelectionMode.Multiple:
					items.Enumerate( o => this.owner.SelectedItems.Remove( o ) );
					break;
				case SelectionMode.Single:
					this.owner.SelectedItem = null;
					break;

				default:
					throw new NotSupportedException( String.Format( "Unsupported ListView SelectionMode: {0}", this.owner.SelectionMode ) );
			}
		}

		void RemoveFromListViewSelectionAtIndex( Int32 index )
		{
			switch( this.owner.SelectionMode )
			{
				case SelectionMode.Extended:
				case SelectionMode.Multiple:
					this.owner.SelectedItems.RemoveAt( index );
					break;
				case SelectionMode.Single:
					this.owner.SelectedItem = null;
					break;

				default:
					throw new NotSupportedException( String.Format( "Unsupported ListView SelectionMode: {0}", this.owner.SelectionMode ) );
			}
		}

		IList GetSelectedItemsBag()
		{
			var ev = this.selectedItems as IEntityView;
			if( ev != null )
			{
				return ev.DataSource;
			}
			return this.selectedItems;
		}

		Object GetRealItem( Object source )
		{
			var eiv = source as IEntityItemView;
			if( eiv != null )
			{
				return eiv.EntityItem;
			}

			return source;
		}

		public void SartSync( ListView owner, IList selectedItems )
		{
			this.owner = owner;
			this.selectedItems = selectedItems;

			this.Wire();
		}

		public void StopSync()
		{
			this.Unwire();

			this.owner = null;
			this.selectedItems = null;
		}

		Boolean CanSyncFromSource
		{
			get
			{
				var bag = this.selectedItems;
				return this.owner.SelectionMode != SelectionMode.Single && ( bag is INotifyCollectionChanged || bag is IEntityView );
			}
		}

		void Wire()
		{
			this.owner.SelectionChanged += sceh;

			if( this.CanSyncFromSource )
			{
				var bag = this.selectedItems;
				if( bag is INotifyCollectionChanged )
				{
					( ( INotifyCollectionChanged )bag ).CollectionChanged += ncceh;
				}
				else if( bag is IEntityView )
				{
					( ( IEntityView )bag ).ListChanged += lceh;
				}
			}
		}

		void Unwire()
		{
			this.owner.SelectionChanged -= sceh;
			if( this.CanSyncFromSource )
			{
				var bag = this.selectedItems;
				if( bag is INotifyCollectionChanged )
				{
					( ( INotifyCollectionChanged )bag ).CollectionChanged -= ncceh;
				}
				else if( bag is IEntityView )
				{
					( ( IEntityView )bag ).ListChanged -= lceh;
				}
			}
		}
	}
}
