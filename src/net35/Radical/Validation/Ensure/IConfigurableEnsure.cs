using System;
using System.Linq.Expressions;

namespace Topics.Radical.Validation
{
	/// <summary>
	/// Defines an ensure ready to be configured.
	/// </summary>
	/// <typeparam name="T">The type this ensure is attached to.</typeparam>
	public interface IConfigurableEnsure<T> : IEnsure<T>
	{

		//On the phone sometimes generates a SecurityViolation exception.

		/// <summary>
		/// Identifies the name of the parameter that will be validated.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <returns>The Ensure instance for fluent interface usage.</returns>
		IEnsure<T> Named( Expression<Func<T>> parameterName );

		/// <summary>
		/// Identifies the name of the parameter that will be validated.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <returns>The Ensure instance for fluent interface usage.</returns>
		IEnsure<T> Named( String parameterName );
	}
}
