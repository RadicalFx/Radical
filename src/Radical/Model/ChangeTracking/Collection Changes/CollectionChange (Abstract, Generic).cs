﻿using Radical.ComponentModel.ChangeTracking;

namespace Radical.ChangeTracking.Specialized
{
    /// <summary>
    /// A base, abstract, change class specific for a collection change.
    /// </summary>
    public abstract class CollectionChange<TDescriptor> : Change<TDescriptor>
        where TDescriptor : CollectionChangeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChange&lt;TDescriptor&gt;"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="descriptor">The descriptor.</param>
        /// <param name="rejectCallback">The reject callback.</param>
        /// <param name="commitCallback">The commit callback.</param>
        /// <param name="description">The description.</param>
        protected CollectionChange(object owner, TDescriptor descriptor, RejectCallback<TDescriptor> rejectCallback, CommitCallback<TDescriptor> commitCallback, string description)
            : base(owner, descriptor, rejectCallback, commitCallback, description)
        {

        }

        /// <summary>
        /// Gets the descriptor of this change.
        /// </summary>
        /// <value>The descriptor instance.</value>
        public TDescriptor Descriptor
        {
            get { return CachedValue; }
        }
    }
}