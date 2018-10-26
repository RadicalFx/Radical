using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radical.Validation;
using Radical;
using System.Collections;
using Radical.ComponentModel;
using Radical.Collections;

namespace Radical.Linq
{
    /// <summary>
    /// Adds behaviors to the generic <c>IEnumerable</c> interface.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Convert the given source tree in a flat list.
        /// </summary>
        /// <typeparam name="T">The type of the source items.</typeparam>
        /// <param name="root">The root.</param>
        /// <param name="childrenGetter">The delegate to retrieve the children of a given node.</param>
        /// <returns>
        /// A flat readonly list of all the items in the source tree.
        /// </returns>
        public static IEnumerable<T> ToFlat<T>( this T root, Func<T, IEnumerable<T>> childrenGetter ) 
            where T : class
        {
            Ensure.That( root ).Named( "root" ).IsNotNull();
            Ensure.That( childrenGetter ).Named( "childrenGetter" ).IsNotNull();

            return ( new[] { root } ).ToFlat( childrenGetter );
        }

        /// <summary>
        /// Convert the given source tree in a flat list.
        /// </summary>
        /// <typeparam name="T">The type of the source items.</typeparam>
        /// <param name="source">The source tree.</param>
        /// <param name="childrenGetter">The delegate to retrieve the children of a given node.</param>
        /// <returns>A flat readonly list of all the items in the source tree.</returns>
        public static IEnumerable<T> ToFlat<T>( this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenGetter )
        {
            Ensure.That( source ).Named( "source" ).IsNotNull();
            Ensure.That( childrenGetter ).Named( "childrenGetter" ).IsNotNull();

            foreach( T item in source )
            {
                yield return item;

                var children = childrenGetter( item );
                if( children != null )
                {
                    foreach( T child in children.ToFlat( childrenGetter ) )
                    {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// Finds all the nodes in the given source tree that satisfies the given condition.
        /// </summary>
        /// <typeparam name="T">The type of the item in the source tree.</typeparam>
        /// <param name="source">The source tree..</param>
        /// <param name="childrenGetter">The delegate to retrieve the children of a given node.</param>
        /// <param name="condition">The condition to usa as filter.</param>
        /// <returns>A flat readonly list of all the items in the source tree that mets the given condition.</returns>
        public static IEnumerable<T> FindNodes<T>( this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenGetter, Func<T, Boolean> condition )
        {
            Ensure.That( source ).Named( "source" ).IsNotNull();
            Ensure.That( childrenGetter ).Named( "childrenGetter" ).IsNotNull();
            Ensure.That( condition ).Named( "condition" ).IsNotNull();

            var flat = source.ToFlat( childrenGetter );
            foreach( var item in flat )
            {
                if( condition( item ) )
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Determines whether the current tree item is child of any of the given items list.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="parents">The flat parent list.</param>
        /// <param name="parentGetter">A predicate to retrieve the parent of the current item.</param>
        /// <returns>
        ///     <c>true</c> if tha item is child of any of the items in the given items list; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsChildOfAny<T>( this T item, IEnumerable<T> parents, Func<T, T> parentGetter )
            where T : class
        {
            Ensure.That( parents ).Named( "parents" ).IsNotNull();
            Ensure.That( parentGetter ).Named( "parentGetter" ).IsNotNull();

            return item.IsChildOfAny( parents, parentGetter, new ReferenceEqualityComparer<T>() );
        }

        /// <summary>
        /// Determines whether the current tree item is child of any of the given items list.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="parents">The flat parent list.</param>
        /// <param name="parentGetter">A delegate to retrieve the parent of the current item.</param>
        /// <param name="comparer">A delegate used to compare items for equality.</param>
        /// <returns>
        ///     <c>true</c> if tha item is child of any of the items in the given items list; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsChildOfAny<T>( this T item, IEnumerable<T> parents, Func<T, T> parentGetter, Func<T, T, Boolean> comparer )
        {
            Ensure.That( parents ).Named( "parents" ).IsNotNull();
            Ensure.That( parentGetter ).Named( "parentGetter" ).IsNotNull();
            Ensure.That( comparer ).Named( "comparer" ).IsNotNull();

            return item.IsChildOfAny( parents, parentGetter, new DelegateEqualityComparer<T>( comparer, t => t.GetHashCode() ) );
        }

        /// <summary>
        /// Determines whether the current tree item is child of any of the given items list.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="parents">The flat parent list.</param>
        /// <param name="parentGetter">A delegate to retrieve the parent of the current item.</param>
        /// <param name="comparer">An equality comparer used to compare items for equality.</param>
        /// <returns>
        ///     <c>true</c> if tha item is child of any of the items in the given items list; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsChildOfAny<T>( this T item, IEnumerable<T> parents, Func<T, T> parentGetter, IEqualityComparer<T> comparer )
        {
            Ensure.That( parents ).Named( "parents" ).IsNotNull();
            Ensure.That( parentGetter ).Named( "parentGetter" ).IsNotNull();
            Ensure.That( comparer ).Named( "comparer" ).IsNotNull();

            if( item == null )
            {
                return false;
            }

            if( parents.Contains( item, comparer ) )
            {
                return true;
            }

            var parent = parentGetter( item );
            return parent.IsChildOfAny( parents, parentGetter, comparer );
        }

        /// <summary>
        /// Applies the specified Action to every item in the enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the enumeration.</typeparam>
        /// <param name="list">The enumeration to enumerate on.</param>
        /// <param name="action">The action to apply/execute.</param>
        /// <returns>The source enumeration.</returns>
        public static IEnumerable<T> ForEach<T>( this IEnumerable<T> list, Action<T> action )
        {
            Ensure.That( list ).Named( "list" ).IsNotNull();
            Ensure.That( action ).Named( "action" ).IsNotNull();

            foreach( T element in list )
            {
                action( element );
            }

            return list;
        }

        /// <summary>
        /// Executes the specified Func for each item in the enumeration 
        /// passing throught the given state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="T">The type of the elements of the enumeration.</typeparam>
        /// <param name="list">The enumeration to enumerate on.</param>
        /// <param name="initialState">The initial state.</param>
        /// <param name="func">The Func to invoke for each element.</param>
        /// <returns>The source enumeration.</returns>
        public static IEnumerable<T> ForEach<TState, T>( this IEnumerable<T> list, TState initialState, Func<TState, T, TState> func )
        {
            Ensure.That( list ).Named( "list" ).IsNotNull();
            Ensure.That( func ).Named( "func" ).IsNotNull();

            TState actualState = initialState;
            foreach( var element in list )
            {
                actualState = func( actualState, element );
            }

            return list;
        }

        /// <summary>
        /// Applies the specified Action to every item in the enumeration.
        /// </summary>
        /// <param name="list">The enumeration to enumerate.</param>
        /// <param name="action">The action to apply/execute.</param>
        /// <returns>The source list for fluent interface usage.</returns>
        public static IEnumerable Enumerate( this IEnumerable list, Action<Object> action )
        {
            return list.OfType<Object>().ForEach( action );
        }

        /// <summary>
        /// Creates a read only copy of the given source list.
        /// </summary>
        /// <typeparam name="T">The type of item of the list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <returns>A new read only <c>IEnumerable</c> containing the same items of the source list.</returns>
        public static IEnumerable<T> AsReadOnly<T>( this IEnumerable<T> list )
        {
            Ensure.That( list ).Named( "list" ).IsNotNull();

            var tmp = list as List<T>;
            if( tmp != null )
            {
                return tmp.AsReadOnly();
            }

            return new ReadOnlyCollection<T>(list);
        }

        /// <summary>
        /// Determines if the sequence contains elements.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <returns><c>True</c> if the sequence contains elements; otherwise <c>false</c>.</returns>
        public static Boolean Any( this IEnumerable source )
        {
            Ensure.That( source ).Named( "source" ).IsNotNull();

            var e = source.GetEnumerator();
            return e.MoveNext();
        }

        /// <summary>
        /// Determines if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the sequence source item.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        ///     <c>True</c> if the sequence contains no elements; otherwise <c>false</c>.
        /// </returns>
        public static Boolean None<T>( this IEnumerable<T> source )
        {
            return !source.Any();
        }

        /// <summary>
        /// Determines if the sequence contains no elements.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <returns><c>True</c> if the sequence contains no elements; otherwise <c>false</c>.</returns>
        public static Boolean None( this IEnumerable source )
        {
            return !source.Any();
        }

        /// <summary>
        /// Alternates the source list with the suplied separator instance.
        /// </summary>
        /// <typeparam name="T">The type of the list item.</typeparam>
        /// <param name="items">The source items list.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The new list with inserted the separator.</returns>
        public static IEnumerable<T> AlternateWith<T>( this IEnumerable<T> items, T separator )
        {
            Ensure.That( items ).Named( "items" ).IsNotNull();

            var isFirst = true;
            foreach( var item in items )
            {
                if( isFirst )
                {
                    isFirst = false;
                }
                else
                {
                    yield return separator;
                }

                yield return item;
            }
        }

        /// <summary>
        /// Shouffles the items in the source list in a random order.
        /// </summary>
        /// <typeparam name="T">The type of the item in the source list.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>A new list ordered in a random manner.</returns>
        public static IEnumerable<T> Shouffle<T>( this IEnumerable<T> items ) 
        {
            return items.OrderBy( i => Guid.NewGuid() );
        }
    }
}