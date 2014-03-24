using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using Topics.Radical.Conversions;
using Topics.Radical.ComponentModel;
using Windows.UI.Core;

namespace Topics.Radical.Observers
{
    /// <summary>
    /// A static entry to simplify the creation a PropertyChangedMonitor.
    /// </summary>
    public static class PropertyObserver
    {
        /// <summary>
        /// Monitors the specified source.
        /// </summary>
        /// <typeparam name="T">The type of the source to monitor.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>An instance of the monitor.</returns>
        public static PropertyChangedMonitor<T> For<T>( T source )
            where T : INotifyPropertyChanged
        {
            return new PropertyChangedMonitor<T>( source );
        }

        /// <summary>
        /// Monitors the specified source.
        /// </summary>
        /// <typeparam name="T">The type of the source to monitor.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>An instance of the monitor.</returns>
        public static PropertyChangedMonitor<T> For<T>( T source, CoreDispatcher dispatcher )
            where T : INotifyPropertyChanged
        {
            return new PropertyChangedMonitor<T>( source, dispatcher );
        }

        /// <summary>
        /// Monitors all the properties of specified source.
        /// </summary>
        /// <typeparam name="T">The type of the source to monitor.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// An instance of the monitor.
        /// </returns>
        public static PropertyChangedMonitor ForAllPropertiesOf<T>( T source )
            where T : INotifyPropertyChanged
        {
            return new PropertyChangedMonitor( source );
        }
    }

    /// <summary>
    /// A specialized observer to monitor INotifyPropertyChanged instances.
    /// </summary>
    public class PropertyChangedMonitor : AbstractMonitor
    {
        PropertyChangedEventHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMonitor"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public PropertyChangedMonitor( INotifyPropertyChanged source )
            : base( source )
        {

        }

        /// <summary>
        /// Starts monitoring the given source object.
        /// </summary>
        /// <param name="source">The source.</param>
        protected override void StartMonitoring( object source )
        {
            base.StartMonitoring( source );

            this.handler = ( s, e ) => this.OnChanged();

            var inpc = ( INotifyPropertyChanged )source;
            inpc.PropertyChanged += this.handler;
        }

        /// <summary>
        /// Called in order to allow inheritors to stop the monitoring operations.
        /// </summary>
        /// <param name="targetDisposed"><c>True</c> if this call is subsequent to the Dispose of the monitored instance.</param>
        protected override void OnStopMonitoring( bool targetDisposed )
        {
            if ( !targetDisposed && this.WeakSource != null && this.WeakSource.IsAlive )
            {
                var inpc = ( INotifyPropertyChanged )this.WeakSource.Target;
                inpc.PropertyChanged -= this.handler;
            }

            this.handler = null;
        }

    }

    /// <summary>
    /// A specialized observer to monitor INotifyPropertyChanged instances.
    /// </summary>
    /// <typeparam name="T">The type of the item to monitor.</typeparam>
    public class PropertyChangedMonitor<T> : AbstractMonitor<T>,
        INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="args">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged( PropertyChangedEventArgs args )
        {
            if ( this.Dispatcher != null && !this.Dispatcher.HasThreadAccess )
            {
                this.Dispatcher.RunAsync( CoreDispatcherPriority.Normal, () =>
                {
                    this.OnPropertyChanged( args );
                } );
            }
            else
            {
                if ( this.PropertyChanged != null )
                {
                    this.PropertyChanged( this, args );
                }
            }
        }

        IDictionary<String, Action<T, String>> propertiesToWatch = new Dictionary<String, Action<T, String>>();
        IList<AbstractMonitor> observablePropertiesToWatch = new List<AbstractMonitor>();

        PropertyChangedEventHandler handler = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMonitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public PropertyChangedMonitor( T source )
            : this( source, null )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMonitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public PropertyChangedMonitor( T source, CoreDispatcher dispatcher )
            : base( source, dispatcher )
        {
            handler = ( s, e ) =>
            {
                Action<T, String> callback;
                if ( propertiesToWatch.TryGetValue( e.PropertyName, out callback ) )
                {
                    if ( callback != null )
                    {
                        callback( source, e.PropertyName );
                    }
                }

                if ( propertiesToWatch.ContainsKey( e.PropertyName ) )
                {
                    this.OnPropertyChanged( e );
                    this.OnChanged();
                }
            };

            this.Source.PropertyChanged += handler;
        }

