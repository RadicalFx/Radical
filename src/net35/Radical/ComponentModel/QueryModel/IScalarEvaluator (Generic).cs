using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.ComponentModel.QueryModel
{
    /// <summary>
    /// Defines the contract of a scalar evaluator.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TProvider">The type of the provider.</typeparam>
    public interface IScalarEvaluator<TSource, TResult, TProvider>
    {
        /// <summary>
        /// Evaluates the specified scalar specification against the given provider.
        /// </summary>
        /// <param name="scalarSpec">The scalar specification to execute.</param>
        /// <param name="context">The current data context.</param>
        /// <param name="provider">The provider to use a data context.</param>
        /// <returns>The searched entity.</returns>
        TResult Evaluate( QueryModel.IScalarSpecification<TSource, TResult> scalarSpec, IDataContext context, TProvider provider );
    }

    /// <summary>
    /// Defines the contract of a scalar evaluator.
    /// </summary>
    /// <typeparam name="TScalar">The type of the scalar.</typeparam>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TProvider">The type of the provider.</typeparam>
    public interface IScalarEvaluator<TScalar, TSource, TResult, TProvider>
        : IScalarEvaluator<TSource, TResult, TProvider>
        where TScalar : QueryModel.IScalarSpecification<TSource, TResult>
    {
        /// <summary>
        /// Evaluates the specified scalar specification against the given provider.
        /// </summary>
        /// <param name="scalarSpec">The scalar specification to execute.</param>
        /// <param name="context">The current data context.</param>
        /// <param name="provider">The provider to use a data context.</param>
        /// <returns>The searched entity.</returns>
        TResult Evaluate( TScalar scalarSpec, IDataContext context, TProvider provider );
    }
}
