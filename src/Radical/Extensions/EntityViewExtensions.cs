namespace Radical
{
    using Radical.ComponentModel;
    using Radical.Validation;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public static class EntityViewExtensions
    {
        public static IEnumerable<T> AsEntityItems<T>(this IEntityView<T> view)
            where T : class
        {
            return view.Select(item => item.EntityItem); ;
        }

        public static IEntityView<T> ApplySimpleSort<T>(this IEntityView<T> view, string property)
            where T : class
        {
            Ensure.That(view).Named("view").IsNotNull();

            var actualDirection = view.SortDirection;
            var actualProperty = view.SortProperty == null ? (string)null : view.SortProperty.Name;

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
                 * Arriviamo qui se è un "nuovo" sort o se
                 * il sort è su null
                 */
                view.ApplySort(property);
            }

            return view;
        }
    }
}
