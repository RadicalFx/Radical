namespace Radical.Validation
{
    /// <summary>
    /// Represents the base class for a validation result.
    /// </summary>
    public abstract class ValidationResult
    {
    }

    /// <summary>
    /// Represents a successful validation result.
    /// </summary>
    public sealed class SuccessfulValidationResult : ValidationResult { }

    /// <summary>
    /// Represents a failed validation result containing the associated error message.
    /// </summary>
    public sealed class FailedValidationResult : ValidationResult
    {
        internal FailedValidationResult(string error)
        {
            Error = error;
        }

        /// <summary>
        /// Gets the error message describing why validation failed.
        /// </summary>
        public string Error { get; }
    }
}
