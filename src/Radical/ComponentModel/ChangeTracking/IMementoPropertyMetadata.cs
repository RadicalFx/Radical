namespace Radical.ComponentModel.ChangeTracking
{
    /// <summary>
    /// Describes property metadata of a memento entity.
    /// </summary>
    public interface IMementoPropertyMetadata
    {
        /// <summary>
        /// Whether or not changes tracking should be enabled for this property.
        /// </summary>
        bool TrackChanges { get; set; }
    }
}
