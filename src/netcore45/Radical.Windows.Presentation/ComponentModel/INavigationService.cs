using System;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	public interface INavigationService
	{
		bool CanGoBack { get; }
		void GoBack();

        bool CanGoForward { get; }
        void GoForward();

		void Navigate<TView>() where TView : global::Windows.UI.Xaml.DependencyObject;
		void Navigate<TView>( Object navigationArguments ) where TView : global::Windows.UI.Xaml.DependencyObject;

		void Navigate( Type viewType );
		void Navigate( Type viewType, Object navigationArguments );

		void Navigate( String uri );
		void Navigate( String uri, Object navigationArguments );

		void Suspend( ISuspensionManager manager );
		void Resume( ISuspensionManager manager );
	}
}
