using System;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618
    /// <summary>
    /// Issue to notify that the application boot process has been completed.
    /// </summary>
    public class ApplicationBootCompleted : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBootCompleted"/> class.
        /// </summary>
        public ApplicationBootCompleted()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBootCompleted"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public ApplicationBootCompleted(Object sender)
            : base(sender)
        {

        }
    }
#pragma warning restore 0618
}
