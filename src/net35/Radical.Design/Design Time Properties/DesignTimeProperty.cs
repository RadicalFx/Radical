using System;
using System.Linq;
using System.ComponentModel;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using System.Globalization;
using System.Collections;

namespace Topics.Radical.Design
{
    abstract class DesignTimeProperty : PropertyDescriptor
    {
        bool _isReadOnly;

        protected DesignTimeProperty( Type componentType, String propertyName )
            : base( propertyName, null )
        {
            this._componentType = componentType;
        }

        protected DesignTimeProperty( Type componentType, DesignTimeProperty sourceProperty )
            : base( sourceProperty.Name, null )
        {
            this._componentType = componentType;

            this._isReadOnly = sourceProperty.IsReadOnly;
            this.IsLocalizableResource = sourceProperty.IsLocalizableResource;
            this.CulturePropertyName = sourceProperty.CulturePropertyName;
        }

        public override bool CanResetValue( object component )
        {
            return false;
        }

        Type _componentType;
        public override Type ComponentType
        {
            get { return this._componentType; }
        }

        public override void ResetValue( object component )
        {

        }

        public override void SetValue( object component, object value )
        {

        }

        public override bool ShouldSerializeValue( object component )
        {
            return false;
        }

        public override bool IsReadOnly
        {
            get { return this._isReadOnly; }
        }

        public DesignTimeProperty AsReadOnly()
        {
            this._isReadOnly = true;
            return this;
        }

        public bool IsLocalizableResource
        {
            get;
            private set;
        }

        protected string CulturePropertyName { get; private set; }

        public DesignTimeProperty AsLocalizableResource()
        {
            return this.AsLocalizableResource( "Culture" );
        }

        public DesignTimeProperty AsLocalizableResource( String culturePropertyName )
        {
            this.CulturePropertyName = culturePropertyName;
            this.IsLocalizableResource = true;
            return this;
        }

        public virtual void OnUICultureChanged( CultureInfo newUICulture )
        {
            var value = this.GetValue( null );
            
            if( value is ISupportUICulture )
            {
                ( ( ISupportUICulture )value ).UICulture = newUICulture;
            }
            else if( value is IEnumerable )
            {
                ( ( IEnumerable )value )
                    .OfType<ISupportUICulture>()
                    .ForEach( isuc => isuc.UICulture = newUICulture );
            }
        }
    }
}
