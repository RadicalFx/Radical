using Topics.Radical.Windows.Behaviors;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Topics.Radical.Windows.Presentation.Behaviors
{
    public class ControlLayoutUpdatesBehavior : RadicalBehavior<Control>
    {
        RoutedEventHandler onLoaded;
        RoutedEventHandler onUnloaded;
        WindowSizeChangedEventHandler onSizeChanged;

        public ControlLayoutUpdatesBehavior()
        {
            this.onSizeChanged = ( s, e ) => this.SynchronizeVisualState();
            this.onLoaded = ( s, e ) => 
            {
                Window.Current.SizeChanged += this.onSizeChanged;
                this.SynchronizeVisualState();
            };

            this.onUnloaded = ( s, e ) =>
            {
                Window.Current.SizeChanged -= this.onSizeChanged;
            };
        }

        private string DetermineVisualState( ApplicationViewState applicationViewState )
        {
            return applicationViewState.ToString();
        }

        void SynchronizeVisualState() 
        {
            string visualState = this.DetermineVisualState( ApplicationView.Value );
            VisualStateManager.GoToState( this.AssociatedObject, visualState, false );
        }

        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += this.onLoaded;
            this.AssociatedObject.Unloaded += this.onUnloaded;
        }

        protected override void OnDetached()
        {
            this.AssociatedObject.Loaded -= this.onLoaded;
            this.AssociatedObject.Unloaded -= this.onUnloaded;
        }
    }
}
