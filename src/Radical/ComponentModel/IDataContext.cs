using Radical.ComponentModel.QueryModel;
using System;
using System.Collections.Generic;

namespace Radical.ComponentModel
{
    /// <summary>
    /// An IDataContext represents an Unit of Work.
    /// </summary>
    [Contract]
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Insert( Object entity );

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update( Object entity );

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Save( Object entity );

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete( Object entity );

        /// <summary>
        /// Clears this instance removing all the cached entities, cancelling all pending saves, updates and deletes.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the specified entity is attached.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is attached; otherwise, <c>false</c>.
        /// </returns>
        bool IsAttached( Object entity );

        /// <summary>
        /// Detaches the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Detach( Object entity );

        ///// <summary>
        ///// Attaches the specified entity.
        ///// </summary>
        ///// <param name="entity">The entity.</param>
        //void Attach( Object entity );

        /// <summary>
        /// Gets a value indicating whether this instance has pending changes.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has pending changes; otherwise, <c>false</c>.
        /// </value>
        bool HasPendingChanges { get; }

        /// <summary>
        /// Begins a new transaction.
        /// </summary>
        /// <returns>The begun transaction.</returns>
        ITransaction BeginTransaction();


        /// <summary>
        /// Begins a new transaction with the specified isolation lavel.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns>The begun transaction.</returns>
        ITransaction BeginTransaction( System.Data.IsolationLevel isolationLevel );


        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        ITransaction Transaction { get; }

        /// <summary>
        /// Flushes all the the pending changes.
        /// </summary>
        void FlushChanges();

        /// <summary>
        /// Gets a list of entity that satifies the given query.
        /// </summary>
        /// <typeparam name="TSource">The type of the source entity.</typeparam>
        /// <typeparam name="TResult">The type of the expected result.</typeparam>
        /// <param name="querySpec">The query specification.</param>
        /// <returns>The list of entities that satifies the given query.</returns>
        IList<TResult> GetByQuery<TSource, TResult>( IQuerySpecification<TSource, TResult> querySpec );

        /// <summary>
        /// Gets a single value (a scalar value) that satisfies the given query.
        /// </summary>
        /// <typeparam name="TSource">The type of the source entity.</typeparam>
        /// <typeparam name="TResult">The type of the expected result.</typeparam>
        /// <param name="scalarSpec">The scalar specification.</param>
        /// <returns>The scalar value that satisfies the given query.</returns>
        TResult GetScalar<TSource, TResult>( IScalarSpecification<TSource, TResult> scalarSpec );

        /// <summary>
        /// Gets a list of entity that satifies the given generic specification.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the expected result.</typeparam>
        /// <param name="specification">The generic specification.</param>
        /// <returns>The list of entities that satifies the given generic specification.</returns>
        IEnumerable<TResult> GetBySpecification<TSource, TResult>( ISpecification<TSource, TResult> specification );

        /// <summary>
        /// Gets single entity given its key.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="key">The entity key.</param>
        /// <returns>The requested entity.</returns>
        T GetByKey<T>( Object key );

        /// <summary>
        /// Executes the specified command against the current data source using the underlying provider.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>
        /// The number of affected entities.
        /// </returns>
        int Execute<TCommand>( TCommand command ) where TCommand : IBatchCommand;
    }
}
