namespace Radical.ComponentModel.ChangeTracking
{
    using System;

    /// <summary>
    /// Determines the tracking state of an object.
    /// </summary>
    [Flags]
    public enum EntityTrackingStates
    {
        /// <summary>
        /// The state of the entity is not changed, 
        /// the entity is not transient or the entity
        /// is not tracked.
        /// </summary>
        None =0,

        /// <summary>
        /// The entity is transient.
        /// </summary>
        IsTransient = 1,

        /// <summary>
        /// if an entity is marked as <c>AutoRemove</c> (the default behavior) and RejectChanges,
        /// or an Undo that removes the last IChange of the entity, is called then the entity is 
        /// automatically removed from the list of the transient entities.
        /// </summary>
        AutoRemove = 2,

        /// <summary>
        /// The entity is changed and has changes that can be undone.
        /// </summary>
        HasBackwardChanges = 4,

        /// <summary>
        /// The entity has changes that can be reapplied.
        /// </summary>
        HasForwardChanges = 8,
    }
}
