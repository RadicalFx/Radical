namespace Radical.ComponentModel.ChangeTracking
{
    /// <summary>
    /// Represents a suggested action produced by
    /// the provisioning system of a change tracking service.
    /// </summary>
    public interface IAdvisedAction
    {
        /// <summary>
        /// Gets the suggested action for the target object encapsulated by this instance.
        /// </summary>
        /// <value>The suggested action.</value>
        ProposedActions Action { get; }

        /// <summary>
        /// Gets the target object of the suggested action.
        /// </summary>
        /// <value>The target object.</value>
        object Target { get; }
    }
}
