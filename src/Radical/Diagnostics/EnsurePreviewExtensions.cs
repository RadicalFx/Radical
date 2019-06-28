using Radical.Validation;
using System.Diagnostics;

namespace Radical.Diagnostics
{
    /// <summary>
    /// Defines some extensions facilities for the <see cref="Ensure"/> class.
    /// </summary>
    public static class EnsurePreviewExtensions
    {
        /// <summary>
        /// Logs the errors to raised by the ensure infrastructure using the given logger.
        /// </summary>
        /// <typeparam name="T">The validated data type.</typeparam>
        /// <param name="validator">The validator.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>The current validator for fluent interface usage.</returns>
        public static IEnsure<T> LogErrorsTo<T>(this IEnsure<T> validator, TraceSource logger)
        {
            validator.WithPreview((v, e) =>
           {
               var msg = v.GetFullErrorMessage();
               logger.Error(msg, e, logger.Switch.Level == SourceLevels.Verbose);
           });

            return validator;
        }
    }
}
