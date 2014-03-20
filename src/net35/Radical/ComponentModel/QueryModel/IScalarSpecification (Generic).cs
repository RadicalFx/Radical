using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.ComponentModel.QueryModel
{
	/// <summary>
	/// Defines a scalar specification with support for projections.
	/// </summary>
	/// <remarks>A scalar specification is expected to return a single result.</remarks>
	/// <typeparam name="TSource">The type of the source.</typeparam>
	/// <typeparam name="TResult">The type of the expected result.</typeparam>
	public interface IScalarSpecification<TSource, TResult> : 
		ISpecification<TSource, TResult>
	{

	}

	/// <summary>
	/// Defines a scalar specification that does not require a projection
	/// </summary>
	/// <remarks>A scalar specification is expected to return a single result.</remarks>
	/// <typeparam name="T">The type of the source and of the expected result.</typeparam>
	public interface IScalarSpecification<T> : IScalarSpecification<T, T>
	{

	}
}
