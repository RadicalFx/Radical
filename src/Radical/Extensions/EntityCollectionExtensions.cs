using Radical.ComponentModel;
using System;
using System.Collections.Generic;

namespace Radical
{
    /// <summary>
    /// Provides extension methods for <see cref="Radical.ComponentModel.IEntityCollection{T}"/> to support bulk loading of data.
    /// </summary>
    public static class EntityCollectionExtensions
    {
        /// <summary>
        /// Clears the collection and bulk-loads all items from the specified data source.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="list">The target entity collection.</param>
        /// <param name="data">The data to load into the collection.</param>
        /// <returns>A reference to the target collection to allow fluent usage.</returns>
        public static IEntityCollection<T> BulkLoad<T>(this IEntityCollection<T> list, IEnumerable<T> data)
            where T : class
        {
            return BulkLoad(list, data, true);
        }

        /// <summary>
        /// Bulk-loads all items from the specified data source into the collection,
        /// optionally clearing existing items first.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="list">The target entity collection.</param>
        /// <param name="data">The data to load into the collection.</param>
        /// <param name="clear"><c>true</c> to clear the collection before loading; otherwise, <c>false</c>.</param>
        /// <returns>A reference to the target collection to allow fluent usage.</returns>
        public static IEntityCollection<T> BulkLoad<T>(this IEntityCollection<T> list, IEnumerable<T> data, bool clear)
            where T : class
        {
            list.BeginInit();

            if (clear)
            {
                list.Clear();
            }

            list.AddRange(data);
            list.EndInit(true);

            return list;
        }

        /// <summary>
        /// Bulk-loads items from the specified data source into the collection, converting each source item
        /// using the provided adapter delegate.
        /// </summary>
        /// <typeparam name="T">The type of items in the target collection.</typeparam>
        /// <typeparam name="TSource">The type of items in the source data.</typeparam>
        /// <param name="list">The target entity collection.</param>
        /// <param name="data">The source data to load.</param>
        /// <param name="adapter">A delegate that converts a source item to the target type.</param>
        /// <returns>A reference to the target collection to allow fluent usage.</returns>
        public static IEntityCollection<T> BulkLoad<T, TSource>(this IEntityCollection<T> list, IEnumerable<TSource> data, Func<TSource, T> adapter)
            where T : class
        {
            list.BeginInit();
            foreach (var item in data)
            {
                list.Add(adapter(item));
            }
            list.EndInit(true);

            return list;
        }
    }
}
