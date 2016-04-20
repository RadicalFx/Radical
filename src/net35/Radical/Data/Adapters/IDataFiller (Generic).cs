namespace Topics.Radical.Data.Adapters
{
    using System;

    public interface IDataFiller<TSource, TDestination>
    {
        /// <summary>
        /// Invalidates this instance.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Adapts the specified source filling the given destination instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="instance">The destination instance to fill.</param>
        /// <returns>The adapted destination instance.</returns>
        TDestination Fill( TSource source, TDestination destination );
    }
}
