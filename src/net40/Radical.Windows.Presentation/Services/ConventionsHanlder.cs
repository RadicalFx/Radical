using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Conversions;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Presentation.Behaviors;
using Topics.Radical.Windows.Presentation.Boot;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Regions;

namespace Topics.Radical.Windows.Presentation.Services
{
	/// <summary>
	/// Handles Presentation conventions.
	/// </summary>
	class ConventionsHanlder : IConventionsHandler
	{
		readonly IMessageBroker broker;
		readonly IReleaseComponents releaser;
		readonly BootstrapConventions bootstrapConventions;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConventionsHanlder"/> class.
		/// </summary>
		/// <param name="broker">The broker.</param>
		/// <param name="releaser">The releaser.</param>
		/// <param name="bootstrapConventions">The Bootstrap Conventions</param>
		public ConventionsHanlder( IMessageBroker broker, IReleaseComponents releaser, BootstrapConventions bootstrapConventions )
		{
			this.broker = broker;
			this.releaser = releaser;
			this.bootstrapConventions = bootstrapConventions;

			this.ResolveViewModelType = viewType =>
			{
				var aName = new AssemblyName( viewType.Assembly.FullName );
				var vmTypeName = String.Format( "{0}.{1}Model, {2}", viewType.Namespace, viewType.Name, aName.FullName );
				var vmType = Type.GetType( vmTypeName, false );

				return vmType;
			};

			this.ResolveViewType = viewModelType =>
			{
				var aName = new AssemblyName( viewModelType.Assembly.FullName );
				var vTypeName = String.Format( "{0}.{1}, {2}", viewModelType.Namespace, viewModelType.Name.Remove( viewModelType.Name.LastIndexOf( 'M' ) ), aName.FullName );
				var vType = Type.GetType( vTypeName, true );

				return vType;
			};

			this.ViewReleaseHandler = v =>
			{
				var vm = this.GetViewDataContext( v, ViewDataContextSearchBehavior.LocalOnly );
				if ( vm != null )
				{
					this.releaser.Release( vm );
					if ( this.ShouldUnsubscribeViewModelOnRelease( v ) )
					{
						this.broker.Unsubscribe( vm );
					}
				}

				this.DetachViewBehaviors( v );

				this.releaser.Release( v );
			};

			Func<DependencyObject, Boolean> isSingletonView = view =>
			{
				var implementation = view.GetType();
				var contracts = this.bootstrapConventions.SelectViewContracts( implementation );
				var isShell = this.bootstrapConventions.IsShellView( contracts, implementation );

				return isShell;
			};

			this.ShouldUnsubscribeViewModelOnRelease = view => !isSingletonView( view );

			this.ShouldReleaseView = view => !isSingletonView( view );

			this.ShouldUnregisterRegionManagerOfView = view => !isSingletonView( view );

			this.FindHostingWindowOf = vm =>
			{
				var view = this.GetViewOfViewModel( vm );
				var window = this.FindWindowOf( view );//.FindWindow();
				return window;
			};

			this.FindWindowOf = dependencyObject =>
			{
				var window = Window.GetWindow( dependencyObject );//.FindWindow();
				return window;
			};

			this.ViewHasDataContext = ( view, behavior ) =>
			{
				return this.GetViewDataContext( view, behavior ) != null;
			};

			this.GetViewDataContext = ( view, behavior ) =>
			{
				if ( behavior == ViewDataContextSearchBehavior.Legacy )
				{
					if ( view is FrameworkElement )
					{
						return ( ( FrameworkElement )view ).DataContext;
					}
#if !SILVERLIGHT
					else if ( view is FrameworkContentElement )
					{
						return ( ( FrameworkContentElement )view ).DataContext;
					}
#endif
				}
				else
				{
					if ( view is FrameworkElement )
					{
						var dc = view.ReadLocalValue( FrameworkElement.DataContextProperty );
						if ( dc != DependencyProperty.UnsetValue )
						{
							return dc;
						}
					}
#if !SILVERLIGHT
					else if ( view is FrameworkContentElement )
					{
						var dc = view.ReadLocalValue( FrameworkContentElement.DataContextProperty );
						if ( dc != DependencyProperty.UnsetValue )
						{
							return dc;
						}
					}
#endif
				}

				return null;
			};

			this.SetViewDataContext = ( view, dc ) =>
			{
				if ( view is FrameworkElement )
				{
					( ( FrameworkElement )view ).DataContext = dc;
				}
#if !SILVERLIGHT
				else if ( view is FrameworkContentElement )
				{
					( ( FrameworkContentElement )view ).DataContext = dc;
				}
#endif
			};

			this.TryHookClosedEventOfHostOf = ( view, closedCallback ) =>
			{
				//dobbiamo anche cercare una IClosableView oltre che una Window
				var window = this.FindWindowOf( view );
				if ( window != null )
				{
#if SILVERLIGHT
					EventHandler<System.ComponentModel.ClosingEventArgs> closing = null;
					closing = ( s, e ) =>
					{
						try
						{
							closedCallback( window );
						}
						finally
						{
							window.Closing -= closing;
						}
					};

					window.Closing += closing;
#else
					EventHandler closed = null;
					closed = ( s, e ) =>
					{
						try
						{
							closedCallback( window );
						}
						finally
						{
							window.Closed -= closed;
						}
					};

					window.Closed += closed;
#endif
				}

				return window;
			};

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

#if !SILVERLIGHT
			this.AttachViewBehaviors = view =>
			{
				var bhv = Interaction.GetBehaviors( view );
				if ( view is Window && bhv.OfType<WindowLifecycleNotificationsBehavior>().None() )
				{
					bhv.Add( new WindowLifecycleNotificationsBehavior( this.broker, this ) );
				}
				else if ( view is FrameworkElement && bhv.OfType<FrameworkElementLifecycleNotificationsBehavior>().None() )
				{
					bhv.Add( new FrameworkElementLifecycleNotificationsBehavior( this.broker, this ) );
				}

				if ( bhv.OfType<DependencyObjectCloseHandlerBehavior>().None() )
				{
					bhv.Add( new DependencyObjectCloseHandlerBehavior( this.broker, this ) );
				}
			};
#else
			this.AttachViewBehaviors = view =>
			{
				var bhv = Interaction.GetBehaviors( view );
				//if( view is Page )
				//{
				//    bhv.Add( new PageNavigationNotifcationsBehavior( this.broker ) );
				//}
				
				bhv.Add( new FrameworkElementLifecycleNotificationsBehavior( this.broker, this ) );
				bhv.Add( new DependencyObjectCloseHandlerBehavior( this.broker, this ) );
			};
#endif

#if !SILVERLIGHT
			this.DetachViewBehaviors = view =>
			{
				var bhv = Interaction.GetBehaviors( view );
				if ( view is Window )
				{
					bhv.OfType<WindowLifecycleNotificationsBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
				}
				else if ( view is FrameworkElement )
				{
					bhv.OfType<FrameworkElementLifecycleNotificationsBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
				}
				bhv.OfType<DependencyObjectCloseHandlerBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
			};
#else
			this.DetachViewBehaviors = view =>
			{
				var bhv = Interaction.GetBehaviors( view );
				bhv.OfType<FrameworkElementLifecycleNotificationsBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
				bhv.OfType<DependencyObjectCloseHandlerBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
			};
#endif

			this.ShouldNotifyViewModelLoaded = ( view, dataContext ) =>
			{
				if ( dataContext == null )
				{
					return false;
				}

				var hasAttribute = dataContext.GetType().IsAttributeDefined<NotifyLoadedAttribute>();
				var hasRegions = RegionService.CurrentService.HoldsRegionManager( view );

				return hasAttribute || hasRegions;
			};

			this.ShouldNotifyViewLoaded = view =>
			{
				/*
				 * we should decide if the attribute must be applied on the view or, as in this fix,
				 * mainly for backward compatibility, can be applied also on the ViewModel and the
				 * _View_Loaded message is still broadcasted.
				 */
				//var dataContext = this.GetViewDataContext( view );
				//var hasAttributeOnViewModel = dataContext != null && dataContext.GetType().IsAttributeDefined<NotifyLoadedAttribute>();
				var hasAttributeOnView = view.GetType().IsAttributeDefined<NotifyLoadedAttribute>();
				var hasRegions = RegionService.CurrentService.HoldsRegionManager( view );

				return /* hasAttributeOnViewModel || */ hasAttributeOnView || hasRegions;
			};
		}

