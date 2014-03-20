using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Topics.Radical
{
    namespace ComponentModel
    {
        public interface IAnalyticsServices
        {
            Boolean IsEnabled { get; set; }
            void TrackUserActionAsync( Analytics.AnalyticsEvent action );
        }
    }

    namespace Services
    {
        class AnalyticsServices : ComponentModel.IAnalyticsServices
        {
            public void TrackUserActionAsync( Analytics.AnalyticsEvent action )
            {
                Topics.Radical.Analytics.AnalyticsServices.TrackUserActionAsync( action );
            }

            public bool IsEnabled
            {
                get { return Topics.Radical.Analytics.AnalyticsServices.IsEnabled; }
                set { Topics.Radical.Analytics.AnalyticsServices.IsEnabled = value; }
            }
        }
    }

    namespace Analytics
    {
        public static class AnalyticsServices
        {
            public static Boolean IsEnabled { get; set; }

            public static void TrackUserActionAsync( AnalyticsEvent action )
            {
                if ( IsEnabled && UserActionTrackingHandler != null )
                {
#if FX40
                System.Threading.Tasks.Task.Factory.StartNew( () =>
                {
                    UserActionTrackingHandler( action );
                } );
#else
                    UserActionTrackingHandler.BeginInvoke( action, new AsyncCallback( r => { } ), null );
#endif
                }
            }

            public static Action<AnalyticsEvent> UserActionTrackingHandler { get; set; }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public class AnalyticsEvent
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AnalyticsEvent" /> class.
            /// </summary>
            public AnalyticsEvent()
            {
                this.ExecutedOn = DateTimeOffset.Now;
#if !SILVERLIGHT && !NETFX_CORE
            this.Identity = Thread.CurrentPrincipal.Identity;
#endif
            }

            public DateTimeOffset ExecutedOn { get; set; }

            public String Name { get; set; }

            public Object Data { get; set; }
#if !SILVERLIGHT && !NETFX_CORE
        public IIdentity Identity { get; set; }
#endif
        }
    }
}