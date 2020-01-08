namespace Radical.Validation
{
    public abstract class ValidationResult
    {
    }

    public sealed class SuccessfulValidationResult : ValidationResult { }

    public sealed class FailedValidationResult : ValidationResult 
    {
        internal FailedValidationResult(string error) 
        {
            Error = error;
        }

        public string Error { get; }
    }
}
