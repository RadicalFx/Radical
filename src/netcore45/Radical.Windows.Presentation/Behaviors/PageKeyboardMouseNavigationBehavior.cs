using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Topics.Radical.Windows.Presentation.Behaviors
{
    public class PageKeyboardMouseNavigationBehavior : RadicalBehavior<Page>
    {
        readonly INavigationService navigation;
        RoutedEventHandler loaded = null;
        RoutedEventHandler unloaded = null;

        public PageKeyboardMouseNavigationBehavior( INavigationService navigation )
        {
            this.navigation = navigation;

            this.loaded = ( s, e ) =>
            {
                // Keyboard and mouse navigation only apply when occupying the entire window
                if ( this.AssociatedObject.ActualHeight == Window.Current.Bounds.Height
                    && this.AssociatedObject.ActualWidth == Window.Current.Bounds.Width )
                {
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated +=
                        OnAcceleratorKeyActivated;
                    Window.Current.CoreWindow.PointerPressed +=
                        this.OnPointerPressed;
                }
            };

            this.unloaded = ( s, e ) =>
            {
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -=
                    OnAcceleratorKeyActivated;
                Window.Current.CoreWindow.PointerPressed -=
                    this.OnPointerPressed;
            };
        }

        private void OnAcceleratorKeyActivated( CoreDispatcher sender, AcceleratorKeyEventArgs args )
        {
            var virtualKey = args.VirtualKey;

            // Only investigate further when Left, Right, or the dedicated Previous or Next keys
            // are pressed
            if ( ( args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
                args.EventType == CoreAcceleratorKeyEventType.KeyDown ) &&
                ( virtualKey == VirtualKey.Left || virtualKey == VirtualKey.Right ||
                ( int )virtualKey == 166 || ( int )virtualKey == 167 ) )
            {
                var coreWindow = Window.Current.CoreWindow;
                var downState = CoreVirtualKeyStates.Down;
                bool menuKey = ( coreWindow.GetKeyState( VirtualKey.Menu ) & downState ) == downState;
                bool controlKey = ( coreWindow.GetKeyState( VirtualKey.Control ) & downState ) == downState;
                bool shiftKey = ( coreWindow.GetKeyState( VirtualKey.Shift ) & downState ) == downState;
                bool noModifiers = !menuKey && !controlKey && !shiftKey;
                bool onlyAlt = menuKey && !controlKey && !shiftKey;

                if ( this.navigation.CanGoBack
                    &&
                    (
                        ( ( int )virtualKey == 166 && noModifiers )
                        || ( virtualKey == VirtualKey.Left && onlyAlt )
                    )
                   )
                {
                    // When the previous key or Alt+Left are pressed navigate back
                    args.Handled = true;
                    this.navigation.GoBack();
                }
                else if ( this.navigation.CanGoForward
                        &&
                        (
                            ( ( int )virtualKey == 167 && noModifiers )
                            || ( virtualKey == VirtualKey.Right && onlyAlt )
                        )
                    )
                {
                    // When the next key or Alt+Right are pressed navigate forward
                    args.Handled = true;
                    this.navigation.GoForward();
                }
            }
        }

        private void OnPointerPressed( CoreWindow sender,
            PointerEventArgs args )
        {
            var properties = args.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if ( properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed ) return;

            // If back or foward are pressed (but not both) navigate appropriately
            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;
            if ( backPressed ^ forwardPressed )
            {
                args.Handled = true;
                if ( backPressed && this.navigation.CanGoBack ) this.navigation.GoBack();
                if ( forwardPressed && this.navigation.CanGoForward ) this.navigation.GoForward();
            }
        }

        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += this.loaded;
        }

        protected override void OnDetached()
        {
            this.AssociatedObject.Loaded -= this.loaded;
        }
    }
}
