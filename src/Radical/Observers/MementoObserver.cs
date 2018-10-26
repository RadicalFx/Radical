using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radical.ComponentModel.ChangeTracking;
using Radical.Validation;
using Radical.ComponentModel;

namespace Radical.Observers
{
    public static class MementoObserver
    {
        public static MementoMonitor Monitor( IChangeTrackingService source )
        {
            return new MementoMonitor( source );
        }

        public static MementoMonitor Monitor( IChangeTrackingService source, IDispatcher dispatcher )
        {
            return new MementoMonitor( source, dispatcher );
        }
    }

    public class MementoMonitor : AbstractMonitor<IChangeTrackingService>
    {
        EventHandler handler = null;

        public MementoMonitor( IChangeTrackingService source )
            : this( source, null )
        {

        }

        public MementoMonitor( IChangeTrackingService source, IDispatcher dispatcher )
            : base( source, dispatcher )
        {
            handler = ( s, e ) => this.OnChanged();

            this.Source.TrackingServiceStateChanged += handler;
        }

        protected override void OnStopMonitoring( Boolean targetDisposed )
        {
            if( !targetDisposed && this.WeakSource != null && this.WeakSource.IsAlive )
            {
                this.Source.TrackingServiceStateChanged -= handler;
            }

            handler = null;
        }
    }
}
