using System;
using System.Collections.Generic;
using System.Linq;
using Topics.Radical.ComponentModel.Validation;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Presentation.Services.Validation
{
	/// <summary>
	/// Defines a validation service specialized in entity validation.
	/// </summary>
	/// <typeparam name="T">The type of the entity to validate.</typeparam>
	public class EntityValidationService<T> : AbstractValidationService
	{
		readonly T entity;
		readonly IEnumerable<IValidator<T>> validators;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityValidationService&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="validators">The validators.</param>
		public EntityValidationService( T entity, params IValidator<T>[] validators )
		{
			this.entity = entity;
			this.validators = validators;
		}

		/// <summary>
		/// Called in order to execute the concrete validation process.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <returns>
		/// A list of <seealso cref="ValidationError"/>.
		/// </returns>
		protected override IEnumerable<ValidationError> OnValidate( String ruleSet )
		{
			var validator = this.validators.SingleOrDefault( v => v.RuleSet == ruleSet );
			if( validator != null )
			{
				var result = validator.Validate( this.entity );
				return result.Errors;
			}

			return new ValidationError[ 0 ];
		}
	}
}