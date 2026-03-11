using Radical.Linq;
using System;
using System.Linq.Expressions;

namespace Radical.Model
{
    /// <summary>
    /// Provides typed metadata for a property of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    public class PropertyMetadata<T> : PropertyMetadata
    {
        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            DefaultValueInterceptor = null;
            propertyChangedHandler = null;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertyMetadata{T}"/> using the property owner and name.
        /// </summary>
        /// <param name="propertyOwner">The object that owns the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        public PropertyMetadata(object propertyOwner, string propertyName)
            : base(propertyOwner, propertyName)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertyMetadata{T}"/> using a property expression.
        /// </summary>
        /// <param name="propertyOwner">The object that owns the property.</param>
        /// <param name="property">An expression identifying the property.</param>
        public PropertyMetadata(object propertyOwner, Expression<Func<T>> property)
            : this(propertyOwner, property.GetMemberName())
        {

        }

        /// <summary>
        /// Sets the default value for the property and returns this instance for fluent chaining.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>This instance for fluent chaining.</returns>
        public PropertyMetadata<T> WithDefaultValue(T defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        /// <summary>
        /// Sets a factory delegate used to lazily compute the default value, and returns this instance for fluent chaining.
        /// </summary>
        /// <param name="defaultValueInterceptor">A delegate that returns the default value.</param>
        /// <returns>This instance for fluent chaining.</returns>
        public PropertyMetadata<T> WithDefaultValue(Func<T> defaultValueInterceptor)
        {
            DefaultValueInterceptor = defaultValueInterceptor;
            return this;
        }

        private bool defaultValueSet;
        private T _defaultValue;
        /// <summary>
        /// Gets or sets the default value for this property.
        /// </summary>
        public virtual T DefaultValue
        {
            get
            {
                if (!defaultValueSet && DefaultValueInterceptor != null)
                {
                    SetDefaultValue(new PropertyValue<T>(DefaultValueInterceptor()));
                }

                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
                defaultValueSet = true;
            }
        }

        /// <summary>Gets or sets the delegate used to lazily provide the default value.</summary>
        public Func<T> DefaultValueInterceptor
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public override void SetDefaultValue(PropertyValue value)
        {
            DefaultValue = ((PropertyValue<T>)value).Value;
        }

        /// <inheritdoc/>
        public override PropertyValue GetDefaultValue()
        {
            return new PropertyValue<T>(DefaultValue);
        }

        Action<PropertyValueChangedArgs<T>> propertyChangedHandler;

        /// <summary>
        /// Registers a callback invoked whenever the property value changes.
        /// </summary>
        /// <param name="propertyChangedHandler">The callback to invoke on change.</param>
        /// <returns>This instance for fluent chaining.</returns>
        public PropertyMetadata<T> OnChanged(Action<PropertyValueChangedArgs<T>> propertyChangedHandler)
        {
            this.propertyChangedHandler = propertyChangedHandler;

            return this;
        }

        internal PropertyMetadata<T> NotifyChanged(PropertyValueChangedArgs<T> pvc)
        {
            propertyChangedHandler?.Invoke(pvc);

            return this;
        }
    }

    /// <summary>
    /// Marks a property as having associated <see cref="PropertyMetadata"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyMetadataAttribute : Attribute
    {

    }
}
