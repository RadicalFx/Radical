using System;
using System.Windows;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618
    /// <summary>
    /// Domain event that identifies that a view has been loaded.
    /// </summary>
    public class ViewLoaded: Message
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewLoaded" /> class.
		/// </summary>
		/// <param name="view">The view.</param>
		public ViewLoaded(DependencyObject view)
		{
			this.View = view;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLoaded"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="view">The view.</param>
		[Obsolete( "The Radical message broker now supports POCO messages, use the default contructor, will be removed in the next version.", false )]
		public ViewLoaded( Object sender, DependencyObject view )
            : base(sender)
        {
            this.View = view;
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        public DependencyObject View { get; private set; }
    }
#pragma warning restore 0618
}
