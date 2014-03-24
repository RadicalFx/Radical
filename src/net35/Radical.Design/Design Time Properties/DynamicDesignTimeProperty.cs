using System;
using System.Globalization;
using System.Reflection;

namespace Topics.Radical.Design
{
    class DynamicDesignTimeProperty<T, TValue> : DesignTimeProperty<T, TValue>
    {
        readonly Func<CultureInfo, TValue> valueHandler;
        public DynamicDesignTimeProperty( DesignTimeHost<T> host, DesignTimeProperty property, Func<CultureInfo, TValue> valueHandler )
            : base( host, property )
        {
            this.valueHandler = valueHandler;
        }

        public override object GetValue( object component )
        {
            return this.valueHandler( this.host.UICulture );
        }

        public override void OnUICultureChanged( CultureInfo newUICulture )
        {
            base.OnUICultureChanged( newUICulture );

            if ( this.IsLocalizableResource )
            {
                var property = this.valueHandler( newUICulture ).GetType().GetProperty
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
