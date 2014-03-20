using System;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using System.Globalization;

namespace Topics.Radical.Design
{
	sealed class PropertyBuilder<T, TValue> :
		IPropertyBuilder,
		IPropertyBuilder<T, TValue>,
		IReadOnlyPropertyBuilder<T, TValue>
		//IStaticPropertyBuilder<T, TValue>,
		//ILivePropertyBuilder<T, TValue>,
		//IDesignablePropertyBuilder<T, TValue>,
		//IDynamicPropertyBuilder<T>
	{
		readonly DesignTimeHost<T> host;

		internal PropertyBuilder( DesignTimeHost<T> host )
		{
			this.host = host;
		}

		DesignTimeProperty property = null;

		DesignTimeProperty IPropertyBuilder.GetProperty()
		{
			Ensure.That( property ).Named( "property" ).IsNotNull();

			return this.property;
		}

		public IPropertyBuilder<T, TValue> Create( Expression<Func<T, TValue>> property )
		{
			return this.Create( property.GetMemberName() );
		}

		public IPropertyBuilder<T, TValue> Create( String property )
		{
			this.property = new UnboundedDesignTimeProperty<T, TValue>( this.host, property );

			return this;
		}

		IReadOnlyPropertyBuilder<T, TValue> IPropertyBuilder<T, TValue>.AsReadOnly()
		{
			this.property.AsReadOnly();

			return this;
		}

		void WithStaticValue( TValue value )
		{
			this.property = new StaticDesignTimeProperty<T, TValue>( this.host, this.property, value );
		}

		void WithLiveValue( Expression<Func<TValue>> liveValueProperty )
		{
			this.property = new LiveDesignTimeProperty<T, TValue>( this.host, this.property, liveValueProperty );
		}

        void WithSimilarValue<TObject>( TObject value )
		{
			this.property = new SimilarDesignTimeProperty<T, TObject>( this.host, this.property, value );
		}

        void WithLiveSimilarValue<TObject>( Expression<Func<TObject>> liveValueProperty )
        {
            this.property = new LiveDesignTimeProperty<T, TObject>( this.host, this.property, liveValueProperty );
        }

        void WithDynamicValue( Func<CultureInfo, TValue> valueHandler )
        {
            this.property = new DynamicDesignTimeProperty<T, TValue>( this.host, this.property, valueHandler );
        }

        IPropertyBuilder<T, TValue> IPropertyBuilder<T, TValue>.AsLocalizableResource()
        {
            this.property.AsLocalizableResource();

            return this;
        }

		void IPropertyBuilder<T, TValue>.WithStaticValue( TValue value )
		{
			this.WithStaticValue( value );
		}

        void IPropertyBuilder<T, TValue>.WithDynamicValue( Func<CultureInfo, TValue> valueHandler )
        {
            this.WithDynamicValue( valueHandler );
        }

		void IPropertyBuilder<T, TValue>.WithLiveValue( Expression<Func<TValue>> liveValueProperty )
		{
			this.WithLiveValue( liveValueProperty );
		}

		void IPropertyBuilder<T, TValue>.WithSimilarValue<TObject>( TObject value )
		{
            this.WithSimilarValue( value );
		}

        void IPropertyBuilder<T, TValue>.WithLiveSimilarValue<TObject>( Expression<Func<TObject>> liveValueProperty )
        {
            this.WithLiveSimilarValue( liveValueProperty );
        }

		void IReadOnlyPropertyBuilder<T, TValue>.WithStaticValue( TValue value )
		{
			this.WithStaticValue( value );
		}

        void IReadOnlyPropertyBuilder<T, TValue>.WithDynamicValue( Func<CultureInfo, TValue> valueHandler )
        {
            this.WithDynamicValue( valueHandler );
        }

		void IReadOnlyPropertyBuilder<T, TValue>.WithLiveValue( Expression<Func<TValue>> liveValueProperty )
		{
			this.WithLiveValue( liveValueProperty );
		}

		void IReadOnlyPropertyBuilder<T, TValue>.WithSimilarValue<TObject>( TObject value )
		{
            this.WithSimilarValue( value );
		}

        void IReadOnlyPropertyBuilder<T, TValue>.WithLiveSimilarValue<TObject>( Expression<Func<TObject>> liveValueProperty )
        {
            this.WithLiveSimilarValue( liveValueProperty );
        }
	}
}
