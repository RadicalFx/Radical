using Radical.Linq;
using Radical.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Radical.Model
{
    [Serializable]
    public abstract class Entity :
        INotifyPropertyChanged,
        IDisposable
    {

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Entity"/> is reclaimed by garbage collection.
        /// </summary>
        ~Entity()
        {
            Dispose(false);
        }

        private bool isDisposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_events != null)
                {
                    Events.Dispose();
                }

                valuesBag.Clear();

                foreach (var item in propertiesMetadata.Values)
                {
                    item.Dispose();
                }

                propertiesMetadata.Clear();
            }

            _events = null;
            isDisposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Verifies that this instance is not disposed, throwing an
        /// <see cref="ObjectDisposedException"/> if this instance has
        /// been already disposed.
        /// </summary>
        protected virtual void EnsureNotDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }



        [NonSerialized]
        private EventHandlerList _events;

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <item>The events.</item>
        protected EventHandlerList Events
        {
            get
            {
                if (_events == null)
                {
                    _events = new EventHandlerList();
                }

                return _events;
            }
        }



        private static readonly object propertyChangedEventKey = new object();
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { Events.AddHandler(propertyChangedEventKey, value); }
            remove { Events.RemoveHandler(propertyChangedEventKey, value); }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            EnsureNotDisposed();
            (Events[propertyChangedEventKey] as PropertyChangedEventHandler)?.Invoke(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(property.GetMemberName()));
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity()
        {

        }

        /// <summary>
        /// Sets the initial property value building default
        /// meta-data for the given property and setting the default
        /// value in the built meta-data.
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="value">The default value.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is a shortcut for the GetMetadata method, in order
        /// to fine customize property meta-data use the GetMetadata method.
        /// The main difference between SetInitialPropertyValue and SetPropertyValue
        /// is that SetInitialPropertyValue does not raise a property change notification.
        /// </remarks>
        protected PropertyMetadata<T> SetInitialPropertyValue<T>(Expression<Func<T>> property, T value)
        {
            return SetInitialPropertyValue<T>(property.GetMemberName(), value);
        }

        /// <summary>
        /// Sets the initial property value building default
        /// meta-data for the given property and setting the default
        /// value in the built meta-data. With this overload the default
        /// value is lazily evaluated only when requested by the infrastructure.
        /// </summary>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="lazyValue">The lazy value.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is a shortcut for the GetMetadata method, in order
        /// to fine customize property meta-data use the GetMetadata method.
        /// The main difference between SetInitialPropertyValue and SetPropertyValue
        /// is that SetInitialPropertyValue does not raise a property change notification.
        /// </remarks>
        protected PropertyMetadata<T> SetInitialPropertyValue<T>(Expression<Func<T>> property, Func<T> lazyValue)
        {
            var metadata = GetPropertyMetadata<T>(property)
                .WithDefaultValue(lazyValue);

            return metadata;
        }

        /// <summary>
        /// Sets the initial property value building default 
        /// meta-data for the given property and setting the default
        /// value in the built meta-data.
        /// </summary>
        /// <remarks>
        /// This method is a shortcut for the GetMetadata method, in order
        /// to fine customize property meta-data use the GetMetadata method.
        /// The main difference between SetInitialPropertyValue and SetPropertyValue 
        /// is that SetInitialPropertyValue does not raise a property change notification.
        /// </remarks>
        /// <typeparam name="T">The property type</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="value">The default value.</param>
        protected PropertyMetadata<T> SetInitialPropertyValue<T>(string property, T value)
        {
            var metadata = GetPropertyMetadata<T>(property)
                .WithDefaultValue(value);

            return metadata;
        }

        readonly IDictionary<string, PropertyMetadata> propertiesMetadata = new Dictionary<string, PropertyMetadata>();

        /// <summary>
        /// Gets the property meta-data.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>
        /// An instance of the requested property meta-data.
        /// </returns>
        protected PropertyMetadata<T> GetPropertyMetadata<T>(Expression<Func<T>> property)
        {
            Ensure.That(property).Named("property").IsNotNull();

            return GetPropertyMetadata<T>(property.GetMemberName());
        }

        /// <summary>
        /// Gets the property meta-data.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>An instance of the requested property metadata.</returns>
        protected PropertyMetadata<T> GetPropertyMetadata<T>(string propertyName)
        {
            Ensure.That(propertyName).Named("propertyName").IsNotNullNorEmpty();

            if (!propertiesMetadata.TryGetValue(propertyName, out PropertyMetadata md))
            {
                md = GetDefaultMetadata<T>(propertyName);
                propertiesMetadata.Add(propertyName, md);
            }

            return (PropertyMetadata<T>)md;
        }

        /// <summary>
        /// Gets the default property meta-data.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>An instance of the requested default property meta-data.</returns>
        protected virtual PropertyMetadata<T> GetDefaultMetadata<T>(string propertyName)
        {
            return PropertyMetadata.Create<T>(this, propertyName);
        }

        /// <summary>
        /// Sets the property meta-data.
        /// </summary>
        /// <param name="metadata">The property meta-data.</param>
        protected virtual void SetPropertyMetadata<T>(PropertyMetadata<T> metadata)
        {
            Ensure.That(metadata).Named("metadata").IsNotNull();

            Ensure.That(propertiesMetadata)
                .WithMessage("Meta-data for the supplied property ({0}) has already been set.", metadata.PropertyName)
                .IsFalse(d => d.ContainsKey(metadata.PropertyName));

            propertiesMetadata.Add(metadata.PropertyName, metadata);
        }

        class NotificationSuspension<T> : IDisposable
        {
            readonly PropertyMetadata<T> property;
            
            public NotificationSuspension(PropertyMetadata<T> property)
            {
                this.property = property;
            }

            public void Dispose()
            {
                property.EnableChangesNotifications();
            }
        }

        /// <summary>
        /// Suspends notifications for the given property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        protected IDisposable SuspendNotificationsOf<T>(Expression<Func<T>> property)
        {
            var md = GetPropertyMetadata(property);
            md.DisableChangesNotifications();

            return new NotificationSuspension<T>(md);
        }

        /// <summary>
        /// Resumes notifications for the given property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        protected void ResumeNotificationsFor<T>(Expression<Func<T>> property)
        {
            var md = GetPropertyMetadata(property);
            md.EnableChangesNotifications();
        }

        /// <summary>
        /// Determines whether meta-data for the specified property has been set.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>
        ///     <c>true</c> if meta-data for the specified property has been set; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool HasMetadata<T>(Expression<Func<T>> property)
        {
            return propertiesMetadata.ContainsKey(property.GetMemberName());
        }

        readonly IDictionary<string, PropertyValue> valuesBag = new Dictionary<string, PropertyValue>();

        protected void SetPropertyValueCore<T>(string propertyName, T data, PropertyValueChanged<T> pvc)
        {
            var oldValue = GetPropertyValue<T>(propertyName);

            if (!Equals(oldValue, data))
            {
                if (valuesBag.ContainsKey(propertyName))
                {
                    valuesBag[propertyName] = new PropertyValue<T>(data);
                }
                else
                {
                    valuesBag.Add(propertyName, new PropertyValue<T>(data));
                }

                var args = new PropertyValueChangedArgs<T>(data, oldValue);
                pvc?.Invoke(args);

                var metadata = GetPropertyMetadata<T>(propertyName);
                if (metadata.NotifyChanges)
                {
                    OnPropertyChanged(propertyName);

                    metadata
                        .NotifyChanged(args)
                        .GetCascadeChangeNotifications()
                        .ForEach(s =>
                        {
                            OnPropertyChanged(s);
                        });
                }
            }
        }

        protected void SetPropertyValue<T>(Expression<Func<T>> property, T data)
        {
            var propertyName = property.GetMemberName();

            SetPropertyValue(propertyName, data, null);
        }

        protected void SetPropertyValue<T>(Expression<Func<T>> property, T data, PropertyValueChanged<T> pvc)
        {
            var propertyName = property.GetMemberName();

            SetPropertyValue(propertyName, data, pvc);
        }

        protected virtual void SetPropertyValue<T>([CallerMemberName]string propertyName = null, T data = default, PropertyValueChanged<T> pvc = default)
        {
            Ensure.That(propertyName).Named(nameof(propertyName)).IsNotNullNorEmpty();

            SetPropertyValueCore(propertyName, data, pvc);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="property">A Lambda Expression representing the property.</param>
        /// <returns>The requested property value.</returns>
        protected T GetPropertyValue<T>(Expression<Func<T>> property)
        {
            return GetPropertyValue<T>(property.GetMemberName());
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="property">A Lambda Expression representing the property.</param>
        /// <param name="initialValueSetter">The initial value setter.</param>
        /// <returns>The requested property value.</returns>
        protected T GetPropertyValue<T>(Expression<Func<T>> property, Func<T> initialValueSetter)
        {
            if (!HasMetadata(property) && initialValueSetter != null)
            {
                var initialValue = initialValueSetter();

                SetInitialPropertyValue(property, initialValue);
            }

            return GetPropertyValue<T>(property.GetMemberName());
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="T">The property value type.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The requested property value.</returns>
        protected internal virtual T GetPropertyValue<T>([CallerMemberName]string propertyName = null)
        {
            Ensure.That(propertyName).Named(nameof(propertyName)).IsNotNullNorEmpty();

            if (valuesBag.TryGetValue(propertyName, out PropertyValue actual))
            {
                return ((PropertyValue<T>)actual).Value;
            }

            var metadata = GetPropertyMetadata<T>(propertyName);
            return metadata.DefaultValue;
        }
    }
}
