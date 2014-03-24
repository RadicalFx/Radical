namespace Topics.Radical.ComponentModel.ChangeTracking
{
	using System;

	/// <summary>
	/// ChangeRejectedArgs describes the change reject request and transport
	/// data containg detailed information about the rejected change.
	/// </summary>
	/// <typeparam name="T">The type of the cached value.</typeparam>
	public class ChangeRejectedEventArgs<T> : ChangeEventArgs<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ChangeRejectedEventArgs&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="cachedValue">The cached value.</param>
		/// <param name="source">The source.</param>
		/// <param name="reason">The reason.</param>
		public ChangeRejectedEventArgs( Object entity, T cachedValue, IChange source, RejectReason reason )
			: base( entity, cachedValue, source )
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
