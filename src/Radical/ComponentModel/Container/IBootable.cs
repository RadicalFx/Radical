using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Identifies a component the can be booted.
    /// </summary>
    [Obsolete("PuzzleContainer has been removed in v2.0.0. Related contracts will be removed in v3.0.0.")]
    public interface IBootable
    {
        /// <summary>
        /// Boots this instance.
        /// </summary>
        void Boot();
    }
}
