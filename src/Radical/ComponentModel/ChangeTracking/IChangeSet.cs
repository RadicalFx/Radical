using System.Collections.Generic;

namespace Radical.ComponentModel.ChangeTracking
{

    /// <summary>
    /// A change set is a read-only list of <see cref="IChange"/> instances.
    /// </summary>
    public interface IChangeSet : IReadOnlyCollection<IChange>
    {

    }
}
