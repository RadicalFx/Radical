using System;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Topics.Radical.Design
{
    class StaticDesignTimeProperty<T, TValue> : DesignTimeProperty<T, TValue>
    {
        readonly TValue value;
        public StaticDesignTimeProperty( DesignTimeHost<T> host, DesignTimeProperty property, TValue value )
            : base( host, property )
        {
            this.value = value;
        }

        public override object GetValue( object component )
        {
            return this.value;
        }

        //public override Type PropertyType
        //{
        //    get
        //    {
        //        if( this.value == null )
        //        {
        //            return base.PropertyType;
        //        }

        //        return this.value.GetType();
        //    }
        //}

        public override void OnUICultureChanged( CultureInfo newUICulture )
        {
            base.OnUICultureChanged( newUICulture );

            if ( this.IsLocalizableResource )
            {
                var property = this.value.GetType().GetProperty
                (
                    name: this.CulturePropertyName,
                    bindingAttr: BindingFlags.Public | BindingFlags.Static
                );

                if ( property != null )
                {
                    property.SetValue( null, newUICulture, null );

                    this.host.RaisePropertyChanged( this );
                }
                else
                {
                    var message = String.Format( "Cannot find any property named '{0}' on '{1}'.", this.CulturePropertyName, this.value.GetType() );
                    Debug.WriteLine( message );

                    Debugger.Break();
                }
            }
        }
    }

    //interface IDesignTimeList
    //{
    //    String Name { get; set; }
    //    Func<Attribute[], PropertyDescriptorCollection> GetPropertiesHandler { get; set; }
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class DesignTimeList<T> : List<T>, ITypedList, IDesignTimeList, ICustomTypeDescriptor
    //{
    //    /// <summary>
    //    /// Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties on each item used to bind data.
    //    /// </summary>
    //    /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor"/> objects to find in the collection as bindable. This can be null.</param>
    //    /// <returns>
    //    /// The <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> that represents the properties on each item used to bind data.
    //    /// </returns>
    //    public PropertyDescriptorCollection GetItemProperties( PropertyDescriptor[] listAccessors )
    //    {
    //        var properties = TypeDescriptor.GetProperties( typeof( T ) );

    //        return properties;
    //    }

    //    /// <summary>
    //    /// Returns the name of the list.
    //    /// </summary>
    //    /// <param name="listAccessors">An array of <see cref="T:System.ComponentModel.PropertyDescriptor"/> objects, for which the list name is returned. This can be null.</param>
    //    /// <returns>
    //    /// The name of the list.
    //    /// </returns>
    //    public string GetListName( PropertyDescriptor[] listAccessors )
    //    {
    //        return this.Name;
    //    }

    //    /// <summary>
    //    /// Gets or sets the name.
    //    /// </summary>
    //    /// <value>
    //    /// The name.
    //    /// </value>
    //    public string Name
    //    {
    //        get;
    //        set;
    //    }

    //    /// <summary>
    //    /// Gets or sets the get properties handler.
    //    /// </summary>
    //    /// <value>
    //    /// The get properties handler.
    //    /// </value>
    //    public Func<Attribute[], PropertyDescriptorCollection> GetPropertiesHandler
    //    {
    //        get;
    //        set;
    //    }

    //    AttributeCollection ICustomTypeDescriptor.GetAttributes()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    string ICustomTypeDescriptor.GetClassName()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    string ICustomTypeDescriptor.GetComponentName()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    TypeConverter ICustomTypeDescriptor.GetConverter()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    object ICustomTypeDescriptor.GetEditor( Type editorBaseType )
    //    {
    //        throw new NotImplementedException();
    //    }

    //    EventDescriptorCollection ICustomTypeDescriptor.GetEvents( Attribute[] attributes )
    //    {
    //        throw new NotImplementedException();
    //    }

    //    EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties( Attribute[] attributes )
    //    {
    //        return this.GetPropertiesHandler( attributes );
    //    }

    //    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
    //    {
    //        return this.GetPropertiesHandler( null );
    //    }

    //    object ICustomTypeDescriptor.GetPropertyOwner( PropertyDescriptor pd )
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    class SimilarDesignTimeProperty<T, TValue> : DesignTimeProperty<T, TValue> //, ITypedList
    {
        readonly TValue value;
        public SimilarDesignTimeProperty( DesignTimeHost<T> host, DesignTimeProperty property, TValue value )
            : base( host, property )
        {
            this.value = value;

            //var idtl = this.value as IDesignTimeList;
            //if( idtl != null )
            //{
            //    idtl.Name = this.Name;
            //    idtl.GetPropertiesHandler = attributes => 
            //    {
            //        return TypeDescriptor.GetProperties( typeof( TValue ) );
            //    };
            //}
        }

        public override object GetValue( object component )
        {
            return this.value;
        }

        public override void OnUICultureChanged( CultureInfo newUICulture )
        {
            base.OnUICultureChanged( newUICulture );

            if ( this.IsLocalizableResource )
            {
                var property = this.value.GetType().GetProperty
                (
                    name: this.CulturePropertyName,
                    bindingAttr: BindingFlags.Public | BindingFlags.Static
                );

                if ( property != null )
                {
                    property.SetValue( null, newUICulture, null );

                    this.host.RaisePropertyChanged( this );
                }
            }
        }
    }
}
