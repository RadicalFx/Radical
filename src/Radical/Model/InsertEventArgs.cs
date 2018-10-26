namespace Radical.Model
{
    using System;
    using System.ComponentModel;

    public class InsertEventArgs<T> : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="newValue">The new value.</param>
        public InsertEventArgs( Int32 index, T newValue )
            : base( false )
        {
            this.NewValue = newValue;
            this.Index = index;
        }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        /// <value>The new value.</value>
        public T NewValue{ get; private set; }
        
        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public Int32 Index { get; private set; }
    }
}
