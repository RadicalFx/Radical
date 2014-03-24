using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Windows.Presentation.Boot
{
    /// <summary>
    /// Determines the reason for application shutdown.
    /// </summary>
    public enum ApplicationShutdownReason
    {
#if !SILVERLIGHT
        /// <summary>
        /// The application has been shutdown using the Radical canonical behaviors.
        /// In this case the shutdown process can be canceled.
        /// </summary>
        UserRequest = 0,

        /// <summary>
        /// The application is shutting down because another 
        /// instance is already running and the application 
        /// is marked as singleton.
        /// </summary>
        MultipleInstanceNotAllowed = 1,

        /// <summary>
        /// The application is shutting down because the operating system session is ending.
        /// </summary>
        SessionEnding,
#endif
        /// <summary>
        /// The application has been shut down using the App.Current.Shutdown() method.
        /// </summary>
        ApplicationRequest,
    }
}
