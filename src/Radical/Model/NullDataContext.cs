using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radical.ComponentModel;

namespace Radical.Model
{
    /// <summary>
    /// An empty implementation of the <see cref="IDataContext"/> interface.
    /// </summary>
    public sealed class NullDataContext : IDataContext
    {
        /// <summary>
        /// A default singleton instance of the NullDataContext.
        /// </summary>
        public static readonly IDataContext Instance = new NullDataContext();

        private NullDataContext() { }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Insert( object entity )
        {

        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update( object entity )
        {

        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Save( object entity )
        {

        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete( object entity )
        {

        }

        /// <summary>
        /// Clears this instance removing all the cached entities, cancelling all pending saves, updates and deletes.
        /// </summary>
        public void Clear()
        {

        }

        /// <summary>
        /// Determines whether the specified entity is attached.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is attached; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAttached( object entity )
        {
            return false;
        }

        /// <summary>
        /// Detaches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Detach( object entity )
        {

        }

        /// <summary>
        /// Gets a value indicating whether this instance has pending changes.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has pending changes; otherwise, <c>false</c>.
        /// </value>
        public bool HasPendingChanges
        {
            get { return false; }
        }

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        /// <returns>The begun transaction.</returns>
        public ITransaction BeginTransaction()
        {
            return null;
        }

        /// <summary>
        /// Begins a new transaction with the specified isolation lavel.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>The begun transaction.</returns>
        public ITransaction BeginTransaction( System.Data.IsolationLevel isolationLevel )
        {
            return null;
        }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        public ITransaction Transaction
        {
            get { return null; }
        }

        /// <summary>
        /// Flushes all the the pending changes.
        /// </summary>
        public void FlushChanges()
        {

        }

        /// <summary>
        /// Gets a list of entity that satifies the given query.
        /// </summary>
        /// <typeparam name="TSource">The type of the source entity.</typeparam>
        /// <typeparam name="TResult">The type of the expected result.</typeparam>
        /// <param name="querySpec">The query specification.</param>
        /// <returns>
        /// The list of entities that satifies the given query.
        /// </returns>
        public IList<TResult> GetByQuery<TSource, TResult>( ComponentModel.QueryModel.IQuerySpecification<TSource, TResult> querySpec )
        {
            return new List<TResult>();
        }

        /// <summary>
        /// Gets a single value (a scalar value) that satisfies the given query.
        /// </summary>
        /// <typeparam name="TSource">The type of the source entity.</typeparam>
        /// <typeparam name="TResult">The type of the expected result.</typeparam>
        /// <param name="scalarSpec">The scalar specification.</param>
        /// <returns>
        /// The scalar value that satisfies the given query.
        /// </returns>
        public TResult GetScalar<TSource, TResult>( ComponentModel.QueryModel.IScalarSpecification<TSource, TResult> scalarSpec )
        {
            return default( TResult );
        }

        /// <summary>
        /// Gets a list of entity that satifies the given generic specification.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the expected result.</typeparam>
        /// <param name="specification">The generic specification.</param>
        /// <returns>
        /// The list of entities that satifies the given generic specification.
        /// </returns>
        public IEnumerable<TResult> GetBySpecification<TSource, TResult>( ComponentModel.QueryModel.ISpecification<TSource, TResult> specification )
        {
            yield return default( TResult );
        }

        /// <summary>
        /// Gets single entity given its key.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="key">The entity key.</param>
        /// <returns>The requested entity.</returns>
        public T GetByKey<T>( object key )
        {
            return default( T );
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>
        /// The number of affected rows.
        /// </returns>
        public int Execute<TCommand>( TCommand command ) where TCommand : ComponentModel.QueryModel.IBatchCommand
        {
            return 0;
        }
    }
}
