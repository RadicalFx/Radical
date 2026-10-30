using Radical.ComponentModel.ChangeTracking;
using Radical.Linq;
using Radical.Reflection;
using System;
using System.Linq.Expressions;

namespace Radical.Model
{
    /// <summary>
    /// Provides factory methods for creating <see cref="MementoPropertyMetadata{T}"/> instances.
    /// </summary>
    public static class MementoPropertyMetadata
    {
        /// <summary>
        /// Creates a new <see cref="MementoPropertyMetadata{T}"/> for the specified property expression.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyOwner">The object that owns the property.</param>
        /// <param name="property">A lambda expression identifying the property.</param>
        /// <returns>A new <see cref="MementoPropertyMetadata{T}"/> instance for the specified property.</returns>
        public static MementoPropertyMetadata<T> Create<T>(object propertyOwner, Expression<Func<T>> property)
        {
            return new MementoPropertyMetadata<T>(propertyOwner, property);
        }

        /// <summary>
        /// Creates a new <see cref="MementoPropertyMetadata{T}"/> for the specified property name.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyOwner">The object that owns the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>A new <see cref="MementoPropertyMetadata{T}"/> instance for the specified property.</returns>
        public static MementoPropertyMetadata<T> Create<T>(object propertyOwner, string propertyName)
        {
            return new MementoPropertyMetadata<T>(propertyOwner, propertyName);
        }
    }

    /// <summary>
    /// Extends <see cref="PropertyMetadata{T}"/> with change-tracking support, allowing
    /// individual properties to opt in or out of being tracked by an <c>IChangeTrackingService</c>.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class MementoPropertyMetadata<T> : PropertyMetadata<T>,
        IMementoPropertyMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MementoPropertyMetadata{T}"/> class
        /// for the property identified by name. If the property is decorated with a
        /// <see cref="MementoPropertyMetadataAttribute"/>, its <c>TrackChanges</c> setting is applied;
        /// otherwise change tracking is enabled by default.
        /// </summary>
        /// <param name="propertyOwner">The object that owns the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        public MementoPropertyMetadata(object propertyOwner, string propertyName)
            : base(propertyOwner, propertyName)
        {
            if (Property.IsAttributeDefined<MementoPropertyMetadataAttribute>())
            {
                var attribute = Property.GetAttribute<MementoPropertyMetadataAttribute>();
                TrackChanges = attribute.TrackChanges;
            }
            else
            {
                TrackChanges = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoPropertyMetadata{T}"/> class
        /// for the property identified by a lambda expression.
        /// </summary>
        /// <param name="propertyOwner">The object that owns the property.</param>
        /// <param name="property">A lambda expression identifying the property.</param>
        public MementoPropertyMetadata(object propertyOwner, Expression<Func<T>> property)
            : this(propertyOwner, property.GetMemberName())
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether changes to this property are tracked
        /// by the associated change tracking service.
        /// </summary>
        /// <value><c>true</c> if changes are tracked; otherwise, <c>false</c>.</value>
        public bool TrackChanges { get; set; }

        /// <summary>
        /// Disables change tracking for this property.
        /// </summary>
        /// <returns>This metadata instance.</returns>
        public MementoPropertyMetadata<T> DisableChangesTracking()
        {
            TrackChanges = false;
            return this;
        }

        /// <summary>
        /// Enables change tracking for this property.
        /// </summary>
        /// <returns>This metadata instance.</returns>
        public MementoPropertyMetadata<T> EnableChangesTracking()
        {
            TrackChanges = true;
            return this;
        }
    }

    /// <summary>
    /// Attribute used to configure change-tracking behavior for a property backed by
    /// <see cref="MementoPropertyMetadata{T}"/>. Apply this attribute to a property to
    /// control whether its changes are tracked by the change tracking service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MementoPropertyMetadataAttribute : PropertyMetadataAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MementoPropertyMetadataAttribute"/> class
        /// with change tracking enabled by default.
        /// </summary>
        public MementoPropertyMetadataAttribute()
        {
            TrackChanges = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether changes to the decorated property are tracked
        /// by the associated change tracking service.
        /// </summary>
        /// <value><c>true</c> if changes are tracked; otherwise, <c>false</c>. Defaults to <c>true</c>.</value>
        public bool TrackChanges { get; set; }
    }
}
