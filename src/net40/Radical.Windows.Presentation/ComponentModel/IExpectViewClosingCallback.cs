#if !SILVERLIGHT
using System.ComponentModel;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Defines that a ViewModel expects lifecycle notifications from the view.
	/// </summary>
	public interface IExpectViewClosingCallback
	{
		/// <summary>
		/// Called when the view is closing.
		/// </summary>
		/// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
		void OnViewClosing( CancelEventArgs e );
	}
}
#endif