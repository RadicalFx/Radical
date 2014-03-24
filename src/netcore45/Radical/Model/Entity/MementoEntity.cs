using System;
using System.ComponentModel;
using Topics.Radical.ChangeTracking.Specialized;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.Validation;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topics.Radical.Linq;

namespace Topics.Radical.Model
{
	/// <summary>
	/// The <c>MementoEntity</c> class provides full support for the change tracking
	/// model exposed by the <see cref="IChangeTrackingService"/> interface.
	/// </summary>
	public abstract class MementoEntity :
		Entity,
		IMemento
	{
		Boolean isDisposed = false;

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( !isDisposed )
				{
					/*
					 * Questa chiamata la dobbiamo fare una volta sola 
					 * pena una bella ObjectDisposedException, altrimenti
					 * chiamate succesive alla Dispose fallirebbero
					 */
					( ( IMemento )this ).Memento = null;
				}
			}

			this._memento = null;

			/*
			 * Prima di passare il controllo alla classe base
			 * ci segnamo che la nostra dispose è andata a buon
			 * fine
			 */
			isDisposed = true;

			base.Dispose( disposing );
		}

		/// <summary>
		/// Verifies that this instance is not disposed, throwing an
		/// <see cref="ObjectDisposedException"/> if this instance has
		/// been already disposed.
		/// </summary>
		protected override void EnsureNotDisposed()
		{
			/*
			 * Facendo l'override della Dispose non ci "fidiamo"
			 * più del concetto di isDisposed della classe base
			 * perchè la Dispose potrebbe fallire e noi restare 
			 * in uno stato indeterminato
			 */
			if( this.isDisposed )
			{
				throw new ObjectDisposedException( this.GetType().FullName );
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoEntity"/> class.
		/// </summary>
		protected MementoEntity()
			: this( null, true )
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoEntity"/> class.
		/// </summary>
		/// <param name="memento">The memento.</param>
		protected MementoEntity( IChangeTrackingService memento )
			: this( memento, true )
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoEntity"/> class.
		/// </summary>
		/// <param name="registerAsTransient">if set to <c>true</c> [register as transient].</param>
		protected MementoEntity( Boolean registerAsTransient )
			: this( null, registerAsTransient )
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoEntity"/> class.
		/// </summary>
		/// <param name="memento">The memento.</param>
		/// <param name="registerAsTransient">if set to <c>true</c> [register as transient].</param>
		protected MementoEntity( IChangeTrackingService memento, Boolean registerAsTransient )
			: base()
		{
			this.registerAsTransient = registerAsTransient;
			( ( IMemento )this ).Memento = memento;
		}

		///// <summary>
		///// Gets the default property metadata.
		///// </summary>
		///// <param name="propertyName">Name of the property.</param>
		///// <param name="propertyType">Type of the property.</param>
		///// <returns>
		///// An instance of the requested default property metadata.
		///// </returns>
		//protected override PropertyMetadata GetDefaultMetadata( String propertyName, Type propertyType )
		//{
		//    var type = typeof( MementoPropertyMetadata<> ).MakeGenericType( propertyType );

		//    return ( PropertyMetadata )Activator.CreateInstance( type, new Object[] { propertyName } );
		//}

		protected override PropertyMetadata<T> GetDefaultMetadata<T>( string propertyName )
		{
			return MementoPropertyMetadata.Create<T>( propertyName );
		}

		protected MementoPropertyMetadata<T> SetInitialPropertyValue<T>( Expression<Func<T>> property, T value, Boolean trackChanges )
		{
			return this.SetInitialPropertyValue<T>( property.GetMemberName(), value, trackChanges );
		}

		protected MementoPropertyMetadata<T> SetInitialPropertyValue<T>( String property, T value, Boolean trackChanges )
		{
			var metadata = ( MementoPropertyMetadata<T> )base.SetInitialPropertyValue( property, value );

			metadata.TrackChanges = trackChanges;

			return metadata;
		}

		protected override void SetPropertyValue<T>( string propertyName, T data, PropertyValueChanged<T> pvc )
		{
			base.SetPropertyValue<T>( propertyName, data, e =>
			{
				var md = this.GetPropertyMetadata<T>( propertyName ) as MementoPropertyMetadata<T>;
				if( md != null && md.TrackChanges )
				{
					var callback = this.GetRejectCallback<T>( propertyName );
					this.CacheChange( e.OldValue, callback );
				}

				if( pvc != null )
				{
					pvc( e );
				}
			} );
		}

		readonly IDictionary<String, Delegate> rejectCallbacks = new Dictionary<String, Delegate>();

		RejectCallback<T> GetRejectCallback<T>( String propertyName )
		{
			Delegate d;
			if( !this.rejectCallbacks.TryGetValue( propertyName, out d ) )
			{
				RejectCallback<T> callback = ( pcr ) =>
				{
					var owner = ( MementoEntity )pcr.Source.Owner;
					var actualValue = owner.GetPropertyValue<T>( propertyName );
					var cb = this.GetRejectCallback<T>( propertyName );

					owner.CacheChangeOnRejectCallback( actualValue, cb, null, pcr );
					owner.SetPropertyValueCore( propertyName, pcr.CachedValue, null );
				};

				this.rejectCallbacks.Add( propertyName, callback );

				d = callback;
			}

			return ( RejectCallback<T> )d;
		}

		Boolean registerAsTransient;
		IChangeTrackingService _memento;

		/// <summary>
		/// Gets or sets the change tracking service to use as memento
		/// features provider.
		/// </summary>
		/// <value>The change tracking service.</value>
		IChangeTrackingService IMemento.Memento
		{
			get
			{
				this.EnsureNotDisposed();
				return this._memento;
			}
			set
			{
				this.EnsureNotDisposed();
				if( value != this._memento )
				{
					var old = ( ( IMemento )this ).Memento;
					this._memento = value;
					if( this.registerAsTransient && this.IsTracking )
					{
						this.OnRegisterTransient();
						this.registerAsTransient = false;
					}

					this.OnMementoChanged( value, old );
				}
			}
		}

		/// <summary>
		/// Gets the chenge tracking service.
		/// </summary>
		/// <returns>The current change tracking service, if any; otherwise null.</returns>
		protected IChangeTrackingService GetTrackingService() 
		{
			return ( ( IMemento )this ).Memento;
		}

		/// <summary>
		/// Registers this instance as transient.
		/// </summary>
		protected void RegisterTransient()
		{
			this.registerAsTransient = true;
		}

		/// <summary>
		/// Called in order to register this instance as transient.
		/// </summary>
		protected virtual void OnRegisterTransient()
		{
			( ( IMemento )this ).Memento.RegisterTransient( this );
		}

		/// <summary>
		/// Called when the <see cref="IChangeTrackingService"/> changes.
		/// </summary>
		/// <param name="newMemento">The new memento service.</param>
		/// <param name="oldMemmento">The old memmento service.</param>
		protected virtual void OnMementoChanged( IChangeTrackingService newMemento, IChangeTrackingService oldMemmento )
		{

		}

		/// <summary>
		/// Gets a item indicating whether there is an active change tracking service.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if there is an active change tracking service; otherwise, <c>false</c>.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic" )]
		protected virtual Boolean IsTracking
		{
			get
			{
				this.EnsureNotDisposed();
				return ( ( IMemento )this ).Memento != null && !( ( IMemento )this ).Memento.IsSuspended;
			}
		}

		/// <summary>
		/// Caches the supplied item in the active change tracking service.
		/// </summary>
		/// <typeparam name="T">The system type of the item to cache.</typeparam>
		/// <param name="value">The value to cache.</param>
		/// <param name="restore">A delegate to call when the change tracking 
		/// service needs to restore the cached change.</param>
		/// <returns>A reference to the cached change as an instance of <see cref="IChange"/> interface.</returns>
		protected IChange CacheChange<T>( T value, RejectCallback<T> restore )
		{
			this.EnsureNotDisposed();
			return this.CacheChange<T>( value, restore, null, AddChangeBehavior.Default );
		}

		/// <summary>
		/// Caches the supplied item in the active change tracking service.
		/// </summary>
		/// <typeparam name="T">The system type of the item to cache.</typeparam>
		/// <param name="value">The value to cache.</param>
		/// <param name="restore">A delegate to call when the change tracking
		/// service needs to restore the cached change.</param>
		/// <param name="commit">A delegate to call when the change tracking
		/// service needs to commit the cached change. Passing a null item for
		/// this parameter means that this instance does not need to be notified when
		/// the change is committed.</param>
		/// <returns>
		/// A reference to the cached change as an instance of <see cref="IChange"/> interface.
		/// </returns>
		protected IChange CacheChange<T>( T value, RejectCallback<T> restore, CommitCallback<T> commit )
		{
			this.EnsureNotDisposed();
			return this.CacheChange<T>( value, restore, commit, AddChangeBehavior.Default );
		}

		/// <summary>
		/// Caches the supplied item in the active change tracking service.
		/// </summary>
		/// <typeparam name="T">The system type of the item to cache.</typeparam>
		/// <param name="value">The value to cache.</param>
		/// <param name="restore">A delegate to call when the change tracking
		/// service needs to restore the cached change.</param>
		/// <param name="commit">A delegate to call when the change tracking
		/// service needs to commit the cached change. Passing a null item for
		/// this parameter means that this instance does not need to be notified when
		/// the change is committed.</param>
		/// <returns>
		/// A reference to the cached change as an instance of <see cref="IChange"/> interface.
		/// </returns>
		protected IChange CacheChange<T>( T value, RejectCallback<T> restore, CommitCallback<T> commit, AddChangeBehavior direction )
		{
			this.EnsureNotDisposed();
			if( this.IsTracking )
			{
				IChange iChange = new PropertyValueChange<T>( this, value, restore, commit, String.Empty );

				( ( IMemento )this ).Memento.Add( iChange, direction );

				return iChange;
			}

			return null;
		}

		protected virtual IChange CacheChangeOnRejectCallback<T>( T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, ChangeRejectedEventArgs<T> args )
		{
			this.EnsureNotDisposed();
			switch( args.Reason )
			{
				case RejectReason.Undo:
					return this.CacheChange( value, rejectCallback, commitCallback, AddChangeBehavior.UndoRequest );

				case RejectReason.Redo:
					return this.CacheChange( value, rejectCallback, commitCallback, AddChangeBehavior.RedoRequest );

				case RejectReason.RejectChanges:
				case RejectReason.Revert:
					return null;

				case RejectReason.None:
					throw new ArgumentOutOfRangeException();

				default:
					throw new EnumValueOutOfRangeException();
			}
		}
	}
}
