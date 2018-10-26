namespace Radical.ComponentModel.ChangeTracking
{
    using System;

    /// <summary>
    /// An instance of an <c>IChangeSetFilter</c> is a class 
    /// used to filter <see cref="IChange"/>(s) that has to be
    /// included in an <see cref="IChangeSet"/>.
    /// </summary>
    public interface IChangeSetFilter
    {
        /// <summary>
        /// Determines if the supplied IChange should be
        /// included in the builded IChangeSet.
        /// </summary>
        /// <param name="change">The change to evaluate.</param>
        /// <returns></returns>
        Boolean ShouldInclude( IChange change );
    }
}
