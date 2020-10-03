namespace Radical.ComponentModel
{
    /// <summary>
    /// Delegate used to get property value
    /// </summary>
    public delegate object EntityItemViewValueGetter<T, TValue>(EntityItemViewValueGetterArgs<T, TValue> args);

    /// <summary>
    /// Delegate used to set property value
    /// </summary>
    public delegate void EntityItemViewValueSetter<T, TValue>(EntityItemViewValueSetterArgs<T, TValue> args);

    public abstract class EntityItemViewValueArgs<T>
    {
        /// <summary>
        /// EntityItemViewValueArgs constructor.
        /// </summary>
        /// <param name="item">The entity item view.</param>
        /// <param name="propertyName">The property name.</param>
        protected EntityItemViewValueArgs(IEntityItemView<T> item, string propertyName)
        {
            Item = item;
            PropertyName = propertyName;
        }

        /// <summary>
        /// The entity item view item.
        /// </summary>
        public IEntityItemView<T> Item { get; }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string PropertyName { get; }
    }
    public class EntityItemViewValueGetterArgs<T, TValue> : EntityItemViewValueArgs<T>
    {
        /// <summary>
        /// EntityItemViewValueArgs constructor.
        /// </summary>
        /// <param name="item">The entity item view.</param>
        /// <param name="propertyName">The property name.</param>
        public EntityItemViewValueGetterArgs(IEntityItemView<T> item, string propertyName)
            : base(item, propertyName)
        {

        }
    }

    public class EntityItemViewValueSetterArgs<T, TValue> : EntityItemViewValueArgs<T>
    {
        /// <summary>
        /// EntityItemViewValueArgs constructor.
        /// </summary>
        /// <param name="item">The entity item view.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The set value.</param>
        public EntityItemViewValueSetterArgs(IEntityItemView<T> item, string propertyName, TValue value)
            : base(item, propertyName)
        {
            Value = value;
        }

        /// <summary>
        /// The value stored in the setter.
        /// </summary>
        public TValue Value { get; }
    }
}
