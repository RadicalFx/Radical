
namespace Topics.Radical.Windows.Phone.Shell.Services
{
	/// <summary>
	/// Defines the lifecycle events of a phone application.
	/// </summary>
	public enum LifecycleEvent
	{
		/// <summary>
		/// The app is starting.
		/// </summary>
		Starting,

		/// <summary>
		/// The app is resuming.
		/// </summary>
		Resuming,

		/// <summary>
		/// The app is closing.
		/// </summary>
		Closing,

		/// <summary>
		/// The app is suspending.
		/// </summary>
		Suspending,

		/// <summary>
		/// The app has been resumed.
		/// </summary>
		Resumed,

		/// <summary>
		/// The app has been unobscured.
		/// </summary>
		Unobscured,

		/// <summary>
		/// The app has been obscured.
		/// </summary>
		Obscured
	}
}
