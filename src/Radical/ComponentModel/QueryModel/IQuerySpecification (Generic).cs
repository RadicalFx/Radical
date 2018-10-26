namespace Radical.ComponentModel.QueryModel
{
    /// <summary>
    /// Defines a specification with support for projections.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TResult">The type of the expected result.</typeparam>
    public interface IQuerySpecification<TSource, TResult> :
        ISpecification<TSource, TResult>
    {

    }

    /// <summary>
    /// Defines a specification that does not require a projection
    /// </summary>
    /// <typeparam name="T">The type of the source and of the expected result.</typeparam>
    public interface IQuerySpecification<T> : IQuerySpecification<T, T>
    {

    }
}
