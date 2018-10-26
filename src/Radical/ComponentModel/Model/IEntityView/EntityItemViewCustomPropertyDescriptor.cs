using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Radical.ComponentModel
{
    /// <summary>
    /// A specialized PropertyDescriptor
    /// </summary>
    /// <typeparam name="T">The type incapsulated by this descriptor</typeparam>
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
        public TValue GetDefaultValue()
        {
            if( this.DafaultValueInterceptor != null )
            {
                return this.DafaultValueInterceptor();
            }

            return default( TValue );
        }

        /// <summary>
        /// Gets or sets the dafault value interceptor.
        /// </summary>
        /// <value>
        /// The dafault value interceptor.
        /// </value>
        public Func<TValue> DafaultValueInterceptor
        {
            get;
            set;
        }

        public EntityItemViewCustomPropertyDescriptor( String customPropertyName, EntityItemViewValueGetter<T, TValue> getter )
            : this( customPropertyName, getter, null )
        {

        }

        public EntityItemViewCustomPropertyDescriptor( String customPropertyName, EntityItemViewValueGetter<T, TValue> getter, EntityItemViewValueSetter<T, TValue> setter )
            : this( customPropertyName )
        {
            this.ValueGetter = getter;
            this.ValueSetter = setter;
        }

        public EntityItemViewCustomPropertyDescriptor( String customDisplayName )
            : base()
        {
            this._customDisplayName = customDisplayName;
        }

        String _customDisplayName = null;

        /// <summary>
        /// Gets the name that can be displayed in a window, such as a Properties window.
        /// </summary>
        /// <value></value>
        /// <returns>The name to display for the member.</returns>
        public override String DisplayName
        {
            get { return this.Name; }
        }

        public override String Name
        {
            get { return this._customDisplayName; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get { return typeof( TValue ); }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the property is read-only; otherwise, false.</returns>
        public override Boolean IsReadOnly
        {
            get { return this.ValueSetter == null; }
        }

        protected override object GetValueCore( IEntityItemView<T> component )
        {
            var args = new EntityItemViewValueGetterArgs<T, TValue>( component, this.Name );
            var returnValue = this.ValueGetter( args );

            return returnValue;
        }

        protected override void SetValueCore( IEntityItemView<T> component, object value )
        {
            var args = new EntityItemViewValueSetterArgs<T, TValue>( component, this.Name, ( TValue )value );
            this.ValueSetter( args );
        }
    }
}
