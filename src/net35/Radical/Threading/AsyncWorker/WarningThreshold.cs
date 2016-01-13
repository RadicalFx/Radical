using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Threading
{
    /// <summary>
    /// Defines the warning threshold behavior.
    /// </summary>
    public sealed class WarningThreshold
    {
        /// <summary>
        /// Gets or sets the threshold.
        /// </summary>
        /// <value>The threshold.</value>
        public TimeSpan Threshold { get; set; }

        /// <summary>
        /// Gets or sets the handler to invoke whenever
        /// the warning threshold is reached.
        /// </summary>
        /// <value>The warning threshold reached handler.</value>
        public Action Handler { get; set; }
    }
}
