using System.ComponentModel;

namespace Radical.Model
{
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
            this.Index = index;
        }
    }
}
