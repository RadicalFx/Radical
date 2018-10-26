using System;

namespace Radical.ComponentModel.Validation
{
    /// <summary>
    /// Identifies a factory whose capability is to 
    /// create validators.
    /// </summary>
    public interface IValidatorFactory
    {
        /// <summary>
        /// Creates a validator for the given entity type.
        /// </summary>
        /// <typeparam name="T">The entity type to validate.</typeparam>
        /// <returns>An instance of the requested validator.</returns>
        IValidator<T> CreateValidator<T>();

        /// <summary>
        /// Creates a validator, that uses the given rule set, 
        /// for the given entity.
        /// </summary>
        /// <typeparam name="T">The entity type to validate.</typeparam>
        /// <param name="ruleSet">The rule set to pass to the 
        /// newly created validator.</param>
        /// <returns>An instance of the requested validator.</returns>
        IValidator<T> CreateValidator<T>( String ruleSet );
    }
}