		//Boolean TryFindWindowOrIClosableView( DependencyObject fe, out DependencyObject windowOrIClosableView )
		//{
		//    if( fe is IClosableView || fe is Window )
		//    {
		//        windowOrIClosableView = fe;
		//        return true;
		//    }
		//    else if( fe != null /* && fe.Parent != null */ )
		//    {
		//        var parent = VisualTreeHelper.GetParent( fe );
		//        return TryFindWindowOrIClosableView( parent, out windowOrIClosableView );
		//    }
		//    else
		//    {
		//        windowOrIClosableView = null;
		//        return false;
		//    }
		//}

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

		/// <summary>
		/// Gets or sets the window finder.
		/// </summary>
		/// <value>
		/// The window finder.
		/// </value>
		public Func<Object, Window> FindHostingWindowOf { get; set; }

		public Func<DependencyObject, Window> FindWindowOf { get; set; }

		/// <summary>
		/// Gets or sets the logic that determines if view has data context.
		/// </summary>
		/// <value>
		/// The logic that determines if view has data context.
		/// </value>
		public Func<DependencyObject, ViewDataContextSearchBehavior, Boolean> ViewHasDataContext { get; set; }

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
		public Func<DependencyObject, ViewDataContextSearchBehavior, Object> GetViewDataContext { get; set; }

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
		public Action<DependencyObject> AttachViewBehaviors { get; set; }

