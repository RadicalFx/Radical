namespace Radical.ComponentModel.ChangeTracking
{

    /// <summary>
    /// A pointer to a function to call in order to reject the value of a change.
    /// </summary>
    /// <typeparam name="T">The type of the value to reject.</typeparam>
    /// <param name="value">The <see cref="ChangeRejectedEventArgs{T}"/> instance containing the rejection data.</param>
    public delegate void RejectCallback<T>(ChangeRejectedEventArgs<T> value);

    /// <summary>
    /// A pointer to a function to call in order to commit the value of a change.
    /// </summary>
    /// <typeparam name="T">The type of the value to commit.</typeparam>
    /// <param name="value">The <see cref="ChangeCommittedEventArgs{T}"/> instance containing the commit data.</param>
    public delegate void CommitCallback<T>(ChangeCommittedEventArgs<T> value);
}
