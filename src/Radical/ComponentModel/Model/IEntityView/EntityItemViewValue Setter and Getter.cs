namespace Radical.ComponentModel
{
    /// <summary>
    /// Delegate used to get property value
    /// </summary>
    /// <typeparam name="T">The type of the object wrapped by the entity item view.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <param name="args">The arguments describing the value to get.</param>
    /// <returns>The value of the property.</returns>
    public delegate object EntityItemViewValueGetter<T, TValue>(EntityItemViewValueGetterArgs<T> args);

    /// <summary>
    /// Delegate used to set property value
    /// </summary>
    /// <typeparam name="T">The type of the object wrapped by the entity item view.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <param name="args">The arguments describing the value to set.</param>
    public delegate void EntityItemViewValueSetter<T, TValue>(EntityItemViewValueSetterArgs<T, TValue> args);

    /// <summary>
    /// Base class for argument for an EntityItemViewValueGetter or EntityItemViewValueSetter
    /// </summary>
    /// <typeparam name="T">The type of the object wrapped by the entity item view.</typeparam>
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
    
    /// <summary>
    /// Argument for an EntityItemViewValueGetter
    /// </summary>
    /// <typeparam name="T">The type of the object wrapped by the entity item view.</typeparam>
    public class EntityItemViewValueGetterArgs<T> : EntityItemViewValueArgs<T>
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

    /// <summary>
    /// Argument for an EntityItemViewValueSetter
    /// </summary>
    /// <typeparam name="T">The type of the object wrapped by the entity item view.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
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
