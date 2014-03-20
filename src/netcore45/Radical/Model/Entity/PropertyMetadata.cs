using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Topics.Radical.Linq;
using Topics.Radical.Validation;

namespace Topics.Radical.Model
{
	//public static class PropertyMetadataBuilder
	//{
	//    public class TypedPropertyMetadataBuilder<T>
	//    {
	//        public PropertyMetadata<TValue> And<TValue>( Expression<Func<T, TValue>> property )
	//        {
	//            var name = property.GetMemberName();
	//            return new PropertyMetadata<TValue>( name );
	//        }
	//    }

	//    public static TypedPropertyMetadataBuilder<T> For<T>()
	//    {
	//        return new TypedPropertyMetadataBuilder<T>();
	//    }
	//}

	public abstract class PropertyMetadata
	{
		/// <summary>
		/// Creates the metadata for specified property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="property">The property.</param>
		/// <returns>An instance of the property metadata.</returns>
		public static PropertyMetadata<T> Create<T>( Expression<Func<T>> property )
		{
			var name = property.GetMemberName();
			return PropertyMetadata.Create<T>( name );
		}

		/// <summary>
		/// Creates the metadata for specified property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>
		/// An instance of the property metadata.
		/// </returns>
		public static PropertyMetadata<T> Create<T>( String propertyName )
		{
			return new PropertyMetadata<T>( propertyName );
		}

#if WINDOWS_PHONE
		readonly List<String> cascadeChangeNotifications = new List<String>();
#else
		readonly HashSet<String> cascadeChangeNotifications = new HashSet<String>();
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected PropertyMetadata( String propertyName )
		{
			Ensure.That( propertyName ).Named( "propertyName" ).IsNotNullNorEmpty();

			this.PropertyName = propertyName;
			this.NotifyChanges = true;
		}

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		/// <value>
		/// The name of the property.
		/// </value>
		public String PropertyName { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether the property represented by this metadata should notify changes.
		/// </summary>
		/// <value>
		///   <c>true</c> if the property should notify changes; otherwise, <c>false</c>.
		/// </value>
		public Boolean NotifyChanges { get; set; }

		/// <summary>
		/// Disables changes notifications for this property.
		/// </summary>
		/// <returns>This metadata instance.</returns>
		public PropertyMetadata DisableChangesNotifications()
		{
			this.NotifyChanges = false;
			return this;
		}

		/// <summary>
		/// Enables changes notifications for this property.
		/// </summary>
		/// <returns>This metadata instance.</returns>
		public PropertyMetadata EnableChangesNotifications()
		{
			this.NotifyChanges = true;
			return this;
		}

		public PropertyMetadata AddCascadeChangeNotifications<T>( Expression<Func<T>> property )
		{
			return this.AddCascadeChangeNotifications( property.GetMemberName() );
		}

		public PropertyMetadata AddCascadeChangeNotifications( String property )
		{
			this.cascadeChangeNotifications.Add( property );

			return this;
		}

		public PropertyMetadata RemoveCascadeChangeNotifications<T>( Expression<Func<T>> property )
		{
			return this.RemoveCascadeChangeNotifications( property.GetMemberName() );
		}

		public PropertyMetadata RemoveCascadeChangeNotifications( String property )
		{
			if( this.cascadeChangeNotifications.Contains( property ) )
			{
				this.cascadeChangeNotifications.Remove( property );
			}

			return this;
		}

		public IEnumerable<String> GetCascadeChangeNotifications()
		{
			return this.cascadeChangeNotifications;
		}

		public abstract void SetDefaultValue( PropertyValue value );

		public abstract PropertyValue GetDefaultValue();
	}
}
