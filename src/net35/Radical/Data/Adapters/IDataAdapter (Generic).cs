namespace Topics.Radical.Data.Adapters
{
	using System;

	public interface IDataAdapter<TSource, TDestination>
	{
		/// <summary>
		/// Invalidates this instance.
		/// </summary>
		void Invalidate();

		/// <summary>
		/// Adapts the specified source to a known destination.
		/// </summary>
		/// <param name="source">The source to pick data from.</param>
		/// <returns>An instance of the specified known class containing source data.</returns>
		TDestination Adapt( TSource source );
	}
}
