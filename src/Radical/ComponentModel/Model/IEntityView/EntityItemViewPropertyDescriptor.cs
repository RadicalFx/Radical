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
    public class EntityItemViewPropertyDescriptor<T> :
        PropertyDescriptor
    //where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewPropertyDescriptor&lt;T&gt;"/> 
        /// class, creating a custom calculated property. In this case this EntityItemViewPropertyDescriptor
        /// is not mapped to a real property on the underlying object.
        /// </summary>
        protected EntityItemViewPropertyDescriptor()
            : base(string.Format(CultureInfo.InvariantCulture, "___RuntimeEvaluatedPropertyDescriptor_{0:N}", Guid.NewGuid()), null)
        {
            this._property = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewPropertyDescriptor&lt;T&gt;"/> class.
        /// The EntityItemViewPropertyDescriptor will be mapped on the given property.
        /// </summary>
        /// <param name="property">The property to map to</param>
        public EntityItemViewPropertyDescriptor(PropertyInfo property)
            : base(property.Name, null)
        {
            this._property = property;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewPropertyDescriptor&lt;T&gt;"/> class.
        /// The EntityItemViewPropertyDescriptor will be mapped on the property given its name.
        /// </summary>
        /// <param name="propertyName">The property name to map to, the property must exists on the underlying type</param>
        public EntityItemViewPropertyDescriptor(string propertyName)
            : this(typeof(T).GetProperty(propertyName))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewPropertyDescriptor&lt;T&gt;"/> class.
        /// The EntityItemViewPropertyDescriptor will be mapped on the given property name and uses the given
        /// string as display name.
        /// </summary>
        /// <param name="propertyName">The property name to map to, the property must exists on the underlying type</param>
        /// <param name="customDisplayName">A string used as the DisplayName for this descriptor.</param>
        public EntityItemViewPropertyDescriptor(string propertyName, string customDisplayName)
            : this(typeof(T).GetProperty(propertyName))
        {
            this._customDisplayName = customDisplayName;
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns>The default value for this property.</returns>
        public virtual Object GetDefaultValue()
        {
            if (this.PropertyType.IsValueType)
            {
                return Activator.CreateInstance(this.PropertyType);
            }
            else
            {
                return null;
            }
        }

        private PropertyInfo _property;

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <value>The property.</value>
        protected PropertyInfo Property
        {
            get { return this._property; }
        }

        string _customDisplayName = null;

        /// <summary>
        /// Gets the name that can be displayed in a window, such as a Properties window.
        /// </summary>
        /// <value></value>
        /// <returns>The name to display for the member.</returns>
        public override string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this._customDisplayName))
                {
                    return this._customDisplayName;
                }

                return base.DisplayName;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"/> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"/> methods are invoked, the object specified might be an instance of this type.</returns>
        public override Type ComponentType
        {
            get { return typeof(IEntityItemView<T>); }
        }

        private Type _customPropertyType = null;

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get { return this.Property.PropertyType; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the property is read-only; otherwise, false.</returns>
        public override bool IsReadOnly
        {
            get { return !this.Property.CanWrite; }
        }

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>
        /// true if the property should be persisted; otherwise, false.
        /// </returns>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object changes its value.
        /// </summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>
        /// true if resetting the component changes its value; otherwise, false.
        /// </returns>
        public override bool CanResetValue(object component)
        {
            return false;

            /*
             * Per farlo funzionare correttamente, anche se per l'uso che ne facciamo noi
             * è del tutto irrilevante bisogna andare alla ircerca di eventuali attributi
             * DefaultValue impostati sulla proprietà
             */
            #region Possible Implementation

            //if( this.Property != null )
            //{
            //    PropertyDescriptor desc = (from pd in TypeDescriptor.GetProperties( component ).OfType<PropertyDescriptor>()
            //                              where pd.Name = this.Property.Name 
            //                               select pd).First<PropertyDescriptor>();

            //    var defaultValueAttributes = from at in desc.Attributes.OfType<Attribute>()
            //                                 where at.GetType() == typeof(DefaultValueAttribute) 
            //                                 select at;


            //}
            //else
            //{
            //    return false;
            //}    

            #endregion
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public override void ResetValue(object component)
        {
            //NOP
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>
        /// The value of a property for a given component.
        /// </returns>
        public override sealed object GetValue(object component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            IEntityItemView<T> oiv = component as IEntityItemView<T>;
            if (oiv == null)
            {
                throw new ArgumentException("InvalidComponentType", "component");
            }

            return this.GetValueCore(oiv);
        }

        //Radical.Reflection.Function<Object> fastGetter = null;

        protected virtual object GetValueCore(IEntityItemView<T> component)
        {
            //if( fastGetter == null ) 
            //{
            //    fastGetter = Radical.Reflection.ObjectExtensions.CreateFastPropertyGetter( component.EntityItem, this.Property );
            //}

            //return fastGetter();

            return this.Property.GetValue(component.EntityItem, null);
        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override sealed void SetValue(object component, object value)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException("Current property is readonly.");
            }

            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            IEntityItemView<T> oiv = component as IEntityItemView<T>;
            if (oiv == null)
            {
                throw new ArgumentException("InvalidComponentType", "component");
            }

            this.SetValueCore(oiv, value);
        }

        protected virtual void SetValueCore(IEntityItemView<T> component, object value)
        {
            this.Property.SetValue(component.EntityItem, value, null);
        }
    }
}
