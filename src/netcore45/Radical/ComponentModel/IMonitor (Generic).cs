namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Extends the basic IMonitor interface adding support to
	/// trace the monitored instance.
	/// </summary>
	/// <typeparam name="T">Ths monitored type.</typeparam>
	public interface IMonitor<T> : IMonitor
	{
		/// <summary>
		/// Gets the monitored source.
		/// </summary>
		/// <value>The monitored source.</value>
		T Source { get; }
	}
}