namespace Radical.ComponentModel.ChangeTracking
{
    using System;

    /// <summary>
    /// ChangeArgs is the base class for data describing a change commit
    /// or a change reject, transport data containg detailed infos about 
    /// the rejected change.
    /// </summary>
    /// <typeparam name="T">The type of the cached value.</typeparam>
    public class ChangeEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cachedValue">The cached value.</param>
        /// <param name="source">The source.</param>
        public ChangeEventArgs( Object entity, T cachedValue, IChange source )
        {
            if( entity == null )
            {
                throw new ArgumentNullException( "entity" );
            }

            if( source == null )
            {
                throw new ArgumentNullException( "source" );
            }

            this.Entity = entity;
            this.CachedValue = cachedValue;
            this.Source = source;
        }

        /// <summary>
        /// Gets the changed entity.
        /// </summary>
        /// <value>The entity.</value>
        public Object Entity
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the cached value.
        /// </summary>
        /// <value>The cached value.</value>
        public T CachedValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the source change on witch the reject or 
        /// commit has been requested.
        /// </summary>
        /// <value>The source change.</value>
        public IChange Source
        {
            get;
            private set;
        }
    }
}
