using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radical.ComponentModel;
using Radical.Observers;
using System.Timers;

namespace Radical.Tests.Threading
{
    public class TimerTrigger : AbstractMonitor<Timer>
    {
        static Timer CreateDefault( int interval, TimerTriggerMode triggerMode )
        {
            var t = new Timer( interval );
            t.Enabled = false;
            //t.AutoReset = triggerMode == TimerTriggerMode.Always;

            return t;
        }

        ElapsedEventHandler elapsedEventHandler = null;

        public TimerTrigger( int interval, TimerTriggerMode triggerMode )
            : base( CreateDefault( interval, triggerMode ) )
        {
            this.Mode = triggerMode;

            this.elapsedEventHandler = ( s, e ) =>
            {
                this.Stop();
                this.NotifyChanged();
                if( this.Mode == TimerTriggerMode.Always )
                {
                    this.Start();
                }
            };
        }

        protected override void StartMonitoring( object source )
        {
            base.StartMonitoring( source );

            var t = ( Timer )source;
            t.Elapsed += this.elapsedEventHandler;
        }

        protected override void OnStopMonitoring( bool targetDisposed )
        {
            if( !targetDisposed )
            {
                this.Stop();

                if( this.WeakSource.IsAlive )
                {
                    this.Source.Elapsed -= this.elapsedEventHandler;
                }
            }
        }

        public void Start()
        {
            if( this.WeakSource.IsAlive )
            {
                this.Source.Enabled = true;
            }
        }

        public void Stop()
        {
            if( this.WeakSource.IsAlive )
            {
                this.Source.Enabled = false;
            }
        }

        public TimerTriggerMode Mode { get; private set; }
    }

    public enum TimerTriggerMode 
    { 
        Once, 
        Always 
    }
}