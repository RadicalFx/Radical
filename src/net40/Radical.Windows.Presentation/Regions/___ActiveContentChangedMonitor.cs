//using System;
//using System.Linq;
//using System.Collections.Generic;
//using Topics.Radical.Validation;
//using Topics.Radical.Conversions;
//using Topics.Toolkits.Ran.ComponentModel.Presentation;
//using Topics.Radical.Observers;

//namespace Topics.Toolkits.Ran.Windows.Presentation.Regions
//{
//    /// <summary>
//    /// A concrete implementation of the <see cref="IActiveContentChangedMonitor"/> interface.
//    /// </summary>
//    public sealed class ActiveContentChangedMonitor : IActiveContentChangedMonitor
//    {
//        sealed class Monitorer : AbstractMonitor<ISwitchingElementsRegion>,
//            IRegionMonitor,
//            IDisposable
//        {
//            readonly EventHandler<ActiveContentChangedEventArgs> h;
//            readonly ISwitchingElementsRegion region;
//            //IView previousContent;

//            /// <summary>
//            /// Initializes a new instance of the <see cref="Monitorer"/> class.
//            /// </summary>
//            /// <param name="region">The region.</param>
//            /// <param name="onActiveContentChangedUserCallback">The on active content changed user callback.</param>
//            /// <param name="iMonitorCallback">The i monitor callback.</param>
//            public Monitorer( ISwitchingElementsRegion region, ActiveContentChanged onActiveContentChangedUserCallback, Action iMonitorCallback )
//                : base( region )
//            {
//                Ensure.That( onActiveContentChangedUserCallback ).Named( "onActiveContentChangedUserCallback" ).IsNotNull();
//                Ensure.That( iMonitorCallback ).Named( "iMonitorCallback" ).IsNotNull();

//                this.h = ( s, e ) =>
//                {
//                    this.OnChanged();
//                    onActiveContentChangedUserCallback( this, e );
//                    iMonitorCallback();
//                };

//                this.region = region;
//                this.region.ActiveContentChanged += this.h;
//            }

//            /// <summary>
//            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
//            /// </summary>
//            public void Dispose()
//            {
//                this.OnStopMonitoring( false );
//            }

//            protected override void OnStopMonitoring( bool targetDisposed )
//            {
//                if( this.h != null && this.region != null )
//                {
//                    this.region.ActiveContentChanged -= this.h;
//                }
//            }

//            public IView ActiveContent
//            {
//                get { return this.region.ActiveContent; }
//            }

//            public IView PreviousActiveContent
//            {
//                get { return this.region.PreviousActiveContent; }
//            }

//            public bool TryGetViewModel<T>( IView sourceView, out T viewModel )
//                where T : class, IViewModel
//            {
//                if( sourceView != null )
//                {
//                    viewModel = sourceView.DataContext as T;
//                }
//                else
//                {
//                    viewModel = null;
//                }

//                return viewModel != null;
//            }

//            public T GetViewModel<T>( IView sourceView )
//                where T : class, IViewModel
//            {
//                return ( T )sourceView.DataContext;
//            }


//            public bool TryGetViewModel<T>( IView sourceView, Action<T> succesfullGetInterceptor ) where T : class, IViewModel
//            {
//                T vm;
//                if( this.TryGetViewModel<T>( sourceView, out vm ) )
//                {
//                    succesfullGetInterceptor( vm );
//                    return true;
//                }

//                return false;
//            }
//        }

//        /// <summary>
//        /// Occurs when the source monitored by this monitor changes.
//        /// </summary>
//        public event EventHandler Changed;
		
//        private void OnChanged()
//        {
//            if( this.Changed != null )
//            {
//                this.Changed( this, EventArgs.Empty );
//            }
//        }

//        readonly IRegionService svc;
//        readonly IDictionary<String, Monitorer> monitoredRegions = new Dictionary<String, Monitorer>();

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ActiveContentChangedMonitor"/> class.
//        /// </summary>
//        /// <param name="svc">The SVC.</param>
//        public ActiveContentChangedMonitor( IRegionService svc )
//        {
//            Ensure.That( svc ).Named( "svc" ).IsNotNull();

//            this.svc = svc;
//        }

//        /// <summary>
//        /// Monitors the specified region within the given region manager.
//        /// </summary>
//        /// <param name="regionManager">The region manager.</param>
//        /// <param name="regionName">Name of the region.</param>
//        /// <param name="onActiveContentChanged">The on active content changed delegate invoked whenever the active content changes.</param>
//        /// <returns>The region monitor instance.</returns>
//        public IRegionMonitor Monitor( IRegionManager regionManager, String regionName, ActiveContentChanged onActiveContentChanged )
//        {
//            Monitorer monitorer;
//            if( !this.monitoredRegions.TryGetValue( regionName, out monitorer ) )
//            {
//                ISwitchingElementsRegion region;
//                if( regionManager.TryGetRegion<ISwitchingElementsRegion>( regionName, out region ) )
//                {
//                    monitorer = new Monitorer( region, onActiveContentChanged, () => this.OnChanged() );
//                    this.monitoredRegions.Add( regionName, monitorer );
//                }
//                else
//                {
//                    throw new ArgumentOutOfRangeException( "regionName", "Cannot find the given region within the given RegionManager" );
//                }
//            }

//            return monitorer;
//        }

//        /// <summary>
//        /// Monitors the specified region.
//        /// </summary>
//        /// <typeparam name="TView">The type of the view.</typeparam>
//        /// <param name="regionName">Name of the region.</param>
//        /// <param name="onActiveContentChanged">The on active content changed delegate invoked whenever the active content changes.</param>
//        /// <returns>The region monitor instance.</returns>
//        public IRegionMonitor Monitor<TView>( String regionName, ActiveContentChanged onActiveContentChanged ) where TView : IView
//        {
//            var rgm = this.svc.GetKnownRegionManager<TView>();
//            return this.Monitor( rgm, regionName, onActiveContentChanged );
//        }

//        /// <summary>
//        /// Stops monitoring the region idenfied by the given region name.
//        /// </summary>
//        /// <param name="regionName">Name of the region.</param>
//        public void StopMonitoring( String regionName )
//        {
//            if( this.monitoredRegions.ContainsKey( regionName ) )
//            {
//                var m = this.monitoredRegions[ regionName ];
//                m.Dispose();

//                this.monitoredRegions.Remove( regionName );
//            }
//        }

//        /// <summary>
//        /// Stops all the monitoring operation.
//        /// </summary>
//        public void StopMonitoring()
//        {
//            var all = this.monitoredRegions.Keys.ToArray();
//            foreach( var mr in all )
//            {
//                this.StopMonitoring( mr );
//            }
//        }

//        /// <summary>
//        /// Asks this monitor to raise a change notification in order
//        /// to trigger all the listeners.
//        /// </summary>
//        public void NotifyChanged()
//        {
//            this.OnChanged();
//        }
//    }
//}