using Radical.Validation;

namespace Radical.DataBinding
{
    /// <summary>
    /// Represents a mean to bind enumeration types to controls treating enumeration types
    /// as a common IList of items.
    /// </summary>
    public class EnumBinder<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        public EnumBinder(EnumItemDescriptionAttribute attribute, T value)
            : this(attribute?.Caption, value, attribute?.Index ?? -1)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="value">The value.</param>
        public EnumBinder(string caption, T value)
            : this(caption, string.Empty, value, -1)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        public EnumBinder(string caption, T value, int index)
            : this(caption, string.Empty, value, index)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        public EnumBinder(string caption, string description, T value, int index)
        {
            Ensure.That(caption).Named("caption").IsNotNull();
            Ensure.That(description).Named("description").IsNotNull();
            Ensure.That(value).Named("value").IsFalse(o => o == null);

            Caption = caption;
            Description = description;
            Value = value;
            Index = index;
        }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get;
            private set;
        }
    }
}
