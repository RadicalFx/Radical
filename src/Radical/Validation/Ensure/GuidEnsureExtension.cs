namespace Radical.Validation
{
    using System;

    /// <summary>
    /// Extends the generic Ensure class to add behaviors when the inspected object
    /// is a <c>Guid</c>.
    /// </summary>
    public static class GuidEnsureExtension
    {
        /// <summary>
        /// Extends the Ensure class when the inspected object is a <c>Guid</c> and can
        /// be used to ensure that the inspected GUID is not an empty GUID.
        /// </summary>
        /// <param name="validator">The Ensure class to extend.</param>
        /// <returns>
        /// The Ensure instance for fluent interface usage.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
        /// is raised if the current inspected object is an empty GUID.</exception>
        public static IEnsure<Guid> IsNotEmpty(this IEnsure<Guid> validator)
        {
            validator.If(g => g == Guid.Empty)
                .ThenThrow(v =>
               {
                   return new ArgumentOutOfRangeException(v.Name, v.GetFullErrorMessage("The inspected GUID value should be not empty."));
               });

            return validator;
        }
    }
}
