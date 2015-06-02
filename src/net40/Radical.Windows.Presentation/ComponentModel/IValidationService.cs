using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Determines the reset behavior.
	/// </summary>
	[Flags]
	public enum ValidationResetBehavior
	{
		/// <summary>
		/// Resets only the errors collection.
		/// </summary>
		ErrorsOnly,

		/// <summary>
		/// Resets only the validation tracker that tracks if validation for properties has been called at least once.
		/// </summary>
		ValidationTracker,

		/// <summary>
		/// Resets both the validation tracker and the errors collection.
		/// </summary>
		All = ErrorsOnly | ValidationTracker
	}

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
		event EventHandler ValidationReset;

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
		/// Starts the validation process.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <param name="propertyName">The name of the property to validate.</param>
		/// <returns>The validation error message if any; otherwise a null or empty string.</returns>
		String ValidateRuleSet( String ruleSet, String propertyName );

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
		/// Clears the validation state resetting to it its default valid value.
		/// </summary>
		/// <param name="resetBehavior">The reset behavior.</param>
		void Reset(ValidationResetBehavior resetBehavior);

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

		/// <summary>
		/// Gets the display name of the property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">The entity.</param>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		String GetPropertyDisplayName<T>( T entity, Expression<Func<T, Object>> property );

		/// <summary>
		/// Gets the display name of the property.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		String GetPropertyDisplayName( Object entity, String propertyName );

		/// <summary>
		/// Gets or sets if the service should merge validation errors related to the same property.
		/// </summary>
		/// <value>
		/// <c>True</c> if the service should merge validation errors related to the same property; otherwise <c>False</c>.
		/// </value>
		Boolean MergeValidationErrors { get; set; }
	}
}
