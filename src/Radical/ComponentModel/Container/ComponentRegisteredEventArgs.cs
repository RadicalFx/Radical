using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Defines event arguments for the ComponentRegistered event.
    /// </summary>
    public class ComponentRegisteredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentRegisteredEventArgs"/> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        public ComponentRegisteredEventArgs(IContainerEntry entry)
        {
            //Ensure.That( entry ).Named( "entry" ).IsNotNull();
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
