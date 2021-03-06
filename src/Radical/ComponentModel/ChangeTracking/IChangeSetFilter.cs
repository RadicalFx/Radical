﻿namespace Radical.ComponentModel.ChangeTracking
{
    /// <summary>
    /// An instance of an <c>IChangeSetFilter</c> is a class 
    /// used to filter <see cref="IChange"/>(s) that has to be
    /// included in an <see cref="IChangeSet"/>.
    /// </summary>
    public interface IChangeSetFilter
    {
        /// <summary>
        /// Determines if the supplied IChange should be
        /// included in the built IChangeSet.
        /// </summary>
        /// <param name="change">The change to evaluate.</param>
        /// <returns></returns>
        bool ShouldInclude(IChange change);
    }
}
