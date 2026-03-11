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

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewCustomPropertyDescriptor{T, TValue}"/> class
        /// with the specified property name and value getter.
        /// </summary>
        /// <param name="customPropertyName">The name of the custom property.</param>
        /// <param name="getter">The delegate used to get the property value.</param>
        public EntityItemViewCustomPropertyDescriptor(string customPropertyName, EntityItemViewValueGetter<T, TValue> getter)
            : this(customPropertyName, getter, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewCustomPropertyDescriptor{T, TValue}"/> class
        /// with the specified property name, value getter, and value setter.
        /// </summary>
        /// <param name="customPropertyName">The name of the custom property.</param>
        /// <param name="getter">The delegate used to get the property value.</param>
        /// <param name="setter">The delegate used to set the property value.</param>
        public EntityItemViewCustomPropertyDescriptor(string customPropertyName, EntityItemViewValueGetter<T, TValue> getter, EntityItemViewValueSetter<T, TValue> setter)
            : this(customPropertyName)
        {
            ValueGetter = getter;
            ValueSetter = setter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewCustomPropertyDescriptor{T, TValue}"/> class
        /// with the specified display name.
        /// </summary>
        /// <param name="customDisplayName">The display name of the custom property.</param>
        public EntityItemViewCustomPropertyDescriptor(string customDisplayName)
            : base()
        {
            _customDisplayName = customDisplayName;
        }

        readonly string _customDisplayName;

        /// <summary>
        /// Gets the name that can be displayed in a window, such as a Properties window.
        /// </summary>
        /// <value></value>
        /// <returns>The name to display for the member.</returns>
        public override string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets the name of the custom property.
        /// </summary>
        /// <returns>The name of the custom property.</returns>
        public override string Name
        {
            get { return _customDisplayName; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="System.Type"/> that represents the type of the property.</returns>
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

        /// <summary>
        /// Gets the value of the property for the specified component by invoking the value getter delegate.
        /// </summary>
        /// <param name="component">The <see cref="IEntityItemView{T}"/> that owns the property.</param>
        /// <returns>The value of the property for the given component.</returns>
        protected override object GetValueCore(IEntityItemView<T> component)
        {
            var args = new EntityItemViewValueGetterArgs<T, TValue>(component, Name);
            var returnValue = ValueGetter(args);

            return returnValue;
        }

        /// <summary>
        /// Sets the value of the property for the specified component by invoking the value setter delegate.
        /// </summary>
        /// <param name="component">The <see cref="IEntityItemView{T}"/> that owns the property.</param>
        /// <param name="value">The new value to assign to the property.</param>
        protected override void SetValueCore(IEntityItemView<T> component, object value)
        {
            var args = new EntityItemViewValueSetterArgs<T, TValue>(component, Name, (TValue)value);
            ValueSetter(args);
        }
    }
}
