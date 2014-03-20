namespace Topics.Radical.Model.Providers
{
	using System;
	using Topics.Radical.Validation;
	using System.Reflection;
	using Topics.Radical.ComponentModel.QueryModel;
	using Topics.Radical.Reflection;

	/// <summary>
	/// Default <see cref="IQuerySystemManager"/> implementation.
	/// </summary>
	public class QuerySystemManager : IQuerySystemManager
	{
		readonly IServiceProvider container;

		/// <summary>
		/// Initializes a new instance of the <see cref="QuerySystemManager"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public QuerySystemManager( IServiceProvider container )
		{
			Ensure.That( container ).Named( "container" ).IsNotNull();

			this.container = container;
		}

		/// <summary>
		/// Gets the query engine for the given query.
		/// </summary>
		/// <typeparam name="TSource">The type of the source entity.</typeparam>
		/// <typeparam name="TResult">The type of the expected result entity.</typeparam>
		/// <typeparam name="TProvider">The type of the query provider/session/data context.</typeparam>
		/// <param name="querySpec">The query specication to search an engine for.</param>
		/// <returns>
		/// A query engine instance.
		/// </returns>
		/// <exception cref="SpecificationNotSupportedException">
		/// A <c>SpecificationNotSupportedException</c> is raised if the infrastructure cannot
		/// find a suitable engine for the given specification.
		///   </exception>
		public IQueryEngine<TSource, TResult, TProvider> GetQueryEngine<TSource, TResult, TProvider>( IQuerySpecification<TSource, TResult> querySpec )
		{
			Ensure.That( querySpec ).Named( "querySpec" ).IsNotNull();

			var specType = querySpec.GetType();
			var queryEngineType = typeof( IQueryEngine<,,,> )
				.MakeGenericType(
					specType,
					typeof( TSource ),
					typeof( TResult ),
					typeof( TProvider ) );

			var queryEngine = this.container.GetService( queryEngineType );
			if( queryEngine == null )
			{
				var message = String.Format( "Unsupported specification: {1}, cannot find any QueryEngine for the given query.{0}{0}Query full type name: {2}", Environment.NewLine, specType.ToString( "sn" ), specType.FullName );
				throw new SpecificationNotSupportedException( message );
			}

			return queryEngine as IQueryEngine<TSource, TResult, TProvider>;
		}

		/// <summary>
		/// Gets the scalar evaluator for the given scalar specification.
		/// </summary>
		/// <typeparam name="TSource">The type of the source entity.</typeparam>
		/// <typeparam name="TResult">The type of the expected result entity.</typeparam>
		/// <typeparam name="TProvider">The type of the query provider/session/data context.</typeparam>
		/// <param name="scalarSpec">The scalar spec to search an engine for.</param>
		/// <returns>
		/// A scalar specification engine instance.
		/// </returns>
		/// <exception cref="SpecificationNotSupportedException">
		/// A <c>SpecificationNotSupportedException</c> is raised if the infrastructure cannot
		/// find a suitable engine for the given specification.
		///   </exception>
		public IScalarEvaluator<TSource, TResult, TProvider> GetScalarEvaluator<TSource, TResult, TProvider>( IScalarSpecification<TSource, TResult> scalarSpec )
		{
			Ensure.That( scalarSpec ).Named( "scalarSpec" ).IsNotNull();

			var specType = scalarSpec.GetType();
			var scalarEvaluatorType = typeof( IScalarEvaluator<,,,> )
				.MakeGenericType(
					specType,
					typeof( TSource ),
					typeof( TResult ),
					typeof( TProvider ) );

			var scalarEvaluator = this.container.GetService( scalarEvaluatorType );
			if( scalarEvaluator == null )
			{
				var message = String.Format( "Unsupported specification: {1}, cannot find any ScalarEvaluator for the given query.{0}{0}Query full type name: {2}", Environment.NewLine, specType.ToString( "sn" ), specType.FullName );
				throw new SpecificationNotSupportedException( message );
			}

			return scalarEvaluator as IScalarEvaluator<TSource, TResult, TProvider>;
		}


		/// <summary>
		/// Gets the batch command engine.
		/// </summary>
		/// <typeparam name="TCommand">The type of the command.</typeparam>
		/// <typeparam name="TProvider">The type of the provider.</typeparam>
		/// <param name="command">The command.</param>
		/// <returns>
		/// The bengine responsible for the execution of the given command.
		/// </returns>
		public IBatchCommandEngine<TCommand, TProvider> GetBatchCommandEngine<TCommand, TProvider>( TCommand command ) where TCommand : IBatchCommand
		{
			Ensure.That( command ).Named( "command" ).IsNot( default( TCommand ) );

			var cmdType = command.GetType();
			var engineType = typeof( IBatchCommandEngine<,> )
				.MakeGenericType( 
					cmdType, 
					typeof( TProvider ) );

			var engine = this.container.GetService( engineType );
			if( engine == null )
			{
				var message = String.Format( "Unsupported batch command: {1}, cannot find any Engine for the given command.{0}{0}Command full type name: {2}", Environment.NewLine, cmdType.ToString( "sn" ), cmdType.FullName );
				throw new SpecificationNotSupportedException( message );
			}

			return engine as IBatchCommandEngine<TCommand, TProvider>;
		}
	}
}
