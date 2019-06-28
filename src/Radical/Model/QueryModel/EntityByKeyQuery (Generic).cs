using Radical.ComponentModel;
using Radical.Validation;

namespace Radical.Model.QueryModel
{
    /// <summary>
    /// Defines a scalar query to retrieve an entity given its key.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class EntityByKeyQuery<TSource, TResult> : AbstractScalarSpecification<TSource, TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityByKeyQuery&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public EntityByKeyQuery(IKey key)
        {
            Ensure.That(key).Named("key").IsNotNull();

            this.Key = key;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public IKey Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns a <see cref="System.string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Resources.Labels.EntityByKeyQuery;
        }
    }
}
