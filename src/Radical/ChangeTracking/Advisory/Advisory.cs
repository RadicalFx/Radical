using Radical.ComponentModel.ChangeTracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Radical.ChangeTracking
{
    /// <summary>
    /// An advisory is a read-only list of <c>IAdvisedAction</c>(s) that describes
    /// the action that the change tracking provisioning engine suggest that should
    /// be executed in order to persist the occurred changes.
    /// </summary>
    public class Advisory : ReadOnlyCollection<IAdvisedAction>, IAdvisory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Advisory"/> class.
        /// </summary>
        /// <param name="actions">The actions.</param>
        public Advisory(IList<IAdvisedAction> actions)
            : base(actions)
        {

        }
    }
}
