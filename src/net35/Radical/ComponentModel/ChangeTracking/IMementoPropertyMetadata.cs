using System;

namespace Topics.Radical.ComponentModel.ChangeTracking
{
	public interface IMementoPropertyMetadata
	{
		bool TrackChanges { get; set; }
	}
}
