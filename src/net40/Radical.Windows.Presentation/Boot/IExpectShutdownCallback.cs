using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Boot
{
    /// <summary>
    /// Allows a third party component to be notified when the
    /// application lifecycle state changes.
    /// </summary>
    [ToolkitComponentAttribute]
    public interface IExpectShutdownCallback
    {
        /// <summary>
        /// Called when the application shuts down.
        /// </summary>
        /// <param name="reason">The reason.</param>
        void OnShutdown( ApplicationShutdownReason reason );
    }
}
