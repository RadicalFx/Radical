using System;
using System.Windows;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Handles Presentation conventions.
	/// </summary>
	public interface IConventionsHandler
	{
		/// <summary>
		/// Gets or sets the view model type resolver that can resolve the view model type given the view type.
		/// </summary>
		/// <value>
		/// The view model type resolver.
		/// </value>
		Func<Type, Type> ResolveViewModelType { get; set; }

		/// <summary>
		/// Gets or sets the view type resolver that can resolve the view type given the view model type.
		/// </summary>
		/// <value>
		/// The view type resolver.
		/// </value>
		Func<Type, Type> ResolveViewType { get; set; }

		///// <summary>
		///// Gets or sets the window finder.
		///// </summary>
		///// <value>
		///// The window finder.
		///// </value>
		//Func<Object, Window> FindHostingWindowOf { get; set; }

		/// <summary>
		/// Gets or sets the logic that determines if view has data context.
		/// </summary>
		/// <value>
		/// The logic that determines if view has data context.
		/// </value>
		Predicate<DependencyObject> ViewHasDataContext { get; set; }

		/// <summary>
		/// Gets or sets the logic that determines if ViewModel should notify the loaded message.
		/// </summary>
		/// <value>
		/// The logic that determines if ViewModel should notify the loaded message.
		/// </value>
		Func<DependencyObject, Object, Boolean> ShouldNotifyViewModelLoaded { get; set; }

		/// <summary>
		/// Gets or sets the logic that sets the view data context.
		/// </summary>
		/// <value>
		/// The logic that sets the view data context.
		/// </value>
		Action<DependencyObject, Object> SetViewDataContext { get; set; }

		/// <summary>
		/// Gets or sets the logic that gets view data context.
		/// </summary>
		/// <value>
		/// The logic that gets view data context.
		/// </value>
		Func<DependencyObject, Object> GetViewDataContext { get; set; }

		/// <summary>
		/// Gets or sets the attach view to view model handler.
		/// </summary>
		/// <value>
		/// The attach view to view model handler.
		/// </value>
		Action<DependencyObject, Object> AttachViewToViewModel { get; set; }

		/// <summary>
		/// Gets the view of the given view model.
		/// </summary>
		/// <value>
		/// The get view of view model handler.
		/// </value>
		Func<Object, DependencyObject> GetViewOfViewModel { get; set; }

		/// <summary>
		/// Tries to hook closed event of an the element in the visual tree that hosts this given view.
		/// If the hook succedeed the given callback will be called once the hosting element is closed.
		/// </summary>
		/// <returns>The element, that supports closed notifications, in the visual tree that hosts the given view; otherwise <c>null</c>.</returns>
		Func<DependencyObject, Action<DependencyObject>, DependencyObject> TryHookClosedEventOfHostOf { get; set; }

		/// <summary>
		/// Gets or sets the convention that determines if the given FrameworkElement is a hosting view.
		/// </summary>
		/// <value>
		/// The convention that determines if the given FrameworkElement is a hosting view.
		/// </value>
		Func<FrameworkElement, Boolean> IsHostingView { get; set; }

		/// <summary>
		/// Gets an opportunity toattach behaviors to the view.
		/// </summary>
		/// <value>
		/// The attach view behaviors handler.
		/// </value>
		Action<FrameworkElement> AttachViewBehaviors { get; set; }
	}
}
