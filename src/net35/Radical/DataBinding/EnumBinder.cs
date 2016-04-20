using System;
using Topics.Radical.Validation;

namespace Topics.Radical.DataBinding
{
    /// <summary>
    /// Represents a mean to bind enuration types to controls treating enumeration types
    /// as a common IList of items.
    /// </summary>
    public class EnumBinder<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="value">The value.</param>
        public EnumBinder( EnumItemDescriptionAttribute attribute, T value )
            : this( attribute.Caption, value, attribute.Index )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="value">The value.</param>
        public EnumBinder( String caption, T value )
            : this( caption, String.Empty, value, -1 )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        public EnumBinder( String caption, T value, Int32 index )
            : this( caption, String.Empty, value, index )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBinder&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="description">The description.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index.</param>
        public EnumBinder( String caption, String description, T value, Int32 index )
        {
            Ensure.That( caption ).Named( "caption" ).IsNotNull();
            Ensure.That( description ).Named( "description" ).IsNotNull();
            Ensure.That( value ).Named( "value" ).IsFalse( o => o == null );

            this.Caption = caption;
            this.Description = description;
            this.Value = value;
            this.Index = index;
        }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public String Caption
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public String Description
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
        public Int32 Index
        {
            get;
            private set;
        }
    }
}
