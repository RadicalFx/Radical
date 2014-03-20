using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Validation;

namespace Topics.Radical.Model
{
#if !SILVERLIGHT && !NETFX_CORE
	[Serializable]
#endif
	public abstract class Entity :
		INotifyPropertyChanged,
		IDisposable
	{
		#region IDisposable Members

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Entity"/> is reclaimed by garbage collection.
		/// </summary>
		~Entity()
		{
			this.Dispose( false );
		}

		private Boolean isDisposed;

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( Boolean disposing )
		{
			if( disposing )
			{
				/*
				 * Se disposing è 'true' significa che dispose
				 * è stato invocato direttamentente dall'utente
				 * è quindi lecito accedere ai 'field' e ad 
				 * eventuali reference perchè sicuramente Finalize
				 * non è ancora stato chiamato su questi oggetti
				 */
				if( this._events != null )
				{
					this.Events.Dispose();
				}

				this.valuesBag.Clear();
				//this.initialValuesBag.Clear();
				this.propertiesMetadata.Clear();
			}

			//this._dataContext = null;
			this._events = null;

			//Set isDisposed flag
			this.isDisposed = true;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Verifies that this instance is not disposed, throwing an
		/// <see cref="ObjectDisposedException"/> if this instance has
		/// been already disposed.
		/// </summary>
		protected virtual void EnsureNotDisposed()
		{
			if( this.isDisposed )
			{
				throw new ObjectDisposedException( this.GetType().FullName );
			}
		}

		#endregion

		#region EventHandlerList

		private EventHandlerList _events;

		/// <summary>
		/// Gets the events.
		/// </summary>
		/// <item>The events.</item>
		protected EventHandlerList Events
		{
			get
			{
				if( this._events == null )
				{
					this._events = new EventHandlerList();
				}

				return this._events;
			}
		}

		#endregion

		#region INotifyPropertyChanged Members

		private static readonly Object propertyChangedEventKey = new Object();
		public event PropertyChangedEventHandler PropertyChanged
		{
			add { this.Events.AddHandler( propertyChangedEventKey, value ); }
			remove { this.Events.RemoveHandler( propertyChangedEventKey, value ); }
		}

		protected virtual void OnPropertyChanged( PropertyChangedEventArgs e )
		{
			this.EnsureNotDisposed();

			PropertyChangedEventHandler h = this.Events[ propertyChangedEventKey ] as PropertyChangedEventHandler;
			if( h != null )
			{
				h( this, e );
			}
		}

		protected void OnPropertyChanged( String propertyName )
		{
			this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
		}

		protected void OnPropertyChanged<T>( Expression<Func<T>> property )
		{
			this.EnsureNotDisposed();

			PropertyChangedEventHandler h = this.Events[ propertyChangedEventKey ] as PropertyChangedEventHandler;
			if( h != null )
			{
				this.OnPropertyChanged( new PropertyChangedEventArgs( property.GetMemberName() ) );
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Entity"/> class.
		/// </summary>
		protected Entity()
		{
			
		}

		/// <summary>
		/// Sets the initial property value building default 
		/// metadata for the given property and setting the default
		/// value in the built metadata.
		/// </summary>
		/// <remarks>
		/// This method is a shortcut for the GetMetadata method, in order
		/// to fine customize property metadata use the GetMetadata method.
		/// The main difference between SetInitialPropertyValue and SetPropertyValue 
		/// is that SetInitialPropertyValue does not raise a property change notification.
		/// </remarks>
		/// <typeparam name="T">The property type</typeparam>
		/// <param name="property">The property.</param>
		/// <param name="value">The default value.</param>
		protected PropertyMetadata<T> SetInitialPropertyValue<T>( Expression<Func<T>> property, T value )
		{
			return this.SetInitialPropertyValue<T>( property.GetMemberName(), value );
		}

		/// <summary>
		/// Sets the initial property value building default 
		/// metadata for the given property and setting the default
		/// value in the built metadata.
		/// </summary>
		/// <remarks>
		/// This method is a shortcut for the GetMetadata method, in order
		/// to fine customize property metadata use the GetMetadata method.
		/// The main difference between SetInitialPropertyValue and SetPropertyValue 
		/// is that SetInitialPropertyValue does not raise a property change notification.
		/// </remarks>
		/// <typeparam name="T">The property type</typeparam>
		/// <param name="property">The property.</param>
		/// <param name="value">The default value.</param>
		protected PropertyMetadata<T> SetInitialPropertyValue<T>( String property, T value )
		{
			var metadata = this.GetPropertyMetadata<T>( property );
			metadata.DefaultValue = value;

			return metadata;
		}

		readonly IDictionary<String, PropertyMetadata> propertiesMetadata = new Dictionary<String, PropertyMetadata>();

		/// <summary>
		/// Gets the property metadata.
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		/// <param name="property">The property.</param>
		/// <returns>
		/// An instance of the requested property metadata.
		/// </returns>
		protected PropertyMetadata<T> GetPropertyMetadata<T>( Expression<Func<T>> property )
		{
			Ensure.That( property ).Named( () => property ).IsNotNull();

			return ( PropertyMetadata<T> )this.GetPropertyMetadata<T>( property.GetMemberName() );
		}

		/// <summary>
		/// Gets the property metadata.
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>An instance of the requested property metadata.</returns>
		protected PropertyMetadata<T> GetPropertyMetadata<T>( String propertyName )
		{
			Ensure.That( propertyName ).Named( () => propertyName ).IsNotNullNorEmpty();

			//return ( PropertyMetadata<T> )this.GetPropertyMetadata( propertyName, typeof( T ) );

			PropertyMetadata md;
			if( !this.propertiesMetadata.TryGetValue( propertyName, out md ) )
			{
				md = this.GetDefaultMetadata<T>( propertyName );
				this.propertiesMetadata.Add( propertyName, md );
			}

			return ( PropertyMetadata<T> )md;
		}

		///// <summary>
		///// Gets the property metadata.
		///// </summary>
		///// <param name="propertyName">Name of the property.</param>
		///// <param name="propertyType">Type of the property.</param>
		///// <returns>An instance of the requested property metadata.</returns>
		//protected PropertyMetadata GetPropertyMetadata( String propertyName, Type propertyType )
		//{
		//    PropertyMetadata md;
		//    if( !this.propertiesMetadata.TryGetValue( propertyName, out md ) )
		//    {
		//        md = this.GetDefaultMetadata( propertyName, propertyType );
		//        this.propertiesMetadata.Add( propertyName, md );
		//    }

		//    return md;
		//}

		/// <summary>
		/// Gets the default property metadata.
		/// </summary>
		/// <typeparam name="T">Type of the property.</typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>An instance of the requested default property metadata.</returns>
		protected virtual PropertyMetadata<T> GetDefaultMetadata<T>( String propertyName )
		{
			return PropertyMetadata.Create<T>( propertyName );

			//return ( PropertyMetadata<T> )this.GetDefaultMetadata( propertyName, typeof( T ) );
		}

		///// <summary>
		///// Gets the default property metadata.
		///// </summary>
		///// <param name="propertyName">Name of the property.</param>
		///// <param name="propertyType">Type of the property.</param>
		///// <returns>
		///// An instance of the requested default property metadata.
		///// </returns>
		//protected virtual PropertyMetadata GetDefaultMetadata( String propertyName, Type propertyType )
		//{
		//    var type = typeof( PropertyMetadata<> ).MakeGenericType( propertyType );

		//    return ( PropertyMetadata )Activator.CreateInstance( type, new Object[] { propertyName } );
		//}

		/// <summary>
		/// Sets the property metadata.
		/// </summary>
		/// <param name="metadata">The property metadata.</param>
		protected virtual void SetPropertyMetadata<T>( PropertyMetadata<T> metadata )
		{
			Ensure.That( metadata ).Named( "metadata" ).IsNotNull();

			Ensure.That( propertiesMetadata )
				.WithMessage( "Metadata for the supplied property ({0}) has already been set.", metadata.PropertyName )
				.IsFalse( d => d.ContainsKey( metadata.PropertyName ) );

			propertiesMetadata.Add( metadata.PropertyName, metadata );
		}

		//protected virtual void AddMetadata<T>( Expression<Func<T>> property, Action<PropertyMetadata<T>> interceptor )
		//{
		//    var md = this.GetDefaultMetadata<T>( property.GetMemberName() );
		//    interceptor( md );

		//    this.SetPropertyMetadata( md );
		//}

		/// <summary>
		/// Determines whether metadata for the specified property has been set.
		/// </summary>
		/// <typeparam name="T">The property type.</typeparam>
		/// <param name="property">The property.</param>
		/// <returns>
		/// 	<c>true</c> if metadata for the specified property has been set; otherwise, <c>false</c>.
		/// </returns>
		protected virtual Boolean HasMetadata<T>( Expression<Func<T>> property )
		{
			return this.propertiesMetadata.ContainsKey( property.GetMemberName() );
		}

		readonly IDictionary<String, PropertyValue> valuesBag = new Dictionary<String, PropertyValue>();

		protected void SetPropertyValueCore<T>( String propertyName, T data, PropertyValueChanged<T> pvc )
		{
			var oldValue = this.GetPropertyValue<T>( propertyName );

			if( !Object.Equals( oldValue, data ) )
			{
				if( this.valuesBag.ContainsKey( propertyName ) )
				{
					this.valuesBag[ propertyName ] = new PropertyValue<T>( data );
				}
				else
				{
					this.valuesBag.Add( propertyName, new PropertyValue<T>( data ) );
				}

				var args = new PropertyValueChangedArgs<T>( data, oldValue );
				if( pvc != null )
				{
					pvc( args );
				}

				var metadata = this.GetPropertyMetadata<T>( propertyName );
				if( metadata.NotifyChanges )
				{
					this.OnPropertyChanged( propertyName );

					metadata
						.NotifyChanged( args )
						.GetCascadeChangeNotifications()
						.ForEach( s =>
						{
							this.OnPropertyChanged( s );
						} );
				}
			}
		}

		protected void SetPropertyValue<T>( Expression<Func<T>> property, T data )
		{
			var propertyName = property.GetMemberName();

			this.SetPropertyValue( propertyName, data, null );
		}

		protected void SetPropertyValue<T>( Expression<Func<T>> property, T data, PropertyValueChanged<T> pvc )
		{
			var propertyName = property.GetMemberName();

			this.SetPropertyValue( propertyName, data, pvc );
		}

		protected void SetPropertyValue<T>( String propertyName, T data )
		{
			this.SetPropertyValue( propertyName, data, null );
		}

		protected virtual void SetPropertyValue<T>( String propertyName, T data, PropertyValueChanged<T> pvc )
		{
			this.SetPropertyValueCore( propertyName, data, pvc );
		}

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <typeparam name="T">The property value type.</typeparam>
		/// <param name="property">A Lambda Expressione representing the property.</param>
		/// <returns>The requested property value.</returns>
		protected T GetPropertyValue<T>( Expression<Func<T>> property )
		{
			return this.GetPropertyValue<T>( property.GetMemberName() );
		}

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <typeparam name="T">The property value type.</typeparam>
		/// <param name="property">A Lambda Expressione representing the property.</param>
		/// <param name="initialValueSetter">The initial value setter.</param>
		/// <returns>The requested property value.</returns>
		protected T GetPropertyValue<T>( Expression<Func<T>> property, Func<T> initialValueSetter )
		{
			if( !this.HasMetadata( property ) && initialValueSetter != null )
			{
				var initialValue = initialValueSetter();

				this.SetInitialPropertyValue( property, initialValue );
			}

			return this.GetPropertyValue<T>( property.GetMemberName() );
		}

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <typeparam name="T">The property value type.</typeparam>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>The requested property value.</returns>
		protected virtual T GetPropertyValue<T>( String propertyName )
		{
			PropertyValue actual;
			if( this.valuesBag.TryGetValue( propertyName, out actual ) )
			{
				return ( ( PropertyValue<T> )actual ).Value;
			}

			var metadata = this.GetPropertyMetadata<T>( propertyName );
			return metadata.DefaultValue;
		}
	}
}
