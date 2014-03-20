using System;
using System.Linq.Expressions;

namespace Topics.Radical.ComponentModel.QueryModel
{
	/// <summary>
	/// A query system is the main entry point to retrieve 
	/// query engine and scalar evaluator instances. A query
	/// system manager is not intended to be used directly by 
	/// the application, a query system manager is used by the 
	/// <see cref="IDataContext"/> infrastructure.
	/// </summary>
	public interface IQuerySystemManager
	{
		/// <summary>
		/// Gets the query engine for the given query.
		/// </summary>
		/// <typeparam name="TSource">The type of the source entity.</typeparam>
		/// <typeparam name="TResult">The type of the expected result entity.</typeparam>
		/// <typeparam name="TProvider">The type of the query provider/session/data context.</typeparam>
		/// <param name="querySpec">The query specication to search an engine for.</param>
		/// <returns>A query engine instance.</returns>
		/// <exception cref="SpecificationNotSupportedException">
		/// A <c>SpecificationNotSupportedException</c> is raised if the infrastructure cannot 
		/// find a suitable engine for the given specification.
		/// </exception>
		IQueryEngine<TSource, TResult, TProvider> GetQueryEngine<TSource, TResult, TProvider>( IQuerySpecification<TSource, TResult> querySpec );

		/// <summary>
		/// Gets the scalar evaluator for the given scalar specification.
		/// </summary>
		/// <typeparam name="TSource">The type of the source entity.</typeparam>
		/// <typeparam name="TResult">The type of the expected result entity.</typeparam>
		/// <typeparam name="TProvider">The type of the query provider/session/data context.</typeparam>
		/// <param name="scalarSpec">The scalar spec to search an engine for.</param>
		/// <returns>A scalar specification engine instance.</returns>
		/// <exception cref="SpecificationNotSupportedException">
		/// A <c>SpecificationNotSupportedException</c> is raised if the infrastructure cannot 
		/// find a suitable engine for the given specification.
		/// </exception>
		IScalarEvaluator<TSource, TResult, TProvider> GetScalarEvaluator<TSource, TResult, TProvider>( IScalarSpecification<TSource, TResult> scalarSpec );

		/// <summary>
		/// Gets the batch command engine.
		/// </summary>
		/// <typeparam name="TCommand">The type of the command.</typeparam>
		/// <typeparam name="TProvider">The type of the provider.</typeparam>
		/// <param name="command">The command.</param>
		/// <returns>
		/// The bengine responsible for the execution of the given command.
		/// </returns>
		IBatchCommandEngine<TCommand, TProvider> GetBatchCommandEngine<TCommand, TProvider>( TCommand command ) where TCommand : IBatchCommand;
	}
}
