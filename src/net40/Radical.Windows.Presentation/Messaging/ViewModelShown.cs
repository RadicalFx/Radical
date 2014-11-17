using System;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618

    /// <summary>
	/// Domain event that identifies that a view model has been shown.
	/// </summary>
	public class ViewModelShown : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelShown"/> class.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		public ViewModelShown( Object viewModel )
		{
			this.ViewModel = viewModel;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelShown"/> class.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="viewModel">The view model.</param>
		[Obsolete( "The Radical message broker now supports POCO messages, use the default contructor, will be removed in the next version.", false )]
		public ViewModelShown( Object sender, Object viewModel )
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
