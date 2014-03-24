using System;

namespace Topics.Radical.Threading
{
	/// <summary>
	/// Defines the event arguments for the AsynError event.
	/// </summary>
	public class AsyncErrorEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncErrorEventArgs"/> class.
		/// </summary>
		/// <param name="error">The error.</param>
		public AsyncErrorEventArgs( Exception error )
		{
			this.Error = error;
		}

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether the error has been is handled or not.
		/// </summary>
		/// <value><c>true</c> if the error has been handled; otherwise, <c>false</c>.</value>
		public Boolean Handled{ get;set; }
	}
}
