using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// A specialized PropertyDescriptor
    /// </summary>
    /// <typeparam name="T">The type encapsulated by this descriptor</typeparam>
    /// <typeparam name="TValue">The type of the value</typeparam>
    public class EntityItemViewCustomPropertyDescriptor<T, TValue> :
        EntityItemViewPropertyDescriptor<T>
    //where T : class
    {
        /// <summary>
        /// Delegate used to get property value
        /// </summary>
        /// <value>The value getter.</value>
        protected EntityItemViewValueGetter<T, TValue> ValueGetter
        {
            get;
            private set;
        }

        /// <summary>
        /// Delegate used to set property value
        /// </summary>
        /// <value>The value setter.</value>
        protected EntityItemViewValueSetter<T, TValue> ValueSetter
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>
        /// The default value for this property.
        /// </returns>
        public new TValue GetDefaultValue()
        {
            if (DafaultValueInterceptor != null)
            {
                return DafaultValueInterceptor();
            }

            return default(TValue);
        }

        /// <summary>
        /// Gets or sets the default value interceptor.
        /// </summary>
        /// <value>
        /// The default value interceptor.
        /// </value>
        public Func<TValue> DafaultValueInterceptor
        {
            get;
            set;
        }

        public EntityItemViewCustomPropertyDescriptor(string customPropertyName, EntityItemViewValueGetter<T, TValue> getter)
            : this(customPropertyName, getter, null)
        {

        }

        public EntityItemViewCustomPropertyDescriptor(string customPropertyName, EntityItemViewValueGetter<T, TValue> getter, EntityItemViewValueSetter<T, TValue> setter)
            : this(customPropertyName)
        {
            ValueGetter = getter;
            ValueSetter = setter;
        }

        public EntityItemViewCustomPropertyDescriptor(string customDisplayName)
            : base()
        {
            _customDisplayName = customDisplayName;
        }

        string _customDisplayName = null;

        /// <summary>
        /// Gets the name that can be displayed in a window, such as a Properties window.
        /// </summary>
        /// <value></value>
        /// <returns>The name to display for the member.</returns>
        public override string DisplayName
        {
            get { return Name; }
        }

        public override string Name
        {
            get { return _customDisplayName; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get { return typeof(TValue); }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the property is read-only; otherwise, false.</returns>
        public override bool IsReadOnly
        {
            get { return ValueSetter == null; }
        }

        protected override object GetValueCore(IEntityItemView<T> component)
        {
            var args = new EntityItemViewValueGetterArgs<T, TValue>(component, Name);
            var returnValue = ValueGetter(args);

            return returnValue;
        }

        protected override void SetValueCore(IEntityItemView<T> component, object value)
        {
            var args = new EntityItemViewValueSetterArgs<T, TValue>(component, Name, (TValue)value);
            ValueSetter(args);
        }
    }
}
