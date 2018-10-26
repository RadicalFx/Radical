using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Radical.Linq;
using Radical.Validation;

namespace Radical.ComponentModel.Validation
{
    /// <summary>
    /// Defines the base contract for a validator strongly typed for
    /// a specific entity.
    /// </summary>
    /// <typeparam name="T">The type of the validated entity.</typeparam>
    [Contract]
    public interface IValidator<T> : IValidator
    {
        /// <summary>
        /// Gets the rule set.
        /// </summary>
        String RuleSet { get; }

        /// <summary>
        /// Determines whether the specified entity is valid.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is valid; otherwise, <c>false</c>.
        /// </returns>
        Boolean IsValid( T entity );

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An instance of the <see cref="ValidationResults"/> with the results of the validation process.</returns>
        ValidationResults Validate( T entity );

        /// <summary>
        /// Validates the specified property of the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults"/> with the results of the validation process.
        /// </returns>
        ValidationResults Validate( T entity, String propertyName );

        /// <summary>
        /// Validates the specified property of the given entity.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="property">The property to validate.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults"/> with the results of the validation process.
        /// </returns>
        ValidationResults Validate<TProperty>( T entity, Expression<Func<T, TProperty>> property );

        /// <summary>
        /// Adds the given rule to the list of the validation rules.
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        /// <returns>The current validator instance.</returns>
        IValidator<T> AddRule( Action<ValidationContext<T>> rule );

        /// <summary>
        /// Adds the given rule to the list of the validation rules.
        /// </summary>
        /// <param name="propertyIdentifier">The property identifier.</param>
        /// <param name="rule">The rule to add.</param>
        /// <param name="error">The error if the given rule is not valid.</param>
        /// <returns>The current validator instance.</returns>
        IValidator<T> AddRule( Expression<Func<T, object>> propertyIdentifier, Func<ValidationContext<T>, RuleEvaluation> rule, String error );

        /// <summary>
        /// Adds the given rule to the list of the validation rules.
        /// </summary>
        /// <param name="propertyIdentifier">The property identifier.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        IValidator<T> AddRule( Expression<Func<T, object>> propertyIdentifier, Func<ValidationContext<T>, RuleEvaluation> rule, Func<ValidationContext<T>, string> error );
    }

    /// <summary>
    /// Identifies the result of a rule evaluation.
    /// </summary>
    public enum RuleEvaluation
    {
        /// <summary>
        /// The rule evaluates successfully.
        /// </summary>
        Succeeded,

        /// <summary>
        /// The rule evaluation failed, the result is invalid.
        /// </summary>
        Failed
    }
}
