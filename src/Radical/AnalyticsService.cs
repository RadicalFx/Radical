using System;
using System.Collections.Generic;

namespace Radical
{
    namespace ComponentModel
    {
        /// <summary>
        /// Represents the basic behavior of an analytics services designed to store history of users' actions.
        /// </summary>
        public interface IAnalyticsServices
        {
            /// <summary>
            /// Whether this service is enabled or not. 
            /// </summary>
            bool IsEnabled { get; set; }
            
            /// <summary>
            /// Stores the supplied event
            /// </summary>
            /// <param name="action">The event to store.</param>
            void TrackUserActionAsync(Analytics.AnalyticsEvent action);
        }
    }

    namespace Analytics
    {
        /// <summary>
        /// Defines something that happened in the running application.
        /// </summary>
        public class AnalyticsEvent
        {
            /// <summary>
            /// When the event happened. Default value is <c>DateTimeOffset.Now</c>. 
            /// </summary>
            public DateTimeOffset ExecutedOn { get; set; } = DateTimeOffset.Now;

            /// <summary>
            /// An optional name that describes the event.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// A dictionary of optional data to enrich the event description.
            /// </summary>
            public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        }
    }
}