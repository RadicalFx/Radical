using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Phone.Shell.Services
{
#pragma warning disable 0618

    /// <summary>
	/// Defines the message that notifies a change in the app lifecycle.
	/// </summary>
	public class LifecycleMessage : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LifecycleMessage"/> class.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="evt">The evt.</param>
		public LifecycleMessage( object sender, LifecycleEvent evt )
			: base( sender )
		{
			this.Event = evt;
		}

		/// <summary>
		/// Gets the event.
		/// </summary>
		public LifecycleEvent Event { get; private set; }
    }
#pragma warning restore 0618
}