        /// <summary>
        /// Observes the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// Itself, used for fluent programming.
        /// </returns>
        public PropertyChangedMonitor<T> Observe( String property )
        {
            Ensure.That( property ).Named( "property" ).IsNotNullNorEmpty();

            this.Observe( property, null );

            return this;
        }

        /// <summary>
        /// Observes the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> Observe<TProperty>( Expression<Func<T, TProperty>> property )
        {
            Ensure.That( property ).Named( "property" ).IsNotNull();

            this.Observe( property, null );

            return this;
        }

        /// <summary>
        /// Observes the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> Observe<TProperty>( Expression<Func<T, TProperty>> property, Action<T, String> callback )
        {
            Ensure.That( property ).Named( "property" ).IsNotNull();

            return this.Observe( property.GetMemberName(), callback );
        }

        /// <summary>
        /// Observes the specified property.
        /// </summary>
        /// <param name="propertyName">The property.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> Observe( String propertyName, Action<T, String> callback )
        {
            Ensure.That( propertyName ).Named( "propertyName" ).IsNotNullNorEmpty();

            if ( propertiesToWatch.ContainsKey( propertyName ) )
            {
                propertiesToWatch[ propertyName ] = callback;
            }
            else
            {
                propertiesToWatch.Add( propertyName, callback );
            }

            return this;
        }

        ///// <summary>
        ///// Observes the specified observable property.
        ///// </summary>
        ///// <typeparam name="TValue">The type of the value.</typeparam>
        ///// <param name="property">The property.</param>
        ///// <returns>Itself, used for fluent programming.</returns>
        //public PropertyChangedMonitor<T> Observe<TValue>( Observable<TValue> property )
        //{
        //    Ensure.That( property ).Named( "property" ).IsNotNull();

        //    var om = new PropertyChangedMonitor<Observable<TValue>>( property );
        //    om.Observe( p => p.Value );
        //    om.Changed += ( s, e ) => this.OnChanged();

        //    this.observablePropertiesToWatch.Add( om );

        //    return this;
        //}

        /// <summary>
        /// Stops observing the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> StopObserving<TProperty>( Expression<Func<T, TProperty>> property )
        {
            var propertyName = property.GetMemberName();
            if ( propertiesToWatch.ContainsKey( propertyName ) )
            {
                propertiesToWatch.Remove( propertyName );
            }

            return this;
        }

        ///// <summary>
        ///// Stops observing the specified observable property.
        ///// </summary>
        ///// <typeparam name="TValue">The type of the value.</typeparam>
        ///// <param name="property">The property.</param>
        ///// <returns>Itself, used for fluent programming.</returns>
        //public PropertyChangedMonitor<T> StopObserving<TValue>( Observable<TValue> property )
        //{
        //    Ensure.That( property ).Named( "property" ).IsNotNull();

        //    var om = this.observablePropertiesToWatch
        //        .Where( am => am.As<PropertyChangedMonitor<Observable<TValue>>>().Source == property )
        //        .SingleOrDefault();

        //    if ( om != null )
        //    {
        //        om.StopMonitoring();
        //        this.observablePropertiesToWatch.Remove( om );
        //    }

        //    return this;
        //}

        /// <summary>
        /// Called in order to allow inheritors to stop the monitoring operations.
        /// </summary>
        /// <param name="targetDisposed"><c>True</c> if this call is subsequent to the Dispose of the monitored instance.</param>
        protected override void OnStopMonitoring( bool targetDisposed )
        {
            if ( !targetDisposed && this.WeakSource != null && this.WeakSource.IsAlive )
            {
                this.Source.PropertyChanged -= handler;
            }

            handler = null;
            propertiesToWatch.Clear();
            propertiesToWatch = null;

            observablePropertiesToWatch.Clear();
            observablePropertiesToWatch = null;
        }
    }
}
