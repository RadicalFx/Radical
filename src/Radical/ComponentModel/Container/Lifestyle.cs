using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Defines the lifestyle of a component.
    /// </summary>
    [Obsolete("PuzzleContainer has been removed in v2.0.0. Related contracts will be removed in v3.0.0.")]
    public enum Lifestyle
    {
        /// <summary>
        /// Singleton lifestyle.
        /// </summary>
        Singleton = 0,

        /// <summary>
        /// Transient lifestyle.
        /// </summary>
        Transient
    }
}
