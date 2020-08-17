using Radical.Validation;

namespace Radical.ComponentModel.Validation
{
    /// <summary>
    /// Identifies an object that wants to plug custom
    /// validation logic into the validation process.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    [System.Obsolete("IRequireValidationCallback has been obsoleted and will be removed in v3.0.0")]
    public interface IRequireValidationCallback<T>
    {
        /// <summary>
        /// Called by a validation service to let the 
        /// implementors inject custom validation logic 
        /// into the validation process.
        /// </summary>
        /// <param name="context">The validation context.</param>
        void OnValidate(ValidationContext<T> context);
    }
}
