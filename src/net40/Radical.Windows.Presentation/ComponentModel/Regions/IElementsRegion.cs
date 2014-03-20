using System.Collections.Generic;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// A regione that holds a list of elements.
	/// </summary>
	public interface IElementsRegion : IRegion
	{
		/// <summary>
		/// Adds the specified view.
		/// </summary>
		/// <param name="view">The view.</param>
		void Add( DependencyObject view );

		/// <summary>
		/// Removes the specified view.
		/// </summary>
		/// <param name="view">The view.</param>
		void Remove( DependencyObject view );

		/// <summary>
		/// Gets the elements, in this region, of the given type T.
		/// </summary>
		/// <typeparam name="TView">The type of the elements to find.</typeparam>
		/// <returns>A list of elements of the given type T.</returns>
		IEnumerable<TView> GetElements<TView>() where TView : DependencyObject;
	}
}
