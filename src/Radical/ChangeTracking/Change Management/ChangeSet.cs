using Radical.ComponentModel.ChangeTracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Radical.ChangeTracking
{
    /// <summary>
    /// A change set is a read-only list of <see cref="IChange"/> instance.
    /// </summary>
    public class ChangeSet : ReadOnlyCollection<IChange>, IChangeSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeSet"/> class.
        /// </summary>
        /// <param name="changes">The changes.</param>
        public ChangeSet(IList<IChange> changes)
            : base(changes)
        {

        }
    }
}
