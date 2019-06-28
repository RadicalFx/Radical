using System.Collections.Generic;

namespace Radical.ComponentModel.QueryModel
{
    /// <summary>
    /// Defines the contract of a query engine.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TProvider">The type of the provider.</typeparam>
    public interface IQueryEngine<TSource, TResult, TProvider>
    {
        /// <summary>
        /// Executes the given query against the given provider.
        /// </summary>
        /// <param name="querySpec">The query specification to execute.</param>
        /// <param name="context">The current data context.</param>
        /// <param name="provider">The provider to use a data context.</param>
        /// <returns>A list of entities.</returns>
        IList<TResult> ExecuteQuery( QueryModel.IQuerySpecification<TSource, TResult> querySpec, IDataContext context, TProvider provider );
    }

    /// <summary>
    /// Defines the contract of a query engine.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TProvider">The type of the provider.</typeparam>
    public interface IQueryEngine<TQuery, TSource, TResult, TProvider>
        : IQueryEngine<TSource, TResult, TProvider>
        where TQuery : QueryModel.IQuerySpecification<TSource, TResult>
    {
        /// <summary>
        /// Executes the given query against the given provider.
        /// </summary>
        /// <param name="querySpec">The query specification to execute.</param>
        /// <param name="context">The current data context.</param>
        /// <param name="provider">The provider to use a data context.</param>
        /// <returns>A list of entities.</returns>
        IList<TResult> ExecuteQuery( TQuery querySpec, IDataContext context, TProvider provider );
    }
}
