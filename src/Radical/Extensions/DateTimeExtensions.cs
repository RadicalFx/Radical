using System;

namespace Radical
{
    /// <summary>
    /// Adds behaviors to the <c>DateTime</c> class.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Returns a new DateTime representing the last day of source date month.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A new DateTime representing the last day of source date month.</returns>
        public static DateTime ToEndOfMonth(this DateTime source)
        {
            var tmp = new DateTime(source.Year, source.Month, 1, source.Hour, source.Minute, source.Second, source.Millisecond);
            var endOfMonth = tmp.AddMonths(1).AddDays(-1);

            return endOfMonth;
        }
    }
}
