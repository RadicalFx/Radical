namespace Radical.Model
{
    public delegate void PropertyValueChanged<T>( PropertyValueChangedArgs<T> e );

    public class PropertyValueChangedArgs<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueChangedArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        public PropertyValueChangedArgs( T newValue, T oldValue )
        {
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        /// <value>The new value.</value>
        public T NewValue { get; private set; }

        /// <summary>
        /// Gets the old value.
        /// </summary>
        /// <value>The old value.</value>
        public T OldValue { get; private set; }
    }
}
