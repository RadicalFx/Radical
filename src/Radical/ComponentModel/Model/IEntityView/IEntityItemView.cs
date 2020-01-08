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

        void SetCustomValue<TValue>(string customPropertyName, TValue value);
        TValue GetCustomValue<TValue>(string customPropertyName);

        void NotifyPropertyChanged(string propertyName);
    }

}
