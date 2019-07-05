using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// IMonitor is the base contract to support Observer funtionalities,
    /// such as auto-triggering support exposed by the IDelegateCommand.
    /// </summary>
    public interface IMonitor
    {
        /// <summary>
        /// Occurs when the source monitored by this monitor changes.
        /// </summary>
        event EventHandler Changed;

        /// <summary>
        /// Asks this monitor to raise a change notification in order 
        /// to trigger all the listeners.
        /// </summary>
        void NotifyChanged();

        /// <summary>
        /// Stops all the monitoring operation.
        /// </summary>
        void StopMonitoring();
    }
}
