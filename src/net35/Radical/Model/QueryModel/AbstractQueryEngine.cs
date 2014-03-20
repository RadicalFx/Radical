using System;
using Topics.Radical.ComponentModel.QueryModel;
using System.Collections.Generic;
using System.Linq;

namespace Topics.Radical.Model.QueryModel
{
	/// <summary>
	/// A base implementation of the IQuerySpecification interface.
	/// </summary>
	/// <typeparam name="TQuery">The type of the query.</typeparam>
	/// <typeparam name="TSource">The type of the source.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <typeparam name="TProvider">The type of the provider.</typeparam>
	public abstract class AbstractQueryEngine<TQuery, TSource, TResult, TProvider> :
		IQueryEngine<TQuery, TSource, TResult, TProvider>
		where TQuery : IQuerySpecification<TSource, TResult>
	{
		/// <summary>
		/// Executes the given query against the given provider.
		/// </summary>
		/// <param name="querySpec">The query specification to execute.</param>
		/// <param name="context">The current data context.</param>
		/// <param name="provider">The provider to use a data context.</param>
		/// <returns>A list of entities.</returns>
		public abstract IList<TResult> ExecuteQuery( TQuery querySpec, ComponentModel.IDataContext context, TProvider provider );

		/// <summary>
		/// Executes the given query against the given provider.
		/// </summary>
		/// <param name="querySpec">The query specification to execute.</param>
		/// <param name="context">The current data context.</param>
		/// <param name="provider">The provider to use a data context.</param>
		/// <returns>A list of entities.</returns>
		public IList<TResult> ExecuteQuery( IQuerySpecification<TSource, TResult> querySpec, ComponentModel.IDataContext context, TProvider provider )
		{
			return this.ExecuteQuery( ( TQuery )querySpec, context, provider );
		}
	}
}
