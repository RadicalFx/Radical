using System;

namespace Topics.Radical.Validation
{
	/// <summary>
	/// Defines an ensure configured and attached on a type.
	/// </summary>
	/// <typeparam name="T">The type this ensure is attached to.</typeparam>
	public interface IEnsure<T>
	{
		/// <summary>
		/// Executes the specified action only if the <c>If</c> operation has been evaluated to <c>false</c>.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		IEnsure<T> Else( Action<T, String> action );

		/// <summary>
		/// Executes the specified action only if the <c>If</c> operation has been evaluated to <c>false</c>.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		IEnsure<T> Else( Action<T> action );

		/// <summary>
		/// Gets the full error message combining the user custom message, if any, 
		/// with the supplied validator message and with source stack information.
		/// </summary>
		/// <param name="validatorSpecificMessage">The validator specific message.</param>
		/// <returns>
		/// The error message.
		/// </returns>
		String GetFullErrorMessage( String validatorSpecificMessage );

		/// <summary>
		/// Gets the full error message including source stack information.
		/// </summary>
		/// <returns>
		/// The error message.
		/// </returns>
		String GetFullErrorMessage();
		
		/// <summary>
		/// Gets the currently inspected object value.
		/// </summary>
		/// <returns>The currently inspected object value.</returns>
		T GetValue();
		
		/// <summary>
		/// Gets the currently inspected object value castaed to specified type.
		/// </summary>
		/// <typeparam name="K">The type to cast the inspected object to, K must inherith from T.</typeparam>
		/// <returns>The currently inspected object value.</returns>
		K GetValue<K>() where K : T;

		/// <summary>
		/// Execute the given predicate and saves the result for later usage.
		/// </summary>
		/// <param name="predicate">The predicate to evaluate in order to establish if the operation resault is <c>true</c> or <c>false</c>.</param>
		/// <returns>The Ensure instance for fluent interface usage.</returns>
		IEnsure<T> If( Predicate<T> predicate );

		/// <summary>
		/// Ensure that the supplied object is equal to the currently inspected object.
		/// </summary>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		/// <exception cref="ArgumentException">An ArgumentException is raised if the object equality fails.</exception>
		IEnsure<T> Is( T value );

		/// <summary>
		/// Ensure that the supplied predicate returns false.
		/// </summary>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		/// <exception cref="ArgumentException">An ArgumentException is raised if the predicate result is true.</exception>
		IEnsure<T> IsFalse( Predicate<T> func );

		//IEnsure<T> IsFalse( Boolean condition );

		/// <summary>
		/// Ensure that the supplied object is not equal to the currently inspected object.
		/// </summary>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		/// <exception cref="ArgumentException">An ArgumentException is raised if the object equality does not fail.</exception>
		IEnsure<T> IsNot( T value );

		/// <summary>
		/// Ensure that the supplied predicate returns true.
		/// </summary>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		/// <exception cref="ArgumentException">An ArgumentException is raised if the predicate result is false.</exception>
		IEnsure<T> IsTrue( Predicate<T> func );

		//IEnsure<T> IsTrue( Boolean condition );

		/// <summary>
		/// Gets the name of the parameter to validate.
		/// </summary>
		/// <value>The name of the parameter.</value>
		String Name { get; }

		/// <summary>
		/// Executes the specified action only if the <c>If</c> operation has been evaluated to <c>true</c>.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		IEnsure<T> Then( Action<T, String> action );

		/// <summary>
		/// Executes the specified action only if the <c>If</c> operation has been evaluated to <c>true</c>.
		/// </summary>
		/// <param name="action">The action to execute.</param>
		/// <returns>The Ensure instance for fluent interface usage.</returns>
		IEnsure<T> Then( Action<T> action );

		/// <summary>
		/// Gets the user custom error message.
		/// </summary>
		/// <value>The error message.</value>
		String UserErrorMessage { get; }

		/// <summary>
		/// Gets the value of the validated parameter.
		/// </summary>
		/// <value>The value of the parameter.</value>
		T Value { get; }

		/// <summary>
		/// Specifies the custom user message to be used when raising exceptions.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>This ensure instance for fluent interface usage.</returns>
		IEnsure<T> WithMessage( String errorMessage );

		/// <summary>
		/// Specifies the custom user message to be used when raising exceptions.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="formatArgs">The format arguments.</param>
		/// <returns>
		/// This ensure instance for fluent interface usage.
		/// </returns>
		IEnsure<T> WithMessage( String errorMessage, params Object[] formatArgs );

		/// <summary>
		/// Throws the exception returned by the supplied exception
		/// builder only if the previous "If" check returns true.
		/// </summary>
		/// <param name="builder">The exception builder.</param>
		/// <returns>
		/// This ensure instance for fluent interface usage.
		/// </returns>
		IEnsure<T> ThenThrow( Func<IEnsure<T>, Exception> builder );

		/// <summary>
		/// Throws the specified exception.
		/// </summary>
		/// <param name="error">The exception to throw.</param>
		void Throw( Exception error );

        ///// <summary>
        ///// Injects the specified exception builder into the ensure engine.
        ///// </summary>
        ///// <param name="builder">The builder.</param>
        ///// <returns>
        ///// This ensure instance for fluent interface usage.
        ///// </returns>
        //IEnsure<T> WithException( Func<IEnsure<T>, Exception> builder );

		/// <summary>
		/// Allows the interception of ensure failures before the failure in order to
		/// log the error that will be raised.
		/// </summary>
		/// <param name="validationFailurePreview">The validation failure preview handler.</param>
		/// <returns>
		/// This ensure instance for fluent interface usage.
		/// </returns>
		IEnsure<T> WithPreview( Action<IEnsure<T>, Exception> validationFailurePreview );
	}
}
