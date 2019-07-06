namespace Radical.Conversions
{
    /// <summary>
    /// Add behaviors to convert from one type to another.
    /// </summary>
    public static class ConvertExtensions
    {
        /// <summary>
        /// Returns the source value as a generic observable object.
        /// </summary>
        /// <typeparam name="T">The type of the source value.</typeparam>
        /// <param name="value">The source value.</param>
        /// <returns>The observable instance that wraps the source value.</returns>
        public static Observable<T> AsObservable<T>(this T value)
        {
            return new Observable<T>(value);
        }

    }
}
