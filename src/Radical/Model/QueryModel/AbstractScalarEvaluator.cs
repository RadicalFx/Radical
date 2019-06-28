using Radical.ComponentModel.QueryModel;

namespace Radical.Model.QueryModel
{
    /// <summary>
    /// A base implementation of the IScalarSpecification interface.
    /// </summary>
    /// <typeparam name="TScalar">The type of the scalar.</typeparam>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TProvider">The type of the provider.</typeparam>
    public abstract class AbstractScalarEvaluator<TScalar, TSource, TResult, TProvider> :
        IScalarEvaluator<TScalar, TSource, TResult, TProvider>
        where TScalar : IScalarSpecification<TSource, TResult>
    {
        /// <summary>
        /// Evaluates the specified scalar specification against the given provider.
        /// </summary>
        /// <param name="scalarSpec">The scalar specification to execute.</param>
        /// <param name="context">The current data context.</param>
        /// <param name="provider">The provider to use a data context.</param>
        /// <returns>The searched entity.</returns>
        public abstract TResult Evaluate( TScalar scalarSpec, ComponentModel.IDataContext context, TProvider provider );

        /// <summary>
        /// Evaluates the specified scalar specification against the given provider.
        /// </summary>
        /// <param name="scalarSpec">The scalar specification to execute.</param>
        /// <param name="context">The current data context.</param>
        /// <param name="provider">The provider to use a data context.</param>
        /// <returns>The searched entity.</returns>
        public TResult Evaluate( IScalarSpecification<TSource, TResult> scalarSpec, ComponentModel.IDataContext context, TProvider provider )
        {
            return this.Evaluate( ( TScalar )scalarSpec, context, provider );
        }
    }
}