		/// <summary>
		/// Gets an opportunity to detach behaviors from the view.
		/// </summary>
		/// <value>
		/// The detach view behaviors handler.
		/// </value>
		public Action<DependencyObject> DetachViewBehaviors { get; set; }

		/// <summary>
		/// Gets or sets the logic that determines if ViewModel should notify the loaded message.
		/// </summary>
		/// <value>
		/// The logic that determines if ViewModel should notify the loaded message.
		/// </value>
		public Func<DependencyObject, Object, Boolean> ShouldNotifyViewModelLoaded { get; set; }

		/// <summary>
		/// Gets or sets the logic that determines if View should notify the loaded message.
		/// </summary>
		/// <value>
		/// The logic that determines if View should notify the loaded message.
		/// </value>
		public Func<DependencyObject, Boolean> ShouldNotifyViewLoaded { get; set; }

		/// <summary>
		/// Gets or sets the view relase handler that is responsible to release views and view models.
		/// </summary>
		/// <value>
		/// The view release handler.
		/// </value>
		public Action<DependencyObject> ViewReleaseHandler { get; set; }


        /// <summary>
        /// Gets or sets the handler that determines if a region manager for the given view should be un-registered, the default behavior is that the region manager should be realsed if the view is not a singleton view.
        /// </summary>
        /// <value>
        /// The un-register region manager handler.
        /// </value>
		public Func<DependencyObject, bool> ShouldUnregisterRegionManagerOfView { get; set; }

        /// <summary>
        /// Gets or sets the handler that determines if a view should be relased, the default behavior is that the view is released if not a singleton view.
        /// </summary>
        /// <value>
        /// The view release handler.
        /// </value>
		public Func<DependencyObject, bool> ShouldReleaseView { get; set; }

        /// <summary>
        /// Gets or sets the handler that determines if a view model should be automatically unsubscribed from all the subscriptions when its view is relased, the default behavior is that the view model is unsubscribed if the view is not a singleton view.
        /// </summary>
        /// <value>
        /// The unsubscribe handler.
        /// </value>
		public Func<DependencyObject, bool> ShouldUnsubscribeViewModelOnRelease { get; set; }


        /// <summary>
        /// Gets or sets the default view data context search behavior.
        /// </summary>
        /// <value>
        /// The default view data context search behavior.
        /// </value>
        public ViewDataContextSearchBehavior DefaultViewDataContextSearchBehavior
        {
            get;
            set;
        }
    }
}
