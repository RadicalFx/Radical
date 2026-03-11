using System.ComponentModel;

namespace Radical.Model
{
    /// <summary>
    /// Provides data for the rebuild indexes event, including the index of the item that triggered the rebuild request.
    /// </summary>
    public class RebuildIndexesEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets the index of the removed item that is causing a rebuild index request.
        /// </summary>
        /// <value>The index.</value>
        public int Index { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebuildIndexesEventArgs"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        public RebuildIndexesEventArgs(int index)
            : base(false)
        {
            Index = index;
        }
    }
}
