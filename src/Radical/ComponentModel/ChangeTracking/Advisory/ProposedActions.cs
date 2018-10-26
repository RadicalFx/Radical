namespace Radical.ComponentModel.ChangeTracking
{
    using System;

    /// <summary>
    /// A ProposedActions is a proposal by the change tracking provisioning system.
    /// This action is an ipothetical action that can be performed by an unit of work.
    /// </summary>
    [Flags]
    public enum ProposedActions
    {
        /// <summary>
        /// The provisioning system cannot determine 
        /// the best action for the given object.
        /// </summary>
        None = 0,

        /// <summary>
        /// The data for this object should be created because
        /// the object is marked as new
        /// </summary>
        Create = 1,

        /// <summary>
        /// The object is changed and the storage should be updated
        /// </summary>
        Update = 2,

        /// <summary>
        /// The object has been deleted and should be 
        /// deleted from the storage too.
        /// </summary>
        Delete = 4,

        /// <summary>
        /// The object has been deleted but being marked
        /// as new is a nonsense to remove itr from the
        /// underlying storage, simply dispose its instance
        /// </summary>
        Dispose = 8,
    }
}
