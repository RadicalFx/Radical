using Radical.Linq;
using Radical.Validation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Radical.Model
{
    /// <summary>
    /// Abstract base class that holds metadata for a single property, including its name,
    /// default value, change-notification settings, and cascade change notification targets.
    /// </summary>
    public abstract class PropertyMetadata : IDisposable
    {

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="PropertyMetadata"/> is reclaimed by garbage collection.
        /// </summary>
        ~PropertyMetadata()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                cascadeChangeNotifications.Clear();
            }
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
        /// Creates the meta-data for specified property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyOwner">The property owner.</param>
        /// <param name="property">The property.</param>
        /// <returns>
        /// An instance of the property meta-data.
        /// </returns>
        public static PropertyMetadata<T> Create<T>(object propertyOwner, Expression<Func<T>> property)
        {
            var name = property.GetMemberName();
            return Create<T>(propertyOwner, name);
        }

        /// <summary>
        /// Creates the meta-data for specified property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyOwner">The property owner.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// An instance of the property meta-data.
        /// </returns>
        public static PropertyMetadata<T> Create<T>(object propertyOwner, string propertyName)
        {
            return new PropertyMetadata<T>(propertyOwner, propertyName);
        }

        readonly HashSet<string> cascadeChangeNotifications = new HashSet<string>();

        readonly object propertyOwner;
        PropertyInfo _property;

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> for the property represented by this metadata,
        /// resolved lazily from the owning object's type.
        /// </summary>
        protected PropertyInfo Property
        {
            get
            {
                if (_property == null)
                {
                    _property = propertyOwner
                        .GetType()
                        .GetProperty(PropertyName);
                }

                return _property;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata" /> class.
        /// </summary>
        /// <param name="propertyOwner">The property owner.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected PropertyMetadata(object propertyOwner, string propertyName)
        {
            Ensure.That(propertyOwner).Named("propertyOwner").IsNotNull();
            Ensure.That(propertyName).Named("propertyName").IsNotNullNorEmpty();

            this.propertyOwner = propertyOwner;
            PropertyName = propertyName;
            NotifyChanges = true;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property represented by this meta-data should notify changes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the property should notify changes; otherwise, <c>false</c>.
        /// </value>
        public bool NotifyChanges { get; set; }

        /// <summary>
        /// Disables changes notifications for this property.
        /// </summary>
        /// <returns>This meta-data instance.</returns>
        public PropertyMetadata DisableChangesNotifications()
        {
            NotifyChanges = false;
            return this;
        }

        /// <summary>
        /// Enables changes notifications for this property.
        /// </summary>
        /// <returns>This meta-data instance.</returns>
        public PropertyMetadata EnableChangesNotifications()
        {
            NotifyChanges = true;
            return this;
        }

        /// <summary>
        /// Registers the property identified by the given expression to receive a change notification
        /// whenever this property changes.
        /// </summary>
        /// <typeparam name="T">The type of the cascaded property value.</typeparam>
        /// <param name="property">A lambda expression identifying the property to cascade notifications to.</param>
        /// <returns>This metadata instance.</returns>
        public PropertyMetadata AddCascadeChangeNotifications<T>(Expression<Func<T>> property)
        {
            return AddCascadeChangeNotifications(property.GetMemberName());
        }

        /// <summary>
        /// Registers the named property to receive a change notification whenever this property changes.
        /// </summary>
        /// <param name="property">The name of the property to cascade notifications to.</param>
        /// <returns>This metadata instance.</returns>
        public PropertyMetadata AddCascadeChangeNotifications(string property)
        {
            cascadeChangeNotifications.Add(property);

            return this;
        }

        /// <summary>
        /// Unregisters the property identified by the given expression from receiving cascade
        /// change notifications when this property changes.
        /// </summary>
        /// <typeparam name="T">The type of the cascaded property value.</typeparam>
        /// <param name="property">A lambda expression identifying the property to stop cascading notifications to.</param>
        /// <returns>This metadata instance.</returns>
        public PropertyMetadata RemoveCascadeChangeNotifications<T>(Expression<Func<T>> property)
        {
            return RemoveCascadeChangeNotifications(property.GetMemberName());
        }

        /// <summary>
        /// Unregisters the named property from receiving cascade change notifications when this property changes.
        /// </summary>
        /// <param name="property">The name of the property to stop cascading notifications to.</param>
        /// <returns>This metadata instance.</returns>
        public PropertyMetadata RemoveCascadeChangeNotifications(string property)
        {
            if (cascadeChangeNotifications.Contains(property))
            {
                cascadeChangeNotifications.Remove(property);
            }

            return this;
        }

        /// <summary>
        /// Returns the set of property names that should receive a change notification
        /// whenever this property changes.
        /// </summary>
        /// <returns>An enumerable of property names registered for cascade change notifications.</returns>
        public IEnumerable<string> GetCascadeChangeNotifications()
        {
            return cascadeChangeNotifications;
        }

        /// <summary>
        /// Sets the default value for this property from a boxed <see cref="PropertyValue"/> instance.
        /// </summary>
        /// <param name="value">The boxed property value to use as the default.</param>
        public abstract void SetDefaultValue(PropertyValue value);

        /// <summary>
        /// Gets the current default value for this property as a boxed <see cref="PropertyValue"/> instance.
        /// </summary>
        /// <returns>The boxed default value for this property.</returns>
        public abstract PropertyValue GetDefaultValue();
    }
}
