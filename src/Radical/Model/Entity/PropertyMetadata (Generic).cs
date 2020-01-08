using Radical.Linq;
using System;
using System.Linq.Expressions;

namespace Radical.Model
{
    public class PropertyMetadata<T> : PropertyMetadata
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {

            }

            DefaultValueInterceptor = null;
            propertyChangedHandler = null;
        }

        public PropertyMetadata(object propertyOwner, string propertyName)
            : base(propertyOwner, propertyName)
        {

        }

        public PropertyMetadata(object propertyOwner, Expression<Func<T>> property)
            : this(propertyOwner, property.GetMemberName())
        {

        }

        public PropertyMetadata<T> WithDefaultValue(T defaultValue)
        {
            DefaultValue = defaultValue;
            return this;
        }

        public PropertyMetadata<T> WithDefaultValue(Func<T> defaultValueInterceptor)
        {
            DefaultValueInterceptor = defaultValueInterceptor;
            return this;
        }

        private bool defaultValueSet;
        private T _defaultValue = default(T);
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

        public Func<T> DefaultValueInterceptor
        {
            get;
            set;
        }

        public override void SetDefaultValue(PropertyValue value)
        {
            DefaultValue = ((PropertyValue<T>)value).Value;
        }

        public override PropertyValue GetDefaultValue()
        {
            return new PropertyValue<T>(DefaultValue);
        }

        Action<PropertyValueChangedArgs<T>> propertyChangedHandler;

        public PropertyMetadata<T> OnChanged(Action<PropertyValueChangedArgs<T>> propertyChangedHandler)
        {
            this.propertyChangedHandler = propertyChangedHandler;

            return this;
        }

        internal PropertyMetadata<T> NotifyChanged(PropertyValueChangedArgs<T> pvc)
        {
            if (propertyChangedHandler != null)
            {
                propertyChangedHandler(pvc);
            }

            return this;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyMetadataAttribute : Attribute
    {

    }
}
