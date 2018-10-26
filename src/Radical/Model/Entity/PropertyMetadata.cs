using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Radical.Linq;
using Radical.Validation;
using System.Reflection;

namespace Radical.Model
{
    public abstract class PropertyMetadata : IDisposable
    {
        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="PropertyMetadata"/> is reclaimed by garbage collection.
        /// </summary>
        ~PropertyMetadata()
        {
            this.Dispose( false );
        }
        
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose( Boolean disposing )
        {
            if ( disposing )
            {
                this.cascadeChangeNotifications.Clear();   
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        #endregion

        /// <summary>
        /// Creates the metadata for specified property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyOwner">The property owner.</param>
        /// <param name="property">The property.</param>
        /// <returns>
        /// An instance of the property metadata.
        /// </returns>
        public static PropertyMetadata<T> Create<T>( Object propertyOwner, Expression<Func<T>> property )
        {
            var name = property.GetMemberName();
            return PropertyMetadata.Create<T>( propertyOwner, name );
        }

        /// <summary>
        /// Creates the metadata for specified property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyOwner">The property owner.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// An instance of the property metadata.
        /// </returns>
        public static PropertyMetadata<T> Create<T>( Object propertyOwner, String propertyName )
        {
            return new PropertyMetadata<T>( propertyOwner, propertyName );
        }

#if WINDOWS_PHONE
        readonly List<String> cascadeChangeNotifications = new List<String>();
#else
        readonly HashSet<String> cascadeChangeNotifications = new HashSet<String>();
#endif

        readonly Object propertyOwner;
        PropertyInfo _property;

        protected PropertyInfo Property 
        {
            get 
            {
                if ( this._property == null ) 
                {
                    this._property = this.propertyOwner
                        .GetType()
                        .GetProperty( this.PropertyName );
                }

                return this._property;
            } 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata" /> class.
        /// </summary>
        /// <param name="propertyOwner">The property owner.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected PropertyMetadata( Object propertyOwner, String propertyName )
        {
            Ensure.That( propertyOwner ).Named( "propertyOwner" ).IsNotNull();
            Ensure.That( propertyName ).Named( "propertyName" ).IsNotNullNorEmpty();

            this.propertyOwner = propertyOwner;
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
            if ( this.cascadeChangeNotifications.Contains( property ) )
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

        //public void AddCustomMetadata<T>( String key, T value ) 
        //{

        //}

        //public T GetCustomMetadata<T>( String key )
        //{

        //}
    }
}
