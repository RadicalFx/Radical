namespace Topics.Radical.Validation
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
		/// be used to ensure that the inspected Guid is not an empty Guid.
		/// </summary>
		/// <param name="validator">The Ensure class to extend.</param>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">An <c>ArgumentOutOfRangeException</c>
		/// is raised if the current inspected object is an empty Guid.</exception>
		public static IEnsure<Guid> IsNotEmpty( this IEnsure<Guid> validator )
		{
			validator.If( g => g == Guid.Empty )
				.ThenThrow( v =>
				{
					return new ArgumentOutOfRangeException( v.Name, v.GetFullErrorMessage( "The inspected guid value should be not empty." ) );
				} );

			return validator;
		}
	}
}
