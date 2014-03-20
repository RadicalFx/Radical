using System.Linq;
using System.Windows.Media;
using System.Diagnostics;
using System.Collections.Generic;

namespace System.Windows
{
	public static class LogicalTreeHelper
	{
		/// <summary>
		/// Retrieves all the visual children of a framework element.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <returns>The visual children of the framework element.</returns>
		static IEnumerable<DependencyObject> GetVisualChildren( this DependencyObject parent )
		{
			Debug.Assert( parent != null, "The parent cannot be null." );

			int childCount = VisualTreeHelper.GetChildrenCount( parent );
			for( int counter = 0; counter < childCount; counter++ )
			{
				yield return VisualTreeHelper.GetChild( parent, counter );
			}
		}

		/// <summary>
		/// Retrieves all the logical children of a framework element using a 
		/// depth-first search.  A visual element is assumed to be a logical 
		/// child of another visual element if they are in the same namescope.
		/// For performance reasons this method manually manages the stack 
		/// instead of using recursion.
		/// </summary>
		/// <param name="parent">The parent framework element.</param>
		/// <returns>The logical children of the framework element.</returns>
		public static IEnumerable<FrameworkElement> GetChildren( this FrameworkElement parent )
		{
			Debug.Assert( parent != null, "The parent cannot be null." );

			//EnsureName( parent );
			if( String.IsNullOrWhiteSpace( parent.Name ) ) 
			{
				parent.Name = Guid.NewGuid().ToString();
			}

			string parentName = parent.Name;
			var stack = new Stack<FrameworkElement>( parent.GetVisualChildren().OfType<FrameworkElement>() );

			while( stack.Count > 0 )
			{
				FrameworkElement element = stack.Pop();
				if( element.FindName( parentName ) == parent )
				{
					yield return element;
				}
				else
				{
					foreach( FrameworkElement visualChild in element.GetVisualChildren().OfType<FrameworkElement>() )
					{
						stack.Push( visualChild );
					}
				}
			}
		}
	}
}
