using System;
using System.Security.Principal;
using System.Threading;

namespace Radical
{
    namespace ComponentModel
    {
        public interface IAnalyticsServices
        {
            Boolean IsEnabled { get; set; }
            void TrackUserActionAsync(Analytics.AnalyticsEvent action);
        }
    }

    namespace Services
    {
        class AnalyticsServices : ComponentModel.IAnalyticsServices
        {
            public void TrackUserActionAsync(Analytics.AnalyticsEvent action)
            {
                Analytics.AnalyticsServices.TrackUserActionAsync(action);
            }

            public bool IsEnabled
            {
                get { return Analytics.AnalyticsServices.IsEnabled; }
                set { Analytics.AnalyticsServices.IsEnabled = value; }
            }
        }
    }

    namespace Analytics
    {
        public static class AnalyticsServices
        {
            public static Boolean IsEnabled { get; set; }

            public static void TrackUserActionAsync(AnalyticsEvent action)
            {
                if (IsEnabled && UserActionTrackingHandler != null)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                   {
                       UserActionTrackingHandler(action);
                   });
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
            }

            public DateTimeOffset ExecutedOn { get; set; }

            public String Name { get; set; }

            public Object Data { get; set; }
        }
    }
}