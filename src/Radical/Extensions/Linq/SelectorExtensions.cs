using Radical.Validation;
using System;
using System.Collections.Generic;

namespace Radical.Linq
{
    /// <summary>
    /// Add behaviors to the IEnumerable<paramref name="T"/> interface.
    /// </summary>
    public static class SelectorExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence, or a default value if the
        /// sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="T:System.Collections.Generic.IEnumerable`1"/> to return the first element of.</param>
        /// <param name="defaultValue">A function to return the default value.</param>
        /// <returns>
        /// The value provided by the given Func<paramref name="T"/> if <paramref name="source"/> is empty; otherwise, the first element in <paramref name="source"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="source"/> or <paramref name="defaultValue"/> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static T FirstOr<T>(this IEnumerable<T> source, Func<T> defaultValue)
        {
            Ensure.That(source).Named(() => source).IsNotNull();
            Ensure.That(defaultValue).Named(() => defaultValue).IsNotNull();

            var list = source as IList<T>;
            if (list == null)
            {
                using (var enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current;
                    }
                }
            }
            else
            {
                if (list.Count > 0)
                {
                    return list[0];
                }
            }

            return defaultValue();
        }

        /// <summary>
        /// Returns the only element of a sequence or a default value if no elements exists; this method throws an exception if the list contains more than one element.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to return a single element from.</param>
        /// <param name="defaultValue">A function to return the default value.</param>
        /// <returns>
        /// The single element of the input sequence, or Func<paramref name="T"/> if no such element is found.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="source"/> or <paramref name="defaultValue"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">More than one element satisfies the condition in <paramref name="predicate"/>.</exception>
        public static T SingleOr<T>(this IEnumerable<T> source, Func<T> defaultValue)
        {
            Ensure.That(source).Named(() => source).IsNotNull();
            Ensure.That(defaultValue).Named(() => defaultValue).IsNotNull();

            var list = source as IList<T>;
            if (list == null)
            {
                using (var enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                    {
                        return defaultValue();
                    }

                    var current = enumerator.Current;
                    if (!enumerator.MoveNext())
                    {
                        return current;
                    }
                }
            }
            else
            {
                switch (list.Count)
                {
                    case 0:
                        return defaultValue();
                    case 1:
                        return list[0];
                }
            }

            throw new InvalidOperationException("The source list contains more than one element.");
        }
    }
}
