using System;

namespace Topics.Radical.ComponentModel
{
	public interface IHaveTimestamp<T>
	{
		Timestamp<T> Timestamp { get; set; }
	}
}
