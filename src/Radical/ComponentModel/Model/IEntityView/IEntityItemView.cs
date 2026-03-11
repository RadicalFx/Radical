namespace Radical.ComponentModel
{
    using System.ComponentModel;

    /// <summary>
    /// Identifies an item in an <see cref="IEntityView"/> object.
    /// </summary>
    public interface IEntityItemView :
        ICustomTypeDescriptor,
        INotifyEditableObject,
        INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the underlying entity item.
        /// </summary>
        /// <value>The underlying entity item.</value>
        object EntityItem { get; }

        /// <summary>
        /// Deletes this IEntityItemView and removes if from the view and from the underlying collection.
        /// </summary>
        void Delete();

        /// <summary>
        /// Gets a reference the view that owns this instance.
        /// </summary>
        /// <value>The owner view.</value>
        IEntityView View { get; }

        /// <summary>
        /// Sets a custom property value on this item view using the specified property name.
        /// </summary>
        /// <typeparam name="TValue">The type of the custom property value.</typeparam>
        /// <param name="customPropertyName">The name of the custom property.</param>
        /// <param name="value">The value to set.</param>
        void SetCustomValue<TValue>(string customPropertyName, TValue value);

        /// <summary>
        /// Gets the value of the custom property identified by the specified property name.
        /// </summary>
        /// <typeparam name="TValue">The type of the custom property value.</typeparam>
        /// <param name="customPropertyName">The name of the custom property.</param>
        /// <returns>The value of the custom property.</returns>
        TValue GetCustomValue<TValue>(string customPropertyName);

        /// <summary>
        /// Raises the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event
        /// for the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        void NotifyPropertyChanged(string propertyName);
    }

}
