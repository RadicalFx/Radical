namespace Radical.ComponentModel.ChangeTracking
{

    /// <summary>
    /// Reports the reason of a change reject.
    /// </summary>
    public enum RejectReason
    {
        /// <summary>
        /// None is a default not supported value. The usage of null 
        /// should raise an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        None = 0,

        /// <summary>
        /// The reject is due to an Undo request.
        /// </summary>
        Undo,

        /// <summary>
        /// The reject is due to a Redo request.
        /// </summary>
        Redo,

        /// <summary>
        /// The reject is due to a reject changes request.
        /// </summary>
        RejectChanges,

        /// <summary>
        /// The reject is due to a revert to bookmark request.
        /// </summary>
        Revert
    }

    /// <summary>
    /// Reports the reason of a change commmit.
    /// </summary>
    public enum CommitReason
    {
        /// <summary>
        /// None is a default not supported value. The usage of null 
        /// should raise an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// The commit is due to an accept change request.
        /// </summary>
        AcceptChanges
    }
}
