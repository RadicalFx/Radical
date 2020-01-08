using Radical.ChangeTracking.Specialized;
using Radical.ComponentModel.ChangeTracking;
using Radical.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Radical.Model
{
    /// <summary>
    /// The <c>MementoEntity</c> class provides full support for the change tracking
    /// model exposed by the <see cref="IChangeTrackingService"/> interface.
    /// </summary>
    [Serializable]
    public abstract class MementoEntity :
        Entity,
        IMemento
    {
        bool isDisposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                /*
                    * Questa chiamata la dobbiamo fare una volta sola 
                    * pena una bella ObjectDisposedException, altrimenti
                    * chiamate succesive alla Dispose fallirebbero
                    */
                ((IMemento)this).Memento = null;
            }

            _memento = null;

            /*
             * Prima di passare il controllo alla classe base
             * ci segnamo che la nostra dispose è andata a buon
             * fine
             */
            isDisposed = true;

            base.Dispose(disposing);
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
            if (isDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoEntity"/> class.
        /// </summary>
        protected MementoEntity()
            : this(null, ChangeTrackingRegistration.AsTransient)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoEntity"/> class.
        /// </summary>
        /// <param name="memento">The memento.</param>
        protected MementoEntity(IChangeTrackingService memento)
            : this(memento, ChangeTrackingRegistration.AsTransient)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoEntity"/> class.
        /// </summary>
        /// <param name="registerAsTransient">if set to <c>true</c> [register as transient].</param>
        protected MementoEntity(bool registerAsTransient)
            : this(null, registerAsTransient ? ChangeTrackingRegistration.AsTransient : ChangeTrackingRegistration.AsPersistent)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoEntity" /> class.
        /// </summary>
        /// <param name="registration">The registration.</param>
        protected MementoEntity(ChangeTrackingRegistration registration)
            : this(null, registration)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoEntity" /> class.
        /// </summary>
        /// <param name="memento">The memento.</param>
        /// <param name="registration">The registration.</param>
        protected MementoEntity(IChangeTrackingService memento, ChangeTrackingRegistration registration)
            : base()
        {
            this.registration = registration;
            ((IMemento)this).Memento = memento;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoEntity"/> class.
        /// </summary>
        /// <param name="memento">The memento.</param>
        /// <param name="registerAsTransient">if set to <c>true</c> [register as transient].</param>
        protected MementoEntity(IChangeTrackingService memento, bool registerAsTransient)
            : this(memento, registerAsTransient ? ChangeTrackingRegistration.AsTransient : ChangeTrackingRegistration.AsPersistent)
        {

        }

        /// <summary>
        /// Gets the default property meta-data.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// An instance of the requested default property meta-data.
        /// </returns>
        protected override PropertyMetadata<T> GetDefaultMetadata<T>(string propertyName)
        {
            return MementoPropertyMetadata.Create<T>(this, propertyName);
        }

        /// <summary>
        /// Sets the initial property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="trackChanges">The track changes.</param>
        /// <returns>
        /// This method is a shortcut for the GetMetadata method, in order
        /// to fine customize property meta-data use the GetMetadata method.
        /// The main difference between SetInitialPropertyValue and SetPropertyValue
        /// is that SetInitialPropertyValue does not raise a property change notification.
        /// </returns>
        protected MementoPropertyMetadata<T> SetInitialPropertyValue<T>(Expression<Func<T>> property, T value, bool trackChanges)
        {
            return SetInitialPropertyValue<T>(property.GetMemberName(), value, trackChanges);
        }

        /// <summary>
        /// Sets the initial property value, using a lazily evaluated value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <param name="lazyValue">The lazy value.</param>
        /// <param name="trackChanges">The track changes.</param>
        /// <returns>
        /// This method is a shortcut for the GetMetadata method, in order
        /// to fine customize property meta-data use the GetMetadata method.
        /// The main difference between SetInitialPropertyValue and SetPropertyValue
        /// is that SetInitialPropertyValue does not raise a property change notification.
        /// </returns>
        protected MementoPropertyMetadata<T> SetInitialPropertyValue<T>(Expression<Func<T>> property, Func<T> lazyValue, bool trackChanges)
        {
            var metadata = (MementoPropertyMetadata<T>)SetInitialPropertyValue(property, lazyValue);

            metadata.TrackChanges = trackChanges;

            return metadata;
        }

        /// <summary>
        /// Sets the initial property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="trackChanges">The track changes.</param>
        /// <returns>
        /// This method is a shortcut for the GetMetadata method, in order
        /// to fine customize property meta-data use the GetMetadata method.
        /// The main difference between SetInitialPropertyValue and SetPropertyValue
        /// is that SetInitialPropertyValue does not raise a property change notification.
        /// </returns>
        protected MementoPropertyMetadata<T> SetInitialPropertyValue<T>(string property, T value, bool trackChanges)
        {
            var metadata = (MementoPropertyMetadata<T>)SetInitialPropertyValue(property, value);

            metadata.TrackChanges = trackChanges;

            return metadata;
        }

        protected override void SetPropertyValue<T>(string propertyName, T data, PropertyValueChanged<T> pvc)
        {
            base.SetPropertyValue<T>(propertyName, data, e =>
           {
               var md = GetPropertyMetadata<T>(propertyName) as MementoPropertyMetadata<T>;
               if (md != null && md.TrackChanges)
               {
                   var callback = GetRejectCallback<T>(propertyName);
                   CacheChange(propertyName, e.OldValue, callback);
               }

               if (pvc != null)
               {
                   pvc(e);
               }
           });
        }

        readonly IDictionary<string, Delegate> rejectCallbacks = new Dictionary<string, Delegate>();

        RejectCallback<T> GetRejectCallback<T>(string propertyName)
        {
            Delegate d;
            if (!rejectCallbacks.TryGetValue(propertyName, out d))
            {
                RejectCallback<T> callback = (pcr) =>
                {
                    var owner = (MementoEntity)pcr.Source.Owner;
                    var actualValue = owner.GetPropertyValue<T>(propertyName);
                    var cb = GetRejectCallback<T>(propertyName);

                    owner.CacheChangeOnRejectCallback(propertyName, actualValue, cb, null, pcr);
                    owner.SetPropertyValueCore(propertyName, pcr.CachedValue, null);
                };

                rejectCallbacks.Add(propertyName, callback);

                d = callback;
            }

            return (RejectCallback<T>)d;
        }

        ChangeTrackingRegistration registration = ChangeTrackingRegistration.AsPersistent;
        IChangeTrackingService _memento;

        /// <summary>
        /// Gets or sets the change tracking service to use as memento
        /// features provider.
        /// </summary>
        /// <value>The change tracking service.</value>
        [Bindable(BindableSupport.No)]
        IChangeTrackingService IMemento.Memento
        {
            get
            {
                EnsureNotDisposed();
                return _memento;
            }
            set
            {
                EnsureNotDisposed();
                if (value != _memento)
                {
                    var old = ((IMemento)this).Memento;
                    _memento = value;
                    if (registration == ChangeTrackingRegistration.AsTransient && IsTracking)
                    {
                        OnRegisterTransient(TransientRegistration.AsTransparent);
                    }

                    OnMementoChanged(value, old);
                }
            }
        }

        /// <summary>
        /// Gets the chenge tracking service.
        /// </summary>
        /// <returns>The current change tracking service, if any; otherwise null.</returns>
        protected IChangeTrackingService GetTrackingService()
        {
            return ((IMemento)this).Memento;
        }

        /// <summary>
        /// Registers this instance as transient.
        /// </summary>
        protected void RegisterTransient()
        {
            registration = ChangeTrackingRegistration.AsTransient;
        }

        /// <summary>
        /// Called in order to register this instance as transient.
        /// </summary>
        protected virtual void OnRegisterTransient(TransientRegistration transientRegistration)
        {
            var autoRemove = transientRegistration == TransientRegistration.AsTransparent;
            ((IMemento)this).Memento.RegisterTransient(this, autoRemove);
        }

        /// <summary>
        /// Called when the <see cref="IChangeTrackingService"/> changes.
        /// </summary>
        /// <param name="newMemento">The new memento service.</param>
        /// <param name="oldMemento">The old memmento service.</param>
        protected virtual void OnMementoChanged(IChangeTrackingService newMemento, IChangeTrackingService oldMemento)
        {

        }

        /// <summary>
        /// Gets a item indicating whether there is an active change tracking service.
        /// </summary>
        /// <value>
        ///     <c>true</c> if there is an active change tracking service; otherwise, <c>false</c>.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected virtual bool IsTracking
        {
            get
            {
                EnsureNotDisposed();
                return ((IMemento)this).Memento != null && !((IMemento)this).Memento.IsSuspended;
            }
        }

        /// <summary>
        /// Caches the supplied item in the active change tracking service.
        /// </summary>
        /// <typeparam name="T">The system type of the item to cache.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="restore">A delegate to call when the change tracking 
        /// service needs to restore the cached change.</param>
        /// <returns>A reference to the cached change as an instance of <see cref="IChange"/> interface.</returns>
        protected IChange CacheChange<T>(string propertyName, T value, RejectCallback<T> restore)
        {
            EnsureNotDisposed();
            return CacheChange<T>(propertyName, value, restore, null, AddChangeBehavior.Default);
        }

        /// <summary>
        /// Caches the supplied item in the active change tracking service.
        /// </summary>
        /// <typeparam name="T">The system type of the item to cache.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
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
        protected IChange CacheChange<T>(string propertyName, T value, RejectCallback<T> restore, CommitCallback<T> commit)
        {
            EnsureNotDisposed();
            return CacheChange<T>(propertyName, value, restore, commit, AddChangeBehavior.Default);
        }

        /// <summary>
        /// Caches the supplied item in the active change tracking service.
        /// </summary>
        /// <typeparam name="T">The system type of the item to cache.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="restore">A delegate to call when the change tracking
        /// service needs to restore the cached change.</param>
        /// <param name="commit">A delegate to call when the change tracking
        /// service needs to commit the cached change. Passing a null item for
        /// this parameter means that this instance does not need to be notified when
        /// the change is committed.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>
        /// A reference to the cached change as an instance of <see cref="IChange"/> interface.
        /// </returns>
        protected IChange CacheChange<T>(string propertyName, T value, RejectCallback<T> restore, CommitCallback<T> commit, AddChangeBehavior direction)
        {
            EnsureNotDisposed();
            if (IsTracking)
            {
                IChange iChange = new PropertyValueChange<T>(this, propertyName, value, restore, commit, string.Empty);

                ((IMemento)this).Memento.Add(iChange, direction);

                return iChange;
            }

            return null;
        }

        protected virtual IChange CacheChangeOnRejectCallback<T>(string propertyName, T value, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, ChangeRejectedEventArgs<T> args)
        {
            EnsureNotDisposed();
            switch (args.Reason)
            {
                case RejectReason.Undo:
                    return CacheChange(propertyName, value, rejectCallback, commitCallback, AddChangeBehavior.UndoRequest);

                case RejectReason.Redo:
                    return CacheChange(propertyName, value, rejectCallback, commitCallback, AddChangeBehavior.RedoRequest);

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
