//using System.Windows;
//using Windows.UI.Xaml;

//namespace Topics.Radical.Windows.Presentation
//{
//	/// <summary>
//	/// Adds behaviors to the <see cref="DependencyObject"/> class.
//	/// </summary>
//	public static class DependencyObjectExtensions
//	{
//		/// <summary>
//		/// Finds the window that hosts the given control.
//		/// </summary>
//		/// <param name="view">The view.</param>
//		/// <returns>The hosting window.</returns>
//		public static Window FindWindow( this DependencyObject view )
//		{
//			if( view == null )
//			{
//				return null;
//			}

//			var w = view as Window;
//			if( w != null )
//			{
//				return w;
//			}

//			var parent = VisualTreeHelper.GetParent( view );
//			return FindWindow( parent );
//		}
//	}
//}
