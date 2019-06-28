using System;

namespace Radical
{
    /// <summary>
    /// Attribue specialized in adding description info to an enuration type.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes"), AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnumItemDescriptionAttribute : Attribute
    {
        readonly string _caption;
        readonly string _description;
        readonly int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="caption">The description.</param>
        public EnumItemDescriptionAttribute(string caption)
            : this(caption, string.Empty, -1)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="caption">The description.</param>
        /// <param name="index">The index.</param>
        public EnumItemDescriptionAttribute(string caption, int index)
            : this(caption, string.Empty, index)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumItemDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="description">The description.</param>
        /// <param name="index">The index.</param>
        public EnumItemDescriptionAttribute(string caption, string description, int index)
        {
            if (caption == null)
            {
                throw new ArgumentNullException("caption");
            }

            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            this._caption = caption;
            this._description = description;
            this._index = index;
        }

        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption
        {
            get { return this.OnGetCaption(this._caption); }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return this.OnGetDescription(this._description); }
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public virtual int Index
        {
            get { return this._index; }
        }

        /// <summary>
        /// Called by Caption getter, override this
        /// method to customize Caption return value
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <returns>
        /// The Caption value.
        /// </returns>
        protected virtual string OnGetCaption(string caption)
        {
            return caption;
        }

        /// <summary>
        /// Called by Description getter, override this
        /// method to customize Description return value
        /// </summary>
        /// <returns>The Description value.</returns>
        protected virtual string OnGetDescription(string description)
        {
            return description;
        }
    }
}
