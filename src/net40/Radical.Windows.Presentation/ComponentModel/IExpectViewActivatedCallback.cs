
namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Defines that a ViewModel expects lifecycle notifications from the view.
	/// </summary>
	public interface IExpectViewActivatedCallback
	{
		/// <summary>
		/// Called when the view is activated.
		/// </summary>
		void OnViewActivated();
	}
}