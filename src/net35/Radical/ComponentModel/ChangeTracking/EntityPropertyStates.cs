using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.ComponentModel.ChangeTracking
{
    /// <summary>
    /// Reports entity properties states.
    /// </summary>
    [Flags]
    public enum EntityPropertyStates
    {
        /// <summary>
        /// The property has no tracked states, thus is not changed either.
        /// </summary>
        None = 0,

        /// <summary>
        /// The property has tracked states, thus has changes.
        /// </summary>
        Changed = 1,

        /// <summary>
        /// The property has tracked states and the actual value is different from the original one.
        /// </summary>
        ValueChanged = 2,
    }
}
