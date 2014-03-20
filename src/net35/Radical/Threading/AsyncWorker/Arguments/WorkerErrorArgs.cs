using System;

namespace Topics.Radical.Threading
{
	class WorkerErrorArgs : IAsyncErrorArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WorkerErrorArgs"/> class.
		/// </summary>
		/// <param name="error">The error.</param>
		public WorkerErrorArgs( Exception error )
		{
			this.Error = error;
		}

		/// <summary>
		/// Gets the error occured during the async execution.
		/// </summary>
		/// <value>The async error.</value>
		public Exception Error
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether 
		/// the caller has handled the async error.
		/// If handled is set to false the exception 
		/// is automatically rethrown by the worker.
		/// </summary>
		/// <value><c>true</c> if handled; otherwise, <c>false</c>.</value>
		public bool Handled
		{
			get;
			set;
		}
	}
}
