//using System.Windows.Controls;
//using Topics.Radical.Windows.Behaviors;
//using Topics.Radical.ComponentModel.Messaging;
//using System.Windows.Navigation;
//using Topics.Radical.Windows.Presentation.ComponentModel;
//using System.Windows;

//namespace Topics.Radical.Windows.Presentation.Behaviors
//{
//    /// <summary>
//    /// Wires the NavigationService events to a view model that requires navigation notifications.
//    /// </summary>
//    public class PageNavigationNotifcationsBehavior : RadicalBehavior<Page>
//    {
//        readonly IMessageBroker broker;
//        readonly NavigatedEventHandler navigated;
//        readonly NavigatingCancelEventHandler navigating;
//        readonly RoutedEventHandler loaded;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="PageNavigationNotifcationsBehavior"/> class.
//        /// </summary>
//        /// <param name="broker">The broker.</param>
//        public PageNavigationNotifcationsBehavior( IMessageBroker broker )
//        {
//            this.broker = broker;

//            if( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                this.loaded = ( s, e ) => 
//                {
//                    this.AssociatedObject.NavigationService.Navigating += this.navigating;
//                    this.AssociatedObject.NavigationService.Navigated += this.navigated;
//                };

//                this.navigated += ( s, e ) => 
//                {
//                    //if( this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyLoadedAttribute>() ) 
//                    //            {
//                    //                this.broker.Broadcast( new ViewModelLoaded( this, ( IViewModel )this.AssociatedObject.DataContext ) );
//                    //            }

//                    var dc = this.AssociatedObject.DataContext as IExpectNavigationCallback;
//                    if( dc != null )
//                    {
//                        dc.OnNavigatedTo( e, this.AssociatedObject.NavigationContext );
//                    }
//                };

//                this.navigating += ( s, e ) =>
//                {
//                    var dc = this.AssociatedObject.DataContext as IExpectNavigationCallback;
//                    if( dc != null )
//                    {
//                        dc.OnNavigatingAway( e, this.AssociatedObject.NavigationContext );
//                    }
//                };
//            }
//        }

//        /// <summary>
//        /// Called after the behavior is attached to an AssociatedObject.
//        /// </summary>
//        protected override void OnAttached()
//        {
//            base.OnAttached();

//            if( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                this.AssociatedObject.Loaded += this.loaded;
//            }
//        }

//        /// <summary>
//        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
//        /// </summary>
//        protected override void OnDetaching()
//        {
//            if( !DesignTimeHelper.GetIsInDesignMode() )
//            {
//                this.AssociatedObject.NavigationService.Navigating -= this.navigating;
//                this.AssociatedObject.NavigationService.Navigated -= this.navigated;
//                this.AssociatedObject.Loaded -= this.loaded;
//            }

//            base.OnDetaching();
//        }
//    }
//}
