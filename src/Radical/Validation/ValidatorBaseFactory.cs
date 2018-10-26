using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radical.ComponentModel.Validation;

namespace Radical.Validation
{
    /// <summary>
    /// A factory capable of creationg base validators.
    /// </summary>
    public sealed class ValidatorBaseFactory : IValidatorFactory
    {
        /// <summary>
        /// Creates a validator for the given entity type.
        /// </summary>
        /// <typeparam name="T">The entity type to validate.</typeparam>
        /// <returns>An instance of the requested validator.</returns>
        public IValidator<T> CreateValidator<T>()
        {
            return this.CreateValidator<T>( null );
        }

        /// <summary>
        /// Creates a validator, that uses the given rule set,
        /// for the given entity.
        /// </summary>
        /// <typeparam name="T">The entity type to validate.</typeparam>
        /// <param name="ruleSet">The rule set to pass to the
        /// newly created validator.</param>
        /// <returns>An instance of the requested validator.</returns>
        public IValidator<T> CreateValidator<T>( string ruleSet )
        {
            return new ValidatorBase<T>( ruleSet );
        }
    }
}
