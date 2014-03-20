using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using System.Reflection;

namespace Topics.Radical.Model
{
	public class PropertyMetadata<T> : PropertyMetadata
	{
        public PropertyMetadata( Object propertyOwner, String propertyName )
			: base( propertyOwner, propertyName )
		{
            
		}

        public PropertyMetadata( Object propertyOwner, Expression<Func<T>> property )
			: this( propertyOwner, property.GetMemberName() )
		{

		}

		public PropertyMetadata<T> WithDefaultValue( T defaultValue )
		{
			this.DefaultValue = defaultValue;
			return this;
		}

        public PropertyMetadata<T> WithDefaultValue( Func<T> defaultValueInterceptor )
		{
			this.DefaultValueInterceptor = defaultValueInterceptor;
			return this;
		}

		private Boolean defaultValueSet;
		private T _defaultValue = default( T );
		public virtual T DefaultValue
		{
			get
			{
				if( !this.defaultValueSet && this.DefaultValueInterceptor != null )
				{
					this.SetDefaultValue( new PropertyValue<T>( this.DefaultValueInterceptor() ) );
				}

				return this._defaultValue;
			}
			set
			{
				this._defaultValue = value;
				this.defaultValueSet = true;
			}
		}

		public Func<T> DefaultValueInterceptor
		{
			get;
			set;
		}

		public override void SetDefaultValue( PropertyValue value )
		{
			this.DefaultValue = ( ( PropertyValue<T> )value ).Value;
		}

		public override PropertyValue GetDefaultValue()
		{
			return new PropertyValue<T>( this.DefaultValue );
		}

        Action<PropertyValueChangedArgs<T>> propertyChangedHandler;

        public PropertyMetadata<T> OnChanged( Action<PropertyValueChangedArgs<T>> propertyChangedHandler )
        {
            this.propertyChangedHandler = propertyChangedHandler;

            return this;
        }

        internal PropertyMetadata<T> NotifyChanged( PropertyValueChangedArgs<T> pvc )
        {
            if( this.propertyChangedHandler != null )
            {
                this.propertyChangedHandler( pvc );
            }

            return this;
        }
    }

    [AttributeUsage( AttributeTargets.Property )]
    public class PropertyMetadataAttribute : Attribute
    {
        
    }
}
