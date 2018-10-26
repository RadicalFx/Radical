using System;

namespace Radical.ComponentModel.ChangeTracking
{
    public interface IMementoPropertyMetadata
    {
        bool TrackChanges { get; set; }
    }
}
