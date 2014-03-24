using System.Windows;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// A region that holds a single, replaceble, content.
	/// </summary>
	public interface IContentRegion : IRegion
	{
		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		DependencyObject Content { get; set; }
	}
}
