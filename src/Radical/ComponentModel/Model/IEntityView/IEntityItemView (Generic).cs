﻿
namespace Radical.ComponentModel
{
    /// <summary>
    /// Extends the <see cref="IEntityItemView"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the item encapsulated by this item view instance.</typeparam>
    public interface IEntityItemView<T> :
        IEntityItemView //where T : class
    {
        /// <summary>
        /// Gets the underlying item.
        /// </summary>
        /// <value>The underlying item.</value>
        new T EntityItem { get; }

        /// <summary>
        /// Gets the view that owns this instance.
        /// </summary>
        /// <value>The owner view.</value>
        new IEntityView<T> View { get; }
    }
}
