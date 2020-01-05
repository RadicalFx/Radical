using System.ComponentModel;

namespace Radical.Model
{
    public class AddingNewEventArgs<T> : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddingNewEventArgs&lt;T&gt;"/> class.
        /// </summary>
        public AddingNewEventArgs()
        {

        }

        /// <summary>
        /// Gets or sets the new item.
        /// </summary>
        /// <item>The new item.</item>
        public T NewItem
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to 
        /// automatically call EndNew immediately after
        /// the addition of the new item.
        /// </summary>
        /// <value><c>True</c> to auto commit; otherwise, <c>false</c>.</value>
        public bool AutoCommit { get; set; }
    }
}
