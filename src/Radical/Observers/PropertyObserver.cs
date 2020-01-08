using Radical.ComponentModel;
using Radical.Conversions;
using Radical.Linq;
using Radical.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Radical.Observers
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
        public static PropertyChangedMonitor<T> For<T>(T source)
            where T : INotifyPropertyChanged
        {
            return new PropertyChangedMonitor<T>(source);
        }

        /// <summary>
        /// Monitors the specified source.
        /// </summary>
        /// <typeparam name="T">The type of the source to monitor.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <returns>An instance of the monitor.</returns>
        public static PropertyChangedMonitor<T> For<T>(T source, IDispatcher dispatcher)
            where T : INotifyPropertyChanged
        {
            return new PropertyChangedMonitor<T>(source, dispatcher);
        }

        /// <summary>
        /// Monitors all the properties of specified source.
        /// </summary>
        /// <typeparam name="T">The type of the source to monitor.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// An instance of the monitor.
        /// </returns>
        public static PropertyChangedMonitor ForAllPropertiesOf<T>(T source)
            where T : INotifyPropertyChanged
        {
            return new PropertyChangedMonitor(source);
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
        public PropertyChangedMonitor(INotifyPropertyChanged source)
            : base(source)
        {

        }

        /// <summary>
        /// Starts monitoring the given source object.
        /// </summary>
        /// <param name="source">The source.</param>
        protected override void StartMonitoring(object source)
        {
            base.StartMonitoring(source);

            handler = (s, e) => OnChanged();

            var inpc = (INotifyPropertyChanged)source;
            inpc.PropertyChanged += handler;
        }

        /// <summary>
        /// Called in order to allow inheritors to stop the monitoring operations.
        /// </summary>
        /// <param name="targetDisposed"><c>True</c> if this call is subsequent to the Dispose of the monitored instance.</param>
        protected override void OnStopMonitoring(bool targetDisposed)
        {
            if (!targetDisposed && WeakSource != null && WeakSource.IsAlive)
            {
                var inpc = (INotifyPropertyChanged)WeakSource.Target;
                inpc.PropertyChanged -= handler;
            }

            handler = null;
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
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (Dispatcher != null && !Dispatcher.IsSafe)
            {
                Dispatcher.Dispatch(args, e =>
               {
                   OnPropertyChanged(e);
               });
            }
            else
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, args);
                }
            }
        }

        IDictionary<string, Action<T, string>> propertiesToWatch = new Dictionary<string, Action<T, string>>();
        IList<AbstractMonitor> observablePropertiesToWatch = new List<AbstractMonitor>();

        PropertyChangedEventHandler handler = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMonitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public PropertyChangedMonitor(T source)
            : this(source, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMonitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        public PropertyChangedMonitor(T source, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            handler = (s, e) =>
            {
                Action<T, string> callback;
                if (propertiesToWatch.TryGetValue(e.PropertyName, out callback))
                {
                    if (callback != null)
                    {
                        callback(source, e.PropertyName);
                    }
                }

                if (propertiesToWatch.ContainsKey(e.PropertyName))
                {
                    OnPropertyChanged(e);
                    OnChanged();
                }
            };

            Source.PropertyChanged += handler;
        }

        /// <summary>
        /// Observes the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// Itself, used for fluent programming.
        /// </returns>
        public PropertyChangedMonitor<T> Observe(string property)
        {
            Ensure.That(property).Named("property").IsNotNullNorEmpty();

            Observe(property, null);

            return this;
        }

        /// <summary>
        /// Observes the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> Observe<TProperty>(Expression<Func<T, TProperty>> property)
        {
            Ensure.That(property).Named("property").IsNotNull();

            Observe(property, null);

            return this;
        }

        /// <summary>
        /// Observes the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> Observe<TProperty>(Expression<Func<T, TProperty>> property, Action<T, string> callback)
        {
            Ensure.That(property).Named("property").IsNotNull();

            return Observe(property.GetMemberName(), callback);
        }

        /// <summary>
        /// Observes the specified property.
        /// </summary>
        /// <param name="propertyName">The property.</param>
        /// <param name="callback">The callback.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> Observe(string propertyName, Action<T, string> callback)
        {
            Ensure.That(propertyName).Named("propertyName").IsNotNullNorEmpty();

            if (propertiesToWatch.ContainsKey(propertyName))
            {
                propertiesToWatch[propertyName] = callback;
            }
            else
            {
                propertiesToWatch.Add(propertyName, callback);
            }

            return this;
        }

        /// <summary>
        /// Observes the specified observable property.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> Observe<TValue>(Observable<TValue> property)
        {
            Ensure.That(property).Named("property").IsNotNull();

            var om = new PropertyChangedMonitor<Observable<TValue>>(property);
            om.Observe(p => p.Value);
            om.Changed += (s, e) => OnChanged();

            observablePropertiesToWatch.Add(om);

            return this;
        }

        /// <summary>
        /// Stops observing the specified property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> StopObserving<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var propertyName = property.GetMemberName();
            if (propertiesToWatch.ContainsKey(propertyName))
            {
                propertiesToWatch.Remove(propertyName);
            }

            return this;
        }

        /// <summary>
        /// Stops observing the specified observable property.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>Itself, used for fluent programming.</returns>
        public PropertyChangedMonitor<T> StopObserving<TValue>(Observable<TValue> property)
        {
            Ensure.That(property).Named("property").IsNotNull();

            var om = observablePropertiesToWatch
                .Where(am => am.As<PropertyChangedMonitor<Observable<TValue>>>().Source == property)
                .SingleOrDefault();

            if (om != null)
            {
                om.StopMonitoring();
                observablePropertiesToWatch.Remove(om);
            }

            return this;
        }

        /// <summary>
        /// Called in order to allow inheritors to stop the monitoring operations.
        /// </summary>
        /// <param name="targetDisposed"><c>True</c> if this call is subsequent to the Dispose of the monitored instance.</param>
        protected override void OnStopMonitoring(bool targetDisposed)
        {
            if (!targetDisposed && WeakSource != null && WeakSource.IsAlive)
            {
                Source.PropertyChanged -= handler;
            }

            handler = null;
            propertiesToWatch.Clear();
            propertiesToWatch = null;

            observablePropertiesToWatch.Clear();
            observablePropertiesToWatch = null;
        }
    }
}
