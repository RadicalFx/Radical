using System;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618
    /// <summary>
	/// Domain event that identifies that a view model has been activated.
	/// </summary>
	public class ViewModelActivated : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelActivated"/> class.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="viewModel">The view model.</param>
        public ViewModelActivated( Object sender, Object viewModel )
			: base( sender )
		{
			this.ViewModel = viewModel;
		}

		/// <summary>
		/// Gets the view model.
		/// </summary>
		public Object ViewModel { get; private set; }
    }

#pragma warning restore 0618
}
