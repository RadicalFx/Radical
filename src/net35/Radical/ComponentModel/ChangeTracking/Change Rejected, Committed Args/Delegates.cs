namespace Topics.Radical.ComponentModel.ChangeTracking
{

	/// <summary>
	/// A pointer to a function to call in order to reject the value of a change.
	/// </summary>
	public delegate void RejectCallback<T>( ChangeRejectedEventArgs<T> value );

	/// <summary>
	/// A pointer to a function to call in order to commit the value of a change.
	/// </summary>
	public delegate void CommitCallback<T>( ChangeCommittedEventArgs<T> value );
}
