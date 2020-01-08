using System;
using System.Collections.Generic;

namespace Radical
{
    namespace ComponentModel
    {
        public interface IAnalyticsServices
        {
            bool IsEnabled { get; set; }
            void TrackUserActionAsync(Analytics.AnalyticsEvent action);
        }
    }

    namespace Analytics
    {
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
                ExecutedOn = DateTimeOffset.Now;
            }

            public DateTimeOffset ExecutedOn { get; set; }

            public string Name { get; set; }

            public IDictionary<string, object> Data { get; set; }
        }
    }
}