using System;
using System.ComponentModel;

namespace Radical.Model
{
    public class RebuildIndexesEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets the index of the removed item that is causing a rebuild index request.
        /// </summary>
        /// <value>The index.</value>
        public Int32 Index { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebuildIndexesEventArgs"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        public RebuildIndexesEventArgs( Int32 index )
            : base( false )
        {
            this.Index = index;
        }
    }
}
