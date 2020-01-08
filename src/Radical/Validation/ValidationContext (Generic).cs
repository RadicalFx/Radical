using Radical.Linq;
using Radical.Reflection;
using System.ComponentModel;

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
        public ValidationContext(T entity, Validator<T> validator)
            : this(entity, validator, new ValidationResults())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationContext&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="validator">The validator.</param>
        /// <param name="results">The results.</param>
        public ValidationContext(T entity, Validator<T> validator, ValidationResults results)
        {
            Ensure.That(validator).Named("validator").IsNotNull();
            Ensure.That(results).Named("results").IsNotNull();

            this.Entity = entity;
            this.Validator = validator;
            this.Results = results;
        }

        public ValidationResult Failed(string error)
        {
            return new FailedValidationResult(error);
        }

        public ValidationResult Succeeded()
        {
            return new SuccessfulValidationResult();
        }

        /// <summary>
        /// Gets the entity under validation.
        /// </summary>
        /// <value>The entity.</value>
        public T Entity { get; private set; }

        /// <summary>
        /// Gets or sets the name of the validated property if specified.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets the current validator.
        /// </summary>
        /// <value>The validator.</value>
        public Validator<T> Validator { get; private set; }

        /// <summary>
        /// Gets the current validation results.
        /// </summary>
        /// <value>The validation results.</value>
        public ValidationResults Results { get; private set; }

        internal void Evaluate(ValidationRule<T> rule)
        {
            var result = rule.Rule(this);
            if (result is FailedValidationResult failedValidationResult)
            {
                var propertyName = rule.Property.GetMemberName();
                string displayName = null;
                var pi = Entity.GetType().GetProperty(propertyName);
                if (pi != null && pi.IsAttributeDefined<DisplayNameAttribute>())
                {
                    displayName = pi.GetAttribute<DisplayNameAttribute>().DisplayName;
                }
                Results.AddError(new ValidationError(propertyName, displayName, new[] { failedValidationResult.Error }));
            }
        }
    }
}
