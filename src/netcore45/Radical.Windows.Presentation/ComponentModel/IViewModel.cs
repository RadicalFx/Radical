using System.ComponentModel;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Defines the base contract that a Topics.Presentation
	/// ViewModel must respect.
	/// </summary>
	public interface IViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Gets or sets the view. The view property is intended only for
		/// infrastructural purpose. It is required to hold the one-to-one
		/// relation beteewn the view and the view model.
		/// </summary>
		/// <value>
		/// The view.
		/// </value>
		DependencyObject View { get; set; }
	}
}
