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
    public interface IValidator
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
        Boolean IsValid( Object entity );

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An instance of the <see cref="ValidationResults"/> with the results of the validation process.</returns>
        ValidationResults Validate( Object entity );

        /// <summary>
        /// Validates the specified property of the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults"/> with the results of the validation process.
        /// </returns>
        ValidationResults Validate( Object entity, String propertyName );
    }
}
