using Radical.Validation;
using System;

namespace Radical.Model
{
    public sealed class PredicateEntityItemViewFilter<T> : EntityItemViewFilterBase<T> // where T : class
    {
        public Predicate<T> FilterDelegate { get; private set; }
        private string filterName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateEntityItemViewFilter&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="filterDelegate">The filter delegate.</param>
        public PredicateEntityItemViewFilter( Predicate<T> filterDelegate )
            : this( filterDelegate, null )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateEntityItemViewFilter&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="filterDelegate">The filter delegate.</param>
        /// <param name="filterName">The name of the filter.</param>
        public PredicateEntityItemViewFilter( Predicate<T> filterDelegate, string filterName )
        {
            Ensure.That( filterDelegate ).Named( "filterDelegate" ).IsNotNull();

            this.filterName = filterName ?? Resources.Labels.DefaulPredicateFilterName;
            this.FilterDelegate = filterDelegate;
        }

        /// <summary>
        /// Returns a <see cref="T:System.string"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.string"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return this.filterName;
        }

        #region EntityItemViewFilterBase<T> Members

        /// <summary>
        /// Gets a item that indicates if the given object instance should be included in the result set of the filter operation..
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>
        ///     <collection>True</collection> if the item should be included, otherwise <collection>false</collection>.
        /// </returns>
        public override bool ShouldInclude( T item )
        {
            return this.FilterDelegate( item );
        }

        #endregion
    }
}