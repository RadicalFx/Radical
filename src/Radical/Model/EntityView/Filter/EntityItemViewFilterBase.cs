using Radical.ComponentModel;
using Radical.Reflection;
using Radical.Validation;

namespace Radical.Model
{
    /// <summary>
    /// Provides a standard method to determine if an object instance 
    /// should be, or should not, included in the result set of a filter
    /// operation.
    /// </summary>
    /// <typeparam name="T">The type of the object to test.</typeparam>
    public abstract class EntityItemViewFilterBase<T> : IEntityItemViewFilter<T> //where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityItemViewFilterBase&lt;T&gt;"/> class.
        /// </summary>
        protected EntityItemViewFilterBase()
        {

        }

        /// <summary>
        /// Gets a item that indicates if the given object instance should be included in the result set of the filter operation..
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>
        ///     <collection>True</collection> if the item should be included, otherwise <collection>false</collection>.
        /// </returns>
        public abstract bool ShouldInclude(T item);

        /// <summary>
        /// Gets a item that indicates if the given object instance should be included in the result set of the filter operation..
        /// </summary>
        /// <param name="item">The item to test.</param>
        /// <returns>
        ///     <collection>True</collection> if the item should be included, otherwise <collection>false</collection>.
        /// </returns>
        bool IEntityItemViewFilter.ShouldInclude(object item)
        {
            Ensure.That(item).Named(() => item)
                .IsNotNull()
                .IsTrue(o => o.GetType().Is<T>());

            return this.ShouldInclude((T)item);
        }
        
        /// <summary>
        /// Return a string that represents the current object.
        /// </summary>
        /// <returns>A string representing the current object.</returns>
        public override string ToString()
        {
            return Resources.Labels.DefaultFilterName;
        }
    }
}
