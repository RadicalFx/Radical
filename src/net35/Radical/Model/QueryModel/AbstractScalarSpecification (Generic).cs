using Topics.Radical.ComponentModel.QueryModel;

namespace Topics.Radical.Model.QueryModel
{
	/// <summary>
	/// A base IScalarSpecification.
	/// </summary>
	/// <typeparam name="TSource">The type of the source.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	public abstract class AbstractScalarSpecification<TSource, TResult> : IScalarSpecification<TSource, TResult>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractScalarSpecification&lt;TSource, TResult&gt;"/> class.
		/// </summary>
		protected AbstractScalarSpecification()
		{
			
		}
	}
}
