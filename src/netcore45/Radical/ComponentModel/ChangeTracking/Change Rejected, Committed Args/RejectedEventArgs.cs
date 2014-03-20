namespace Topics.Radical.ComponentModel.ChangeTracking
{
	using System;

	/// <summary>
	/// Contains event data describing the change reject.
	/// </summary>
	public class RejectedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RejectedEventArgs"/> class.
		/// </summary>
		/// <param name="reason">The reason.</param>
		public RejectedEventArgs( RejectReason reason )
		{
			this.Reason = reason;
		}

		/// <summary>
		/// Gets the reason of the reject request.
		/// </summary>
		/// <value>The reason.</value>
		public RejectReason Reason
		{
			get;
			private set;
		}
	}
}
