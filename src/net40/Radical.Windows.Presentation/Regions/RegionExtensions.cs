using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Regions
{
	/// <summary>
	/// Region extensions.
	/// </summary>
	public static class RegionExtensions
	{
		/// <summary>
		/// Loads the given content async in the supplied region.
		/// </summary>
		/// <param name="region">The region</param>
		/// <param name="viewFactory">the view factory</param>
		/// <param name="millisecondsDelay">The async load delay.</param>
		public static void SetContentAsync( this IContentRegion region, Func<DependencyObject> viewFactory, Int32 millisecondsDelay = 2000 )
		{
			Wait.For( TimeSpan.FromMilliseconds( millisecondsDelay ) )
				.AndThen( () =>
				{
					var view = viewFactory();
					region.Content = view;
				} );
		}

		/// <summary>
		/// Add the given content async to the supplied region.
		/// </summary>
		/// <param name="region">the region.</param>
		/// <param name="viewFactory">the view factory</param>
		/// <param name="millisecondsDelay">The async load delay.</param>
		public static void AddContentAsync( this IElementsRegion region, Func<DependencyObject> viewFactory, Int32 millisecondsDelay = 2000 )
		{
			Wait.For( TimeSpan.FromMilliseconds( millisecondsDelay ) )
				.AndThen( () =>
				{
					var view = viewFactory();
					region.Add( view );
				} );
		}
	}
}
