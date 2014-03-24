
namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Defines that a ViewModel expects lifecycle notifications from the view.
	/// </summary>
	public interface IExpectViewLoadedCallback
	{
		/// <summary>
		/// Called when the view is loaded.
		/// </summary>
		void OnViewLoaded();
	}
}
