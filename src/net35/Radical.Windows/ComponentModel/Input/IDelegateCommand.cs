using System;
using System.Windows.Input;
using Topics.Radical.Analytics;
using Topics.Radical.Helpers;

namespace Topics.Radical.ComponentModel.Windows.Input
{
	/// <summary>
	/// Defines an extended ICommand with support for bindings and triggers.
	/// </summary>
	public interface IDelegateCommand : ICommand
	{
		/// <summary>
		/// Gets the display text.
		/// </summary>
		/// <value>The display text.</value>
		String DisplayText { get; }

#if !SILVERLIGHT

		/// <summary>
		/// Gets the input bindings.
		/// </summary>
		/// <value>The input bindings.</value>
		InputBindingCollection InputBindings { get; }

		/// <summary>
		/// Adds the given gesture to the gestures list.
		/// </summary>
		/// <param name="gesture">The gesture.</param>
		/// <returns>An instance of the current command.</returns>
		IDelegateCommand AddGesture( InputGesture gesture );

		/// <summary>
		/// Adds the given key gesture to the gestures list.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>An instance of the current command.</returns>
		IDelegateCommand AddKeyGesture( System.Windows.Input.Key key );

		/// <summary>
		/// Adds the given key gesture to the gestures list.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="modifiers">The key gesture modifiers.</param>
		/// <returns>An instance of the current command.</returns>
		IDelegateCommand AddKeyGesture( System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifiers );

#endif

        /// <summary>
        /// Intercepts the analytics tracking information before sending to the analytics service.
        /// </summary>
        /// <param name="onBeforeTracking">The before tracking handler.</param>
        /// <returns>An instance of the current command.</returns>
        IDelegateCommand WithTrackingPreview( Action<AnalyticsEvent> onBeforeTracking );

		/// <summary>
		/// Sets the given Action as the delegate that must handle
		/// the commands execution logic.
		/// </summary>
		/// <param name="executeMethod">The delegate to execute at execution time.</param>
		/// <returns>An instance of the current command.</returns>
		IDelegateCommand OnExecute( Action<Object> executeMethod );

		/// <summary>
		/// Sets the given Action as the delegate that must handle
		/// the logic that determines whether the command can be executed or not.
		/// </summary>
		/// <param name="canExecuteMethod">The delegate to invoke.</param>
		/// <returns>An instance of the current command.</returns>
		IDelegateCommand OnCanExecute( Func<Object, Boolean> canExecuteMethod );

		/// <summary>
		/// Force the command to evaluates execution status.
		/// </summary>
		void EvaluateCanExecute();

		/// <summary>
		/// Adds a trigger monitor to the list of triggers.
		/// </summary>
		/// <param name="source">The source monitor.</param>
		/// <returns>An instance of the current command.</returns>
		IDelegateCommand AddMonitor( IMonitor source );

		/// <summary>
		/// Adds all the given triggers to the list of triggers.
		/// </summary>
		/// <param name="triggers">The triggers.</param>
		/// <returns>An instance of the current command.</returns>
		IDelegateCommand AddMonitor( params IMonitor[] triggers );

		/// <summary>
		/// Removes the given monitor from the list of triggers.
		/// </summary>
		/// <param name="source">The monitor to remove.</param>
		/// <returns>An instance of the current command.</returns>
		IDelegateCommand RemoveMonitor( IMonitor source );
	}
}
