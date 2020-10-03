using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Defines event arguments for the ComponentRegistered event.
    /// </summary>
    [Obsolete("PuzzleContainer has been removed in v2.0.0. Related contracts will be removed in v3.0.0.")]
    public class ComponentRegisteredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentRegisteredEventArgs"/> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public ComponentRegisteredEventArgs(IContainerEntry entry)
        {
            Entry = entry;
        }

        /// <summary>
        /// Gets the entry.
        /// </summary>
        public IContainerEntry Entry
        {
            get;
            private set;
        }
    }
}
