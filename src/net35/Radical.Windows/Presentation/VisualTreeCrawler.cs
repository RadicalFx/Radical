using System;
using Topics.Radical.Validation;

#if !NETFX_CORE
using System.Windows.Media;
using System.Windows;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
#endif
namespace Topics.Radical.Windows
{
	/// <summary>
	/// A helper calss for visual tree crawling.
	/// </summary>
	public static class VisualTreeCrawler
	{
		/// <summary>
		/// Determines whether the given dependency object is child 
		/// of an object of the specified type T.
		/// </summary>
		/// <typeparam name="T">The type of the parent object.</typeparam>
		/// <param name="obj">The obj to determine the parent type for.</param>
		/// <returns>
		/// 	<c>true</c> if the given dependency object is child of an object of the specified type T; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsChildOfType<T>( DependencyObject obj ) where T : DependencyObject
		{
			if( obj == null )
			{
				return false;
			}

			if( obj is T )
			{
				return true;
			}

			var parent = VisualTreeHelper.GetParent( obj );
			return VisualTreeCrawler.IsChildOfType<T>( parent );
		}

		/// <summary>
		/// Finds, in the Visual Tree, the parent, of the given type T,
		/// of the given dependency object.
		/// </summary>
		/// <typeparam name="T">The type of the searched parent.</typeparam>
		/// <param name="obj">The object where to start search.</param>
		/// <returns>The found parent dependency object or null if none is of the given type T.</returns>
		public static T FindParent<T>( this DependencyObject obj ) where T : DependencyObject
		{
			return obj.FindParent<T>( t => true );
		}

		/// <summary>
		/// Finds, in the Visual Tree, the parent, of the given type T,
		/// of the given dependency object that matches the given condition.
		/// </summary>
		/// <typeparam name="T">The type of the searched parent.</typeparam>
		/// <param name="obj">The object where to start search.</param>
		/// <param name="matchCondition">The match condition.</param>
		/// <returns>
		/// The found parent dependency object or null if none is of the given type T.
		/// </returns>
		public static T FindParent<T>( this DependencyObject obj, Predicate<T> matchCondition ) where T : DependencyObject
		{
			var parent = VisualTreeHelper.GetParent( obj );

			if( parent == null )
			{
				return null;
			}
			else if( parent is T && matchCondition( ( T )parent ) )
			{
				return ( T )parent;
			}
			else
			{
				return VisualTreeCrawler.FindParent<T>( parent, matchCondition );
			}
		}

		/// <summary>
		/// Finds the child fo the givent type.
		/// </summary>
		/// <typeparam name="T">The typeof the child to find.</typeparam>
		/// <param name="source">The source to look on.</param>
		/// <returns>
		/// The found child if any; otherwise null.
		/// </returns>
		public static T FindChild<T>( this DependencyObject source )
			where T : DependencyObject
		{
			return FindChild<T>( source, o => true );
		}

		/// <summary>
		/// Finds the child fo the givent type.
		/// </summary>
		/// <typeparam name="T">The typeof the child to find.</typeparam>
		/// <param name="source">The source to look on.</param>
		/// <param name="filter">The filter to apply to determine if the found child satisfy the requirments of the caller.</param>
		/// <returns>The found child if any; otherwise null.</returns>
		public static T FindChild<T>( this DependencyObject source, Predicate<T> filter )
			where T : DependencyObject
		{
			Ensure.That( source ).Named( "referenceVisual" ).IsNotNull();
			Ensure.That( filter ).Named( "filter" ).IsNotNull();

			DependencyObject child = null;

			var count = VisualTreeHelper.GetChildrenCount( source );
			for( Int32 i = 0; i < count; i++ )
			{
				child = VisualTreeHelper.GetChild( source, i );
				if( child != null && ( child.GetType() == typeof( T ) ) && filter( ( T )child ) )
				{
					break;
				}
				else if( child != null )
				{
					child = VisualTreeCrawler.FindChild( child, filter );
					if( child != null && ( child.GetType() == typeof( T ) ) && filter( ( T )child ) )
					{
						break;
					}
				}
			}

			return child as T;
		}
	}
}
