namespace Radical.ChangeTracking
{
    using System.Collections.Generic;
    using Radical.Collections;
    using Radical.ComponentModel.ChangeTracking;

    /// <summary>
    /// A change set is a readonly list of <see cref="IChange"/> instance.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" )]
    public class ChangeSet : ReadOnlyCollection<IChange>, IChangeSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeSet"/> class.
        /// </summary>
        /// <param name="changes">The changes.</param>
        public ChangeSet( IEnumerable<IChange> changes )
            : base( changes )
        {

        }
    }
}
