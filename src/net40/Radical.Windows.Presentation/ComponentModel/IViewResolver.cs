using System;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Resolves views providing support for a View first approach.
	/// </summary>
	public interface IViewResolver
	{
		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <typeparam name="T">The type of the view.</typeparam>
		/// <returns>The view instance.</returns>
		T GetView<T>() where T : DependencyObject;

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="viewModelInterceptor">The view model interceptor.</param>
		/// <returns></returns>
		T GetView<T>( Action<Object> viewModelInterceptor ) where T : DependencyObject;

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <typeparam name="TView">The type of the view.</typeparam>
		/// <typeparam name="TViewModel">The type of the view model.</typeparam>
		/// <param name="viewModelInterceptor">The view model interceptor.</param>
		/// <returns></returns>
		TView GetView<TView, TViewModel>( Action<TViewModel> viewModelInterceptor )
			where TView : DependencyObject;

		/// <summary>
		/// Gets the view of the given type.
		/// </summary>
		/// <returns>The view instance.</returns>
		DependencyObject GetView( Type viewType );

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <param name="viewType">Type of the view.</param>
		/// <param name="viewModelInterceptor">The view model interceptor.</param>
		/// <returns></returns>
		DependencyObject GetView( Type viewType, Action<Object> viewModelInterceptor );

		///// <summary>
		///// Releases the given view
		///// </summary>
		///// <param name="view">The view to release.</param>
		//void Release( DependencyObject view );
	}
}
