using System.Collections.Generic;

namespace Radical.Validation
{
    /// <summary>
    /// Helper class to expose empty validation errors.
    /// </summary>
    public static class ValidationErrors
    {
        /// <summary>
        /// An empty list of validation errors.
        /// </summary>
        public static readonly IEnumerable<ValidationError> Empty = new ValidationError[ 0 ];
    }
}
