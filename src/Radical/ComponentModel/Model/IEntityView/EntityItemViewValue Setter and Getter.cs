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

    public abstract class EntityItemViewValueArgs<T, TValue>
    {
        protected EntityItemViewValueArgs(IEntityItemView<T> item, string propertyName)
        {
            Item = item;
            PropertyName = propertyName;
        }

        public IEntityItemView<T> Item
        {
            get;
            private set;
        }

        public string PropertyName
        {
            get;
            private set;
        }
    }
    public class EntityItemViewValueGetterArgs<T, TValue> : EntityItemViewValueArgs<T, TValue>
    {
        public EntityItemViewValueGetterArgs(IEntityItemView<T> item, string propertyName)
            : base(item, propertyName)
        {

        }
    }

    public class EntityItemViewValueSetterArgs<T, TValue> : EntityItemViewValueArgs<T, TValue>
    {
        public EntityItemViewValueSetterArgs(IEntityItemView<T> item, string propertyName, TValue value)
            : base(item, propertyName)
        {
            Value = value;
        }

        public TValue Value
        {
            get;
            private set;
        }
    }
}
