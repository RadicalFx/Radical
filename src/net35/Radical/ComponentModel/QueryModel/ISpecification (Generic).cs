namespace Topics.Radical.ComponentModel.QueryModel
{
	/// <summary>
	/// Represents the base interface for all the specification: 
	/// inheritors are expected to inherit from more specific contracts.
	/// </summary>
	/// <typeparam name="TSource">The type of the source.</typeparam>
	/// <typeparam name="TResult">The type of the expected result.</typeparam>
	public interface ISpecification<TSource, TResult>
	{

	}
}
