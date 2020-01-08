namespace Radical.Model
{
    using Radical.ComponentModel;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class EntityItemViewSortComparer<T> : IComparer<IEntityItemView<T>>
    //where T : class
    {
        /// <summary>
        /// Gets the sort descriptions.
        /// </summary>
        /// <item>The sort descriptions.</item>
        protected ListSortDescriptionCollection SortDescriptions
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewSortComparer&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="sortDescriptions">The sort descriptions.</param>
        public EntityItemViewSortComparer(ListSortDescriptionCollection sortDescriptions)
        {
            SortDescriptions = sortDescriptions;
        }

        static int Compare(object a, object b, ListSortDirection direction)
        {
            int retVal = 0;

            if (!Equals(a, b))
            {
                IComparable ic = a as IComparable;
                if (ic != null)
                {
                    retVal = ic.CompareTo(b);
                }
                else
                {
                    /*
                     * Se non sono IComparable, proviamo
                     * a confrontare la loro rappresentazione
                     * sottoforma di stringa
                     */
                    var av = a == null ? (string)null : a.ToString();
                    var bv = b == null ? (string)null : b.ToString();

                    retVal = string.Compare(av, bv, StringComparison.CurrentCulture);
                }
            }

            if (direction == ListSortDirection.Descending)
            {
                retVal *= -1;
            }

            return retVal;
        }

        /// <summary>
        /// Compares two objects and returns a item indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value Condition Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        protected virtual int OnCompare(IEntityItemView<T> x, IEntityItemView<T> y)
        {
            /*
             * Il confronto viene fatto scorrendo tutti
             * i ListSortDescription...
             * 
             * Diventa necessario confrontare i criteri successivi
             * solo se il precedente non ha dato risultati utili, cioè
             * i due valori confrontati sono uguali...
             * 
             * Il classico esempio è Cognome ASC, Nome DESC confronto i 
             * nomi solo se i Cognomi sono uguali
             */
            int retVal = 0;

            if (SortDescriptions.Count == 1)
            {
                ListSortDescription sd = SortDescriptions[0];

                object valueX = sd.PropertyDescriptor.GetValue(x);
                object valueY = sd.PropertyDescriptor.GetValue(y);

                retVal = Compare(valueX, valueY, sd.SortDirection);
            }
            else
            {
                foreach (ListSortDescription sd in SortDescriptions)
                {
                    if (retVal == 0)
                    {
                        object valueX = sd.PropertyDescriptor.GetValue(x);
                        object valueY = sd.PropertyDescriptor.GetValue(y);

                        retVal = Compare(valueX, valueY, sd.SortDirection);
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Compares two objects and returns a item indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value Condition Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public int Compare(IEntityItemView<T> x, IEntityItemView<T> y)
        {
            return OnCompare(x, y);
        }
    }
}
