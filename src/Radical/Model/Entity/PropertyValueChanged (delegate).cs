namespace Radical.Model
{
    /// <summary>
    /// Represents the method that handles a property value changed notification for a strongly typed property.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="e">An object containing the old and new property values.</param>
    public delegate void PropertyValueChanged<T>(PropertyValueChangedArgs<T> e);

    /// <summary>
    /// Provides data for a property value changed notification, carrying the old and new values.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    public class PropertyValueChangedArgs<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueChangedArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <param name="oldValue">The old value.</param>
        public PropertyValueChangedArgs(T newValue, T oldValue)
        {
            NewValue = newValue;
            OldValue = oldValue;
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
