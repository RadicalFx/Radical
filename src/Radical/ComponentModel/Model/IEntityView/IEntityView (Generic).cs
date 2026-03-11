using Radical.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Extends the <see cref="IEntityView"/> interface by providing
    /// advanced features. This <c>IEntityView</c> is capable of managing custom properties added at runtime
    /// in order to build, for example, runtime evaluated columns dynamically.
    /// </summary>
    /// <typeparam name="T">The type (System.Type) of objects managed by this collection view.</typeparam>
    public interface IEntityView<T> :
        IEntityView,
        IEnumerable<IEntityItemView<T>>
    //where T : class
    {
        /// <summary>
        /// Adds a property mapping using the specified display name, the supplied property type and the supplied getter and setter.
        /// </summary>
        /// <param name="calculatedPropertyDisplayName">The display name of the new property.</param>
        /// <param name="getter">A delegate to call in order to get the value of the dynamically generated property.</param>
        /// <param name="setter">A delegate to call in order to set the value of the dynamically generated property.</param>
        /// <returns>A reference to the dynamically generated property.</returns>
        EntityItemViewPropertyDescriptor<T> AddCustomProperty<TProperty>(
            string calculatedPropertyDisplayName,
            EntityItemViewValueGetter<T, TProperty> getter,
            EntityItemViewValueSetter<T, TProperty> setter);

        /// <summary>
        /// Adds a property mapping using the specified display name, the supplied property type and the supplied getter.
        /// </summary>
        /// <param name="calculatedPropertyDisplayName">Calculated name of the property display.</param>
        /// <param name="getter">A delegate to call in order to get the value of the dynamically generated property.</param>
        /// <returns>A reference to the dynamically generated property.</returns>
        /// <remarks>Using this overload implicitly creates a read-only property because no setter has been supplied.</remarks>
        EntityItemViewPropertyDescriptor<T> AddCustomProperty<TProperty>(string calculatedPropertyDisplayName,
            EntityItemViewValueGetter<T, TProperty> getter);

        /// <summary>
        /// Adds a property mapping using the specified display name, the supplied getter, and a default value interceptor.
        /// </summary>
        /// <param name="calculatedPropertyDisplayName">The display name of the new property.</param>
        /// <param name="getter">A delegate to call in order to get the value of the dynamically generated property.</param>
        /// <param name="defaultValueInterceptor">A delegate that supplies the default value for the property.</param>
        /// <returns>A reference to the dynamically generated property.</returns>
        EntityItemViewPropertyDescriptor<T> AddCustomProperty<TProperty>(string calculatedPropertyDisplayName,
            EntityItemViewValueGetter<T, TProperty> getter,
            Func<TProperty> defaultValueInterceptor);

        /// <summary>
        /// Adds a property mapping that maps a property using the supplied display name.
        /// </summary>
        /// <param name="propertyName">Name of the property to map to.</param>
        /// <param name="displayName">The display name.</param>
        /// <returns>A reference to the dynamically generated property.</returns>
        EntityItemViewPropertyDescriptor<T> AddCustomProperty(string propertyName, string displayName);

        /// <summary>
        /// Adds a property mapping using the supplied property descriptor instance.
        /// </summary>
        /// <param name="customProperty">The custom property descriptor to use as mapping.</param>
        /// <returns>A reference to the dynamically generated property.</returns>
        EntityItemViewPropertyDescriptor<T> AddCustomProperty(EntityItemViewPropertyDescriptor<T> customProperty);

        /// <summary>
        /// Adds a property mapping that maps the supplied property.
        /// </summary>
        /// <param name="propertyName">Name of the property to map to.</param>
        /// <returns>A reference to the dynamically generated property.</returns>
        EntityItemViewPropertyDescriptor<T> AddCustomProperty(string propertyName);

        /// <summary>
        /// Determines whether a custom property with the specified name has been defined.
        /// </summary>
        /// <param name="propertyName">The name of the custom property to check.</param>
        /// <returns><c>true</c> if a custom property with the given name exists; otherwise, <c>false</c>.</returns>
        bool IsCustomPropertyDefined(string propertyName);

        /// <summary>
        /// Removes the property mapping.
        /// </summary>
        /// <param name="customProperty">The custom property.</param>
        /// <returns><c>True</c> if the operation was successful, otherwise <c>false</c>.</returns>
        bool RemoveCustomProperty(EntityItemViewPropertyDescriptor<T> customProperty);

        /// <summary>
        /// Removes the property mapping.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>True</c> if the operation was successful, otherwise <c>false</c>.</returns>
        bool RemoveCustomProperty(string propertyName);

        /// <summary>
        /// Gets or sets a value indicating whether this instance must auto generate property mappings.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance must auto generate property mappings; otherwise, <c>false</c>.
        /// </value>
        bool AutoGenerateProperties { get; set; }

        /// <summary>
        /// Gets the all the dynamically added custom property mappings.
        /// </summary>
        /// <returns>A read-only list of dynamically added property mappings.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<EntityItemViewPropertyDescriptor<T>> GetCustomProperties();

        /// <summary>
        /// Returns the custom property descriptor with the specified name.
        /// </summary>
        /// <param name="name">The name of the custom property.</param>
        /// <returns>The matching <see cref="EntityItemViewPropertyDescriptor{T}"/>, or <c>null</c> if not found.</returns>
        EntityItemViewPropertyDescriptor<T> GetCustomProperty(string name);

        /// <summary>
        /// Returns the property descriptor with the specified name, including both native and custom properties.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The matching <see cref="PropertyDescriptor"/>, or <c>null</c> if not found.</returns>
        PropertyDescriptor GetProperty(string name);

        /// <summary>
        /// Gets or sets the filter to be used to exclude items from the collection of items returned by the data source
        /// </summary>
        /// <returns>The filter used to filter out in the item collection returned by the data source. </returns>
        new IEntityItemViewFilter<T> Filter { get; set; }

        /// <summary>
        /// Moves the specified item to the specified new index.
        /// </summary>
        /// <param name="item">The item to move.</param>
        /// <param name="newIndex">The destination index.</param>
        void Move(IEntityItemView<T> item, int newIndex);

        /// <summary>
        /// Sorts the view items using the specified comparer.
        /// </summary>
        /// <param name="comparer">The comparer used to order the items.</param>
        void ApplySort(IComparer<IEntityItemView<T>> comparer);

        /// <summary>
        /// Applies the given filter.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        void ApplyFilter(Predicate<T> predicate);

        /// <summary>
        /// Gets the value of the specified custom property for the given item.
        /// </summary>
        /// <typeparam name="TValue">The type of the custom property value.</typeparam>
        /// <param name="customPropertyName">The name of the custom property.</param>
        /// <param name="item">The item whose custom property value is retrieved.</param>
        /// <returns>The current value of the custom property for the specified item.</returns>
        TValue GetCustomPropertyValue<TValue>(string customPropertyName, IEntityItemView<T> item);

        /// <summary>
        /// Sets the value of the specified custom property for the given item.
        /// </summary>
        /// <typeparam name="TValue">The type of the custom property value.</typeparam>
        /// <param name="customPropertyName">The name of the custom property.</param>
        /// <param name="item">The item whose custom property value is set.</param>
        /// <param name="value">The new value to assign to the custom property.</param>
        void SetCustomPropertyValue<TValue>(string customPropertyName, IEntityItemView<T> item, TValue value);

        /// <summary>
        /// Occurs before a new item is added to the view, allowing interception or cancellation of the operation.
        /// </summary>
        event EventHandler<AddingNewEventArgs<T>> AddingNew;

        /// <summary>
        /// Adds a new item to the underlying data source and returns a view wrapper for it.
        /// </summary>
        /// <returns>An <see cref="IEntityItemView{T}"/> wrapping the newly added item.</returns>
        new IEntityItemView<T> AddNew();

        /// <summary>
        /// Adds a new item to the underlying data source, using the specified interceptor to configure the operation, and returns a view wrapper for it.
        /// </summary>
        /// <param name="addNewInterceptor">A delegate invoked with the <see cref="AddingNewEventArgs{T}"/> before the item is added.</param>
        /// <returns>An <see cref="IEntityItemView{T}"/> wrapping the newly added item.</returns>
        IEntityItemView<T> AddNew(Action<AddingNewEventArgs<T>> addNewInterceptor);
    }
}
