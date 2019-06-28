namespace Radical.Model
{
    public class SetValueAtEventArgs<T> : InsertEventArgs<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetValueAtEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        public SetValueAtEventArgs(int index, T newValue, T oldValue)
            : base(index, newValue)
        {
            this.OldValue = oldValue;
        }

        /// <summary>
        /// Gets the old value.
        /// </summary>
        /// <value>The old value.</value>
        public T OldValue { get; private set; }
    }
}
