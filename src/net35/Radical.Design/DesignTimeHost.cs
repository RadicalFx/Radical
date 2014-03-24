using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Model;
using System.Threading;
using System.Globalization;

namespace Topics.Radical.Design
{
    /// <summary>
    /// An implementation of <see cref="ICustomTypeDescriptor"/> to create WPF
    /// design time data.
    /// </summary>
    /// <typeparam name="T">The hosted type.</typeparam>
    public class DesignTimeHost<T> : Entity, ICustomTypeDescriptor, ITypedList, IEnumerable<T>, ISupportUICulture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignTimeHost&lt;T&gt;"/> class.
        /// </summary>
        public DesignTimeHost()
        {
            this._defaultUICulture = Thread.CurrentThread.CurrentUICulture;
        }

        /// <summary>
        /// Exposes the specified property.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>A property builder.</returns>
        protected IPropertyBuilder<T, TValue> Expose<TValue>( Expression<Func<T, TValue>> property )
        {
            var builder = new PropertyBuilder<T, TValue>( this );
            this.builders.Add( builder );

            return builder.Create( property );
        }

        internal void RaisePropertyChanged( DesignTimeProperty property )
        {
            this.OnPropertyChanged( property.Name );
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>The property value.</returns>
        public TValue GetDesignPropertyValue<TValue>( Expression<Func<T, TValue>> property )
        {
            var name = property.GetMemberName();
            return this.GetDesignPropertyValue<TValue>( name );
        }

        /// <summary>
        /// Raises the property changed event for one of the fake exposed properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="property">The property.</param>
        protected void OnExposedPropertyChanged<TValue>( Expression<Func<T, TValue>> property )
        {
            var name = property.GetMemberName();
            this.OnPropertyChanged( name );
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        TValue GetDesignPropertyValue<TValue>( String propertyName )
        {
            var prop = this.properties[ propertyName ];

            return ( TValue )prop.GetValue( this );
        }

        CultureInfo _defaultUICulture = null;
        CultureInfo _uiCulture = null;

        /// <summary>
        /// Gets or sets the UI culture.
        /// </summary>
        /// <value>
        /// The UI culture.
        /// </value>
        public CultureInfo UICulture
        {
            get { return _uiCulture; }
            set
            {
                if( value != this.UICulture )
                {
                    if( value == null )
                    {
                        value = this._defaultUICulture;
                    }

                    this._uiCulture = value;
                    this.OnUICultureChanged();
                }
            }
        }

        /// <summary>
        /// Called when UI culture changes.
        /// </summary>
        protected virtual void OnUICultureChanged()
        {
            this.GetProperties()
                .OfType<DesignTimeProperty>()
                .ForEach( dtp => dtp.OnUICultureChanged( this.UICulture ) );
        }

        readonly List<IPropertyBuilder> builders = new List<IPropertyBuilder>();

        PropertyDescriptorCollection properties = null;

        /// <summary>
        /// Returns the properties for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties for this component instance.
        /// </returns>
        protected PropertyDescriptorCollection GetProperties()
        {
            if( properties == null )
            {
                var dtp = builders.Aggregate( new List<DesignTimeProperty>(), ( a, b ) =>
                {
                    a.Add( b.GetProperty() );
                    return a;
                } ).ToArray();

                properties = new PropertyDescriptorCollection( dtp );
            }

            return properties;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties( Attribute[] attributes )
        {
            return this.GetProperties();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return this.GetProperties();
        }

        #region Ignored ICustomTypeDescriptor members

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            throw new NotImplementedException();
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            throw new NotImplementedException();
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            throw new NotImplementedException();
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            throw new NotImplementedException();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        object ICustomTypeDescriptor.GetEditor( Type editorBaseType )
        {
            throw new NotImplementedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents( Attribute[] attributes )
        {
            return new EventDescriptorCollection( new EventDescriptor[ 0 ] );
        }

        object ICustomTypeDescriptor.GetPropertyOwner( PropertyDescriptor pd )
        {
            return this;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return new EventDescriptorCollection( new EventDescriptor[ 0 ] );
        }

        #endregion

        /*
         * The following are required to let the VS2010 designer understand the structure of
         * nested properties.
         * When, e.g., the designer of ListView Columns is opend the designer ignores the fact
         * that the nested bound property is ICustomTypeDescriptor and inspect the type using
         * raw reflection.
         * If the root type implements IEnumerable and ITypedList (it's stupid I know) the designer does
         * the following attempts:
         *  - a first call to GetEnumerator() in order to find the first element of the list is done, if
         *      a first element can be found that element will be inspected to find properties;
         *  - if no first element can be found and the type implements ITypedList a call to GetItemProperties
         *      is performed to get the structure of the element type: the problem here is that this call is
         *      done on the root element with a null listAccessor thus we cannot understand which is the
         *      inspected property. Fortunatly returning an empty PropertyDescriptorCollection leads the
         *      designer to proceed with the next attempt;
         *  - in the end the designer taks care of the item type and since the item is ICustomTypeDescriptor
         *      evenrything flows as expected...
         */

        PropertyDescriptorCollection ITypedList.GetItemProperties( PropertyDescriptor[] listAccessors )
        {
            return new PropertyDescriptorCollection( new PropertyDescriptor[ 0 ] );
        }

        string ITypedList.GetListName( PropertyDescriptor[] listAccessors )
        {
            return String.Empty;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield break;
        }
    }

    interface ISupportUICulture
    {
        CultureInfo UICulture { get; set; }
    }
}
