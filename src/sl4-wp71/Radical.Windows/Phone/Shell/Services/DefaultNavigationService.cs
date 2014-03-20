using System;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Validation;
using Topics.Radical.Phone.Shell.Services;
using Topics.Radical.Windows.Phone.Shell.Presentation;

namespace Topics.Radical.Windows.Phone.Shell.Services
{
    /// <summary>
    /// A default navigation service.
    /// </summary>
    public class DefaultNavigationService : INavigationService
    {
        /// <summary>
        /// Gets the message broker.
        /// </summary>
        protected IMessageBroker Broker { get; private set; }

        readonly PhoneApplicationFrame frame;
        Boolean isResumePending;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNavigationService"/> class.
        /// </summary>
        /// <param name="broker">The broker.</param>
        /// <param name="frame">The frame.</param>
        public DefaultNavigationService( IMessageBroker broker, PhoneApplicationFrame frame )
        {
            Ensure.That( broker ).Named( () => broker ).IsNotNull();
            Ensure.That( frame ).Named( () => frame ).IsNotNull();

            this.Broker = broker;
            this.frame = frame;

            this.Broker.Subscribe<LifecycleMessage>( this, ( s, msg ) =>
            {
                if ( msg.Event == LifecycleEvent.Resuming )
                {
                    this.isResumePending = true;
                }
            } );

            this.frame.Navigated += ( s, e ) =>
            {
                this.OnNavigated( e, this.currentNavigationMode );

                if ( this.isResumePending )
                {
                    this.isResumePending = false;

                    var message = new LifecycleMessage( this, LifecycleEvent.Resumed );
                    this.Broker.Broadcast( this, message );
                    //this.OnResumed( EventArgs.Empty );
                }
            };

            this.frame.Navigating += ( s, e ) =>
            {
                this.OnNavigating( e );
            };

            this.frame.Obscured += ( s, e ) =>
            {
                var message = new LifecycleMessage( this, LifecycleEvent.Obscured );
                this.Broker.Broadcast( this, message );
            };

            this.frame.Unobscured += ( s, e ) =>
            {
                var message = new LifecycleMessage( this, LifecycleEvent.Unobscured );
                this.Broker.Broadcast( this, message );
            };
        }

        NavigationMode currentNavigationMode = NavigationMode.New;
        PhoneApplicationPage _currentPage;

        /// <summary>
        /// Gets the current page if any.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public PhoneApplicationPage CurrentPage
        {
            get { return this._currentPage; }
            private set
            {
                if ( this._currentPage != value )
                {
                    var current = this._currentPage;
                    this._currentPage = value;
                    this.OnCurrentPageChanged( current, value );
                }
            }
        }

        /// <summary>
        /// Determines whether the specified URI is tombstoning.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        ///   <c>true</c> if the specified URI is tombstoning; otherwise, <c>false</c>.
        /// </returns>
        protected Boolean IsTombstoning( Uri uri )
        {
            bool isTombstoning = uri.OriginalString.IsLike( "app://external*" );
            return isTombstoning;
        }

        /// <summary>
        /// Raises the <see cref="E:Navigated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Navigation.NavigationEventArgs"/> instance containing the event data.</param>
        /// <param name="navigationMode">The navigation mode.</param>
        protected virtual void OnNavigated( NavigationEventArgs e, NavigationMode navigationMode )
        {
            this.CurrentPage = e.Content as PhoneApplicationPage;

            if ( this.CurrentPage != null )
            {
                this.CurrentPage.OrientationChanged += this.OnPageOrientationChanged;

                var irnc = this.CurrentPage as IRequireNavigationCallback;
                if ( irnc != null )
                {
                    irnc.OnNavigated( e );
                }

                var irncdc = this.CurrentPage.DataContext as IRequireNavigationCallback;
                if ( irncdc != null )
                {
                    irncdc.OnNavigated( e );
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Navigating"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Navigation.NavigatingCancelEventArgs"/> instance containing the event data.</param>
        protected virtual void OnNavigating( NavigatingCancelEventArgs e )
        {
            if ( this.CurrentPage != null )
            {
                this.CurrentPage.OrientationChanged -= this.OnPageOrientationChanged;

                var irnc = this.CurrentPage as IRequireNavigationCallback;
                if ( irnc != null )
                {
                    irnc.OnNavigating( e );
                }

                var irncdc = this.CurrentPage.DataContext as IRequireNavigationCallback;
                if ( irncdc != null )
                {
                    irncdc.OnNavigating( e );
                }

                if ( !e.Cancel )
                {
                    this.CurrentPage = null;
                }
            }

            this.currentNavigationMode = e.NavigationMode;
        }

        /// <summary>
        /// Called when the current page is changed.
        /// </summary>
        /// <param name="oldPage">The old page.</param>
        /// <param name="newPage">The new page.</param>
        protected virtual void OnCurrentPageChanged( PhoneApplicationPage oldPage, PhoneApplicationPage newPage )
        {

        }

        void OnPageOrientationChanged( Object sender, OrientationChangedEventArgs e )
        {
            this.OnOrientationChanged( e.Orientation );
        }

        /// <summary>
        /// Called when the orientation changes.
        /// </summary>
        /// <param name="orientation">The new orientation.</param>
        protected virtual void OnOrientationChanged( PageOrientation orientation )
        {

        }

        /// <summary>
        /// Gets a value indicating whether this instance can navigate back.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can navigate back; otherwise, <c>false</c>.
        /// </value>
        public Boolean CanNavigateBack
        {
            get { return this.frame.CanGoBack; }
        }

        /// <summary>
        /// Navigates back.
        /// </summary>
        public void NavigateBack()
        {
            if ( this.CanNavigateBack )
            {
                this.frame.GoBack();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can navigate forward.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can navigate forward; otherwise, <c>false</c>.
        /// </value>
        public Boolean CanNavigateForward
        {
            get { return this.frame.CanGoForward; }
        }

        /// <summary>
        /// Navigates forward.
        /// </summary>
        public void NavigateForward()
        {
            if ( this.CanNavigateForward )
            {
                this.frame.GoForward();
            }
        }

        /// <summary>
        /// Navigates to the specified destination URI.
        /// </summary>
        /// <param name="destination">The destination URI.</param>
        /// <returns>
        ///   <c>True</c> if the navigation succeeds; otherwise <c>false</c>.
        /// </returns>
        public Boolean Navigate( Uri destination )
        {
            Ensure.That( destination )
                .Named( () => destination )
                .IsNotNull();

            return this.frame.Navigate( destination );
        }

        /// <summary>
        /// Called by an application in order to initialize the application extension service.
        /// </summary>
        /// <param name="context">Provides information about the application state.</param>
        public void StartService( System.Windows.ApplicationServiceContext context )
        {

        }

        /// <summary>
        /// Called by an application in order to stop the application extension service.
        /// </summary>
        public void StopService()
        {

        }
    }
}
