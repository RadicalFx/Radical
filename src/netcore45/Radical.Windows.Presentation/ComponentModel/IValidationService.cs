using System;
using System.Collections.Generic;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Defines a validation service that can be used to validate an entity or a ViewModel.
	/// </summary>
	public interface IValidationService
	{
		/// <summary>
		/// Gets a value indicating whether the validation process
		/// returns a valid response or not.
		/// </summary>
		/// <value><c>true</c> if the validation process has successfully passed the validation process.; otherwise, <c>false</c>.</value>
		Boolean IsValid { get; }

		/// <summary>
		/// Occurs when validation status changes.
		/// </summary>
		event EventHandler StatusChanged;

		/// <summary>
		/// Occurs when this service is resetted.
		/// </summary>
		event EventHandler Resetted;

		/// <summary>
		/// Gets the invalid properties.
		/// </summary>
		/// <returns>A list of property names that identifies the invalid properties.</returns>
		IEnumerable<String> GetInvalidProperties();

		/// <summary>
		/// Starts the validation process.
		/// </summary>
		/// <returns><c>True</c> if the validation process succedeed; otherwise <c>false</c>.</returns>
		Boolean Validate();

		/// <summary>
		/// Starts the validation process.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <returns>
		///   <c>True</c> if the validation process succedeed; otherwise <c>false</c>.
		/// </returns>
		Boolean ValidateRuleSet( String ruleSet );

		/// <summary>
		/// Validates the specified property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>The validation error message if any; otherwise a null or empty string.</returns>
		String Validate( String propertyName );

		/// <summary>
		/// Gets the validation errors.
		/// </summary>
		/// <value>All the validation errors.</value>
		IEnumerable<ValidationError> ValidationErrors { get; }

		/// <summary>
		/// Clears the validation state resetting to it its default valid value.
		/// </summary>
		void Reset();

		/// <summary>
		/// Gets a value indicating whether the validation process is suspended.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the validation process is suspended; otherwise, <c>false</c>.
		/// </value>
		Boolean IsValidationSuspended { get; }

		/// <summary>
		/// Suspends the validation.
		/// </summary>
		/// <returns>A disposable instance to automatically resume validation on dispose.</returns>
		IDisposable SuspendValidation();

		/// <summary>
		/// Resumes the validation.
		/// </summary>
		void ResumeValidation();
	}
}
