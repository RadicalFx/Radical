using System;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618
    /// <summary>
	/// Domain event that identifies that a view model is closing.
	/// </summary>
	public class ViewModelClosing : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelClosing"/> class.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		public ViewModelClosing(Object viewModel)
		{
			this.ViewModel = viewModel;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelClosing"/> class.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="viewModel">The view model.</param>
		[Obsolete( "The Radical message broker now supports POCO messages, use the default contructor, will be removed in the next version.", false )]
		public ViewModelClosing( Object sender, Object viewModel )
			: base( sender )
		{
			this.ViewModel = viewModel;
		}

		/// <summary>
		/// Gets the view model.
		/// </summary>
		public Object ViewModel { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="ViewModelClosing"/> should be canceled.
		/// </summary>
		/// <value>
		///   <c>true</c> if cancel; otherwise, <c>false</c>.
		/// </value>
		public Boolean Cancel { get; set; }
    }

#pragma warning restore 0618
}
