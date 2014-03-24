using System;
using System.Reflection;
using System.Windows;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Conversions;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Topics.Radical.Windows.Presentation.Services
{
    /// <summary>
    /// Handles Presentation conventions.
    /// </summary>
    class ConventionsHanlder : IConventionsHandler
    {
        readonly IMessageBroker broker;
        readonly IServiceProvider container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionsHanlder"/> class.
        /// </summary>
        /// <param name="broker">The broker.</param>
        public ConventionsHanlder( IMessageBroker broker, IServiceProvider container )
        {
            this.broker = broker;
            this.container = container;

            this.ResolveViewModelType = viewType =>
            {
                var aName = new AssemblyName( viewType.GetTypeInfo().Assembly.FullName );
                var vmTypeName = String.Format( "{0}.{1}Model, {2}", viewType.Namespace, viewType.Name, aName.FullName );
                var vmType = Type.GetType( vmTypeName, false );

                return vmType;
            };

            this.ResolveViewType = viewModelType =>
            {
                var aName = new AssemblyName( viewModelType.GetTypeInfo().Assembly.FullName );
                var vTypeName = String.Format( "{0}.{1}, {2}", viewModelType.Namespace, viewModelType.Name.Remove( viewModelType.Name.LastIndexOf( 'M' ) ), aName.FullName );
                var vType = Type.GetType( vTypeName, true );

                return vType;
            };

            //this.FindHostingWindowOf = vm =>
            //{
            //	var view = this.GetViewOfViewModel( vm );
            //	return view.FindWindow();
            //};

            this.ViewHasDataContext = view =>
            {
                return this.GetViewDataContext( view ) != null;
            };

            this.GetViewDataContext = view =>
            {
                if ( view is FrameworkElement )
                {
                    return ( ( FrameworkElement )view ).DataContext;
                }

                return null;
            };

            this.SetViewDataContext = ( view, dc ) =>
            {
                if ( view is FrameworkElement )
                {
                    ( ( FrameworkElement )view ).DataContext = dc;
                }
            };

            //			this.TryHookClosedEventOfHostOf = ( view, closedCallback ) =>
            //			{
            //				//dobbiamo anche cercare una IClosableView oltre che una Window
            //				var window = view.FindWindow();

            //				if( window != null )
            //				{
            //#if SILVERLIGHT
            //					EventHandler<System.ComponentModel.ClosingEventArgs> closing = null;
            //					closing = ( s, e ) =>
            //					{
            //						try
            //						{
            //							closedCallback( window );
            //						}
            //						finally
            //						{
            //							window.Closing -= closing;
            //						}
            //					};

            //					window.Closing += closing;
            //#else
            //					EventHandler closed = null;
            //					closed = ( s, e ) =>
            //					{
            //						try
            //						{
            //							closedCallback( window );
            //						}
            //						finally
            //						{
            //							window.Closed -= closed;
            //						}
            //					};

            //					window.Closed += closed;
            //#endif
            //				}

            //				return window;
            //			};

            this.IsHostingView = fe => fe.GetType().Name.EndsWith( "View" );

            this.AttachViewToViewModel = ( view, viewModel ) =>
            {
                viewModel.As<IViewModel>( i =>
                {
                    i.View = view;
                } );
            };

            this.GetViewOfViewModel = viewModel =>
            {
                if ( viewModel is IViewModel )
                {
                    return ( ( IViewModel )viewModel ).View;
                }

                return null;
            };

            this.AttachViewBehaviors = view =>
            {
                var bhv = Extensibility.GetBehaviors( view );
                
                if ( view is Page )
                {
                    var navigation = this.container.GetService<INavigationService>();
                    bhv.Add( new Behaviors.PageKeyboardMouseNavigationBehavior( navigation ) );
                    bhv.Add( new Behaviors.ControlLayoutUpdatesBehavior() );
                }

                //bhv.Add( new DependencyObjectCloseHandlerBehavior( this.broker, this ) );
            };

            this.ShouldNotifyViewModelLoaded = ( view, dataContext ) =>
            {
                //var hasAttribute = dataContext.GetType().IsAttributeDefined<NotifyLoadedAttribute>();
                //var hasRegions = RegionService.CurrentService.HoldsRegionManager( view );

                //return hasAttribute || hasRegions;

                return false;
            };
        }

        /// <summary>
        /// Gets or sets the view model type resolver that can resolve the view model type given the view type.
        /// </summary>
        /// <value>
        /// The view model type resolver.
        /// </value>
        public Func<Type, Type> ResolveViewModelType { get; set; }

        /// <summary>
        /// Gets or sets the view type resolver that can resolve the view type given the view model type.
        /// </summary>
        /// <value>
        /// The view type resolver.
        /// </value>
        public Func<Type, Type> ResolveViewType { get; set; }

        ///// <summary>
        ///// Gets or sets the window finder.
        ///// </summary>
        ///// <value>
        ///// The window finder.
        ///// </value>
        //public Func<Object, Window> FindHostingWindowOf { get; set; }

        /// <summary>
        /// Gets or sets the logic that determines if view has data context.
        /// </summary>
        /// <value>
        /// The logic that determines if view has data context.
        /// </value>
        public Predicate<DependencyObject> ViewHasDataContext { get; set; }

        /// <summary>
        /// Gets or sets the logic that sets the view data context.
        /// </summary>
        /// <value>
        /// The logic that sets the view data context.
        /// </value>
        public Action<DependencyObject, Object> SetViewDataContext { get; set; }

        /// <summary>
        /// Gets or sets the logic that gets view data context.
        /// </summary>
        /// <value>
        /// The logic that gets view data context.
        /// </value>
        public Func<DependencyObject, Object> GetViewDataContext { get; set; }

        /// <summary>
        /// Tries to hook closed event of an the element in the visual tree that hosts this given view.
        /// If the hook succedeed the given callback will be called once the hosting element is closed.
        /// </summary>
        /// <returns>
        /// The element, that supports closed notifications, in the visual tree that hosts the given view; otherwise <c>null</c>.
        ///   </returns>
        public Func<DependencyObject, Action<DependencyObject>, DependencyObject> TryHookClosedEventOfHostOf { get; set; }


        /// <summary>
        /// Gets or sets the convention that determines if the given FrameworkElement is a hosting view.
        /// </summary>
        /// <value>
        /// The convention that determines if the given FrameworkElement is a hosting view.
        /// </value>
        public Func<FrameworkElement, bool> IsHostingView { get; set; }

        /// <summary>
        /// Gets or sets the attach view to view model handler.
        /// </summary>
        /// <value>
        /// The attach view to view model handler.
        /// </value>
        public Action<DependencyObject, object> AttachViewToViewModel { get; set; }

        /// <summary>
        /// Gets the view of the given view model.
        /// </summary>
        /// <value>
        /// The get view of view model handler.
        /// </value>
        public Func<object, DependencyObject> GetViewOfViewModel { get; set; }

        /// <summary>
        /// Gets an opportunity toattach behaviors to the view.
        /// </summary>
        /// <value>
        /// The attach view behaviors handler.
        /// </value>
        public Action<FrameworkElement> AttachViewBehaviors { get; set; }

        /// <summary>
        /// Gets or sets the logic that determines if ViewModel should notify the loaded message.
        /// </summary>
        /// <value>
        /// The logic that determines if ViewModel should notify the loaded message.
        /// </value>
        public Func<DependencyObject, Object, Boolean> ShouldNotifyViewModelLoaded { get; set; }
    }
}
