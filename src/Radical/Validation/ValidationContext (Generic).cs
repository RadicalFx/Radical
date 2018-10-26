using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Radical.Linq;
using Radical.Validation;
using Radical.ComponentModel.Validation;

namespace Radical.Validation
{
    /// <summary>
    /// Defines the current validation context.
    /// </summary>
    /// <typeparam name="T">The type of the validated object.</typeparam>
    public class ValidationContext<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationContext&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="validator">The validator.</param>
        public ValidationContext( T entity, IValidator<T> validator )
            : this( entity, validator, new ValidationResults() )
        {
 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationContext&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="validator">The validator.</param>
        /// <param name="results">The results.</param>
        public ValidationContext( T entity, IValidator<T> validator, ValidationResults results )
        {
            Ensure.That( validator ).Named( "validator" ).IsNotNull();
            Ensure.That( results ).Named( "results" ).IsNotNull();

            this.Entity = entity;
            this.Validator = validator;
            this.Results = results;
        }

        /// <summary>
        /// Gets the entity under validation.
        /// </summary>
        /// <value>The entity.</value>
        public T Entity { get; private set; }

        /// <summary>
        /// Gets or sets the validation rule set if specified.
        /// </summary>
        /// <value>The rule set.</value>
        public String RuleSet { get; set; }

        /// <summary>
        /// Gets or sets the name of the validated property if specified.
        /// </summary>
        /// <value>The name of the property.</value>
        public String PropertyName { get; set; }

        /// <summary>
        /// Gets the current validator.
        /// </summary>
        /// <value>The validator.</value>
        public IValidator<T> Validator { get; private set; }

        /// <summary>
        /// Gets the current validation results.
        /// </summary>
        /// <value>The validation results.</value>
        public ValidationResults Results { get; private set; }
    }
}
