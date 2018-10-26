using Radical.ComponentModel.QueryModel;

namespace Radical.Model.QueryModel
{
    /// <summary>
    /// A base query specification.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class AbstractQuerySpecification<TSource, TResult> : IQuerySpecification<TSource, TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractQuerySpecification&lt;TSource, TResult&gt;"/> class.
        /// </summary>
        protected AbstractQuerySpecification()
        {
            
        }
    }
}
