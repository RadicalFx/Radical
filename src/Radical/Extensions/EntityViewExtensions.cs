using Radical.ComponentModel;
using Radical.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Radical
{
    /// <summary>
    /// Provides extension methods for <see cref="Radical.ComponentModel.IEntityView{T}"/>.
    /// </summary>
    public static class EntityViewExtensions
    {
        /// <summary>
        /// Projects each item in the view to its underlying entity item, returning a flat enumerable of entities.
        /// </summary>
        /// <typeparam name="T">The type of the underlying entity.</typeparam>
        /// <param name="view">The entity view to project.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of the underlying entity items.</returns>
        public static IEnumerable<T> AsEntityItems<T>(this IEntityView<T> view)
            where T : class
        {
            return view.Select(item => item.EntityItem);
        }

        /// <summary>
        /// Applies a simple single-column sort to the view on the specified property name,
        /// toggling the sort direction when the same property is sorted again.
        /// </summary>
        /// <typeparam name="T">The type of the underlying entity.</typeparam>
        /// <param name="view">The entity view to sort.</param>
        /// <param name="property">The name of the property to sort by.</param>
        /// <returns>A reference to the view to allow fluent usage.</returns>
        public static IEntityView<T> ApplySimpleSort<T>(this IEntityView<T> view, string property)
            where T : class
        {
            Ensure.That(view).Named("view").IsNotNull();

            var actualDirection = view.SortDirection;
            var actualProperty = view.SortProperty?.Name;

            if (property != null && property == actualProperty)
            {
                /*
                 * Dobbiamo invertire il sort attuale.
                 */
                if (actualDirection == ListSortDirection.Ascending)
                {
                    actualDirection = ListSortDirection.Descending;
                }
                else
                {
                    actualDirection = ListSortDirection.Ascending;
                }

                var lsd = new ListSortDescription(view.GetProperty(property), actualDirection);
                view.ApplySort(new ListSortDescriptionCollection(new[] { lsd }));
            }
            else
            {
                /*
                 * Arriviamo qui se � un "nuovo" sort o se
                 * il sort � su null
                 */
                view.ApplySort(property);
            }

            return view;
        }
    }
}
