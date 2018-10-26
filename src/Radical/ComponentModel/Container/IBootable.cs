using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Identifies a component the can be booted.
    /// </summary>
    public interface IBootable
    {
        /// <summary>
        /// Boots this instance.
        /// </summary>
        void Boot();
    }
}
