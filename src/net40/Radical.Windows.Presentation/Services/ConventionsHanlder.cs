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

			this.DefaultResolveViewModelType = viewType =>
			{
				var aName = new AssemblyName( viewType.Assembly.FullName );
				var vmTypeName = String.Format( "{0}.{1}Model, {2}", viewType.Namespace, viewType.Name, aName.FullName );
				var vmType = Type.GetType( vmTypeName, false );

				return vmType;
			};

			this.ResolveViewModelType = viewType =>
			{
				return this.DefaultResolveViewModelType( viewType );
			};

			this.DefaultResolveViewType = viewModelType =>
			{
				var aName = new AssemblyName( viewModelType.Assembly.FullName );
				var vTypeName = String.Format( "{0}.{1}, {2}", viewModelType.Namespace, viewModelType.Name.Remove( viewModelType.Name.LastIndexOf( 'M' ) ), aName.FullName );
				var vType = Type.GetType( vTypeName, true );

				return vType;
			};

			this.ResolveViewType = viewModelType =>
			{
				return this.DefaultResolveViewType( viewModelType );
			};

			this.DefaultViewReleaseHandler = ( view, behavior ) =>
			{
				var autoDispose = view.GetType().IsAttributeDefined<ViewManualReleaseAttribute>() == false;
				if( autoDispose || behavior == ViewReleaseBehavior.Force )
				{
					var vm = this.GetViewDataContext( view, ViewDataContextSearchBehavior.LocalOnly );
					if( vm != null )
					{
						this.SetViewDataContext( view, null );
						if( this.ShouldUnsubscribeViewModelOnRelease( view ) )
						{
							this.broker.Unsubscribe( vm );
						}

						this.releaser.Release( vm );
					}

					this.DetachViewBehaviors( view );

					this.releaser.Release( view );
				}
			};

			this.ViewReleaseHandler = ( view, behavior ) =>
			{
				this.DefaultViewReleaseHandler( view, behavior );
			};

			Func<DependencyObject, Boolean> isSingletonView = view =>
			{
				var implementation = view.GetType();
				var contracts = this.bootstrapConventions.SelectViewContracts( implementation );
				var isShell = this.bootstrapConventions.IsShellView( contracts, implementation );

				return isShell;
			};

			this.DefaultShouldUnsubscribeViewModelOnRelease = view => !isSingletonView( view );

			this.ShouldUnsubscribeViewModelOnRelease = view => this.DefaultShouldUnsubscribeViewModelOnRelease( view );

			this.DefaultShouldReleaseView = view => !isSingletonView( view );

			this.ShouldReleaseView = view => this.DefaultShouldReleaseView( view );

			this.DefaultShouldUnregisterRegionManagerOfView = view => !isSingletonView( view );

			this.ShouldUnregisterRegionManagerOfView = view => this.DefaultShouldUnregisterRegionManagerOfView( view );

			this.DefaultFindHostingWindowOf = vm =>
			{
				var view = this.GetViewOfViewModel( vm );
				var window = this.FindWindowOf( view );//.FindWindow();
				return window;
			};

			this.FindHostingWindowOf = vm =>
			{
				return this.DefaultFindHostingWindowOf( vm );
			};

			this.DefaultFindWindowOf = dependencyObject =>
			{
				var window = Window.GetWindow( dependencyObject );//.FindWindow();
				return window;
			};

			this.FindWindowOf = dependencyObject =>
			{
				return this.DefaultFindWindowOf(dependencyObject);
			};

			this.DefaultViewHasDataContext = ( view, behavior ) =>
			{
				return this.GetViewDataContext( view, behavior ) != null;
			};

			this.ViewHasDataContext = ( view, behavior ) =>
			{
				return this.DefaultViewHasDataContext( view, behavior );
			};

			this.DefaultGetViewDataContext = ( view, behavior ) =>
			{
				if( behavior == ViewDataContextSearchBehavior.Legacy )
				{
					if( view is FrameworkElement )
					{
						return ( ( FrameworkElement )view ).DataContext;
					}
#if !SILVERLIGHT
					else if( view is FrameworkContentElement )
					{
						return ( ( FrameworkContentElement )view ).DataContext;
					}
#endif
				}
				else
				{
					if( view is FrameworkElement )
					{
						var dc = view.ReadLocalValue( FrameworkElement.DataContextProperty );
						if( dc != DependencyProperty.UnsetValue )
						{
							return dc;
						}
					}
#if !SILVERLIGHT
					else if( view is FrameworkContentElement )
					{
						var dc = view.ReadLocalValue( FrameworkContentElement.DataContextProperty );
						if( dc != DependencyProperty.UnsetValue )
						{
							return dc;
						}
					}
#endif
				}

				return null;
			};

			this.GetViewDataContext = ( view, behavior ) =>
			{
				return this.DefaultGetViewDataContext( view, behavior );
			};

			this.DefaultSetViewDataContext = ( view, dc ) =>
			{
				if( view is FrameworkElement )
				{
					( ( FrameworkElement )view ).DataContext = dc;
				}
#if !SILVERLIGHT
				else if( view is FrameworkContentElement )
				{
					( ( FrameworkContentElement )view ).DataContext = dc;
				}
#endif
			};

			this.SetViewDataContext = ( view, dc ) =>
			{
				this.DefaultSetViewDataContext( view, dc );
			};

			this.DefaultTryHookClosedEventOfHostOf = ( view, closedCallback ) =>
			{
				//dobbiamo anche cercare una IClosableView oltre che una Window
				var window = this.FindWindowOf( view );
				if( window != null )
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

			this.TryHookClosedEventOfHostOf = ( view, closedCallback ) =>
			{
				return this.DefaultTryHookClosedEventOfHostOf( view, closedCallback );
			};

			this.DefaultIsHostingView = fe => fe.GetType().Name.EndsWith( "View" );

			this.IsHostingView = fe => this.DefaultIsHostingView( fe );

			this.DefaultAttachViewToViewModel = ( view, viewModel ) =>
			{
				viewModel.As<IViewModel>( i =>
				{
					i.View = view;
				} );
			};

			this.AttachViewToViewModel = ( view, viewModel ) =>
			{
				this.DefaultAttachViewToViewModel( view, viewModel );
			};

			this.DefaultGetViewOfViewModel = viewModel =>
			{
				if( viewModel is IViewModel )
				{
					return ( ( IViewModel )viewModel ).View;
				}

				return null;
			};

			this.GetViewOfViewModel = viewModel =>
			{
				return this.DefaultGetViewOfViewModel( viewModel );
			};

#if !SILVERLIGHT
			this.DefaultAttachViewBehaviors = view =>
			{
				var bhv = Interaction.GetBehaviors( view );
				if( view is Window && bhv.OfType<WindowLifecycleNotificationsBehavior>().None() )
				{
					bhv.Add( new WindowLifecycleNotificationsBehavior( this.broker, this ) );
				}
				else if( view is FrameworkElement && bhv.OfType<FrameworkElementLifecycleNotificationsBehavior>().None() )
				{
					bhv.Add( new FrameworkElementLifecycleNotificationsBehavior( this.broker, this ) );
				}

				if( bhv.OfType<DependencyObjectCloseHandlerBehavior>().None() )
				{
					bhv.Add( new DependencyObjectCloseHandlerBehavior( this.broker, this ) );
				}
			};

			this.AttachViewBehaviors = view =>
			{
				this.DefaultAttachViewBehaviors( view );
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
			this.DefaultDetachViewBehaviors = view =>
			{
				var bhv = Interaction.GetBehaviors( view );
				if( view is Window )
				{
					bhv.OfType<WindowLifecycleNotificationsBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
				}
				else if( view is FrameworkElement )
				{
					bhv.OfType<FrameworkElementLifecycleNotificationsBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
				}
				bhv.OfType<DependencyObjectCloseHandlerBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
			};

			this.DetachViewBehaviors = view =>
			{
				this.DefaultDetachViewBehaviors( view );
			};
#else
			this.DetachViewBehaviors = view =>
			{
				var bhv = Interaction.GetBehaviors( view );
				bhv.OfType<FrameworkElementLifecycleNotificationsBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
				bhv.OfType<DependencyObjectCloseHandlerBehavior>().ToList().ForEach( x => bhv.Remove( x ) );
			};
#endif

			this.DefaultShouldNotifyViewModelLoaded = ( view, dataContext ) =>
			{
				if( dataContext == null )
				{
					return false;
				}

				var hasAttribute = dataContext.GetType().IsAttributeDefined<NotifyLoadedAttribute>();
				var hasRegions = RegionService.CurrentService.HoldsRegionManager( view );

				return hasAttribute || hasRegions;
			};

			this.ShouldNotifyViewModelLoaded = ( view, dataContext ) =>
			{
				return this.DefaultShouldNotifyViewModelLoaded( view, dataContext );
			};

			this.DefaultShouldNotifyViewLoaded = view =>
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

			this.ShouldNotifyViewLoaded = view =>
			{
				return this.DefaultShouldNotifyViewLoaded( view );
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
		[IgnorePropertyInjectionAttribue]
		public Func<Type, Type> ResolveViewModelType { get; set; }

		/// <summary>
		/// Default: Gets or sets the view model type resolver that can resolve the view model type given the view type.
		/// </summary>
		/// <value>
		/// The view model type resolver.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, Type> DefaultResolveViewModelType { get; private set; }

		/// <summary>
		/// Gets or sets the view type resolver that can resolve the view type given the view model type.
		/// </summary>
		/// <value>
		/// The view type resolver.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, Type> ResolveViewType { get; set; }

		/// <summary>
		/// Default: Gets or sets the view type resolver that can resolve the view type given the view model type.
		/// </summary>
		/// <value>
		/// The view type resolver.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, Type> DefaultResolveViewType { get; private set; }

		/// <summary>
		/// Gets or sets the window finder.
		/// </summary>
		/// <value>
		/// The window finder.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Object, Window> FindHostingWindowOf { get; set; }

		/// <summary>
		/// Default: Gets or sets the window finder.
		/// </summary>
		/// <value>
		/// The window finder.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Object, Window> DefaultFindHostingWindowOf { get; set; }

		/// <summary>
		/// Gets or sets the View -&gt; window finder, that given a View finds the root hosting Window for the given View.
		/// </summary>
		/// <value>
		/// The find window of.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, Window> FindWindowOf { get; set; }

		/// <summary>
		/// Default: Gets or sets the View -&gt; window finder, that given a View finds the root hosting Window for the given View.
		/// </summary>
		/// <value>
		/// The find window of.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, Window> DefaultFindWindowOf { get; private set; }

		/// <summary>
		/// Gets or sets the logic that determines if view has data context.
		/// </summary>
		/// <value>
		/// The logic that determines if view has data context.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, ViewDataContextSearchBehavior, Boolean> ViewHasDataContext { get; set; }

		/// <summary>
		/// Default: Gets or sets the logic that determines if view has data context.
		/// </summary>
		/// <value>
		/// The logic that determines if view has data context.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, ViewDataContextSearchBehavior, Boolean> DefaultViewHasDataContext { get; private set; }

		/// <summary>
		/// Gets or sets the logic that sets the view data context.
		/// </summary>
		/// <value>
		/// The logic that sets the view data context.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject, Object> SetViewDataContext { get; set; }

		/// <summary>
		/// Default: Gets or sets the logic that sets the view data context.
		/// </summary>
		/// <value>
		/// The logic that sets the view data context.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject, Object> DefaultSetViewDataContext { get; private set; }

		/// <summary>
		/// Gets or sets the logic that gets view data context.
		/// </summary>
		/// <value>
		/// The logic that gets view data context.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, ViewDataContextSearchBehavior, Object> GetViewDataContext { get; set; }

		/// <summary>
		/// Default: Gets or sets the logic that gets view data context.
		/// </summary>
		/// <value>
		/// The logic that gets view data context.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, ViewDataContextSearchBehavior, Object> DefaultGetViewDataContext { get; private set; }

		/// <summary>
		/// Tries to hook closed event of an the element in the visual tree that hosts this given view.
		/// If the hook succedeed the given callback will be called once the hosting element is closed.
		/// </summary>
		/// <returns>
		/// The element, that supports closed notifications, in the visual tree that hosts the given view; otherwise <c>null</c>.
		///   </returns>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, Action<DependencyObject>, DependencyObject> TryHookClosedEventOfHostOf { get; set; }

		/// <summary>
		/// Default: Tries to hook closed event of an the element in the visual tree that hosts this given view.
		/// If the hook succedeed the given callback will be called once the hosting element is closed.
		/// </summary>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, Action<DependencyObject>, DependencyObject> DefaultTryHookClosedEventOfHostOf { get; private set; }


		/// <summary>
		/// Gets or sets the convention that determines if the given FrameworkElement is a hosting view.
		/// </summary>
		/// <value>
		/// The convention that determines if the given FrameworkElement is a hosting view.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<FrameworkElement, bool> IsHostingView { get; set; }

		/// <summary>
		/// Default: Gets or sets the convention that determines if the given FrameworkElement is a hosting view.
		/// </summary>
		/// <value>
		/// The convention that determines if the given FrameworkElement is a hosting view.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<FrameworkElement, bool> DefaultIsHostingView { get; private set; }

		/// <summary>
		/// Gets or sets the attach view to view model handler.
		/// </summary>
		/// <value>
		/// The attach view to view model handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject, object> AttachViewToViewModel { get; set; }

		/// <summary>
		/// Default: Gets or sets the attach view to view model handler.
		/// </summary>
		/// <value>
		/// The attach view to view model handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject, object> DefaultAttachViewToViewModel { get; private set; }

		/// <summary>
		/// Gets the view of the given view model.
		/// </summary>
		/// <value>
		/// The get view of view model handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<object, DependencyObject> GetViewOfViewModel { get; set; }

		/// <summary>
		/// Default: Gets the view of the given view model.
		/// </summary>
		/// <value>
		/// The get view of view model handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<object, DependencyObject> DefaultGetViewOfViewModel { get; private set; }

		/// <summary>
		/// Gets an opportunity toattach behaviors to the view.
		/// </summary>
		/// <value>
		/// The attach view behaviors handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject> AttachViewBehaviors { get; set; }

		/// <summary>
		/// Default: Gets an opportunity to attach behaviors to the view.
		/// </summary>
		/// <value>
		/// The attach view behaviors handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject> DefaultAttachViewBehaviors { get; private set; }

		/// <summary>
		/// Gets an opportunity to detach behaviors from the view.
		/// </summary>
		/// <value>
		/// The detach view behaviors handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject> DetachViewBehaviors { get; set; }

		/// <summary>
		/// Default: Gets an opportunity to detach behaviors from the view.
		/// </summary>
		/// <value>
		/// The detach view behaviors handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject> DefaultDetachViewBehaviors { get; private set; }

		/// <summary>
		/// Gets or sets the logic that determines if ViewModel should notify the loaded message.
		/// </summary>
		/// <value>
		/// The logic that determines if ViewModel should notify the loaded message.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, Object, Boolean> ShouldNotifyViewModelLoaded { get; set; }

		/// <summary>
		/// Default: Gets or sets the logic that determines if ViewModel should notify the loaded message.
		/// </summary>
		/// <value>
		/// The logic that determines if ViewModel should notify the loaded message.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, Object, Boolean> DefaultShouldNotifyViewModelLoaded { get; private set; }

		/// <summary>
		/// Gets or sets the logic that determines if View should notify the loaded message.
		/// </summary>
		/// <value>
		/// The logic that determines if View should notify the loaded message.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, Boolean> ShouldNotifyViewLoaded { get; set; }

		/// <summary>
		/// Default: Gets or sets the logic that determines if View should notify the loaded message.
		/// </summary>
		/// <value>
		/// The logic that determines if View should notify the loaded message.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, Boolean> DefaultShouldNotifyViewLoaded { get; private set; }

		/// <summary>
		/// Gets or sets the view relase handler that is responsible to release views and view models.
		/// </summary>
		/// <value>
		/// The view release handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject, ViewReleaseBehavior> ViewReleaseHandler { get; set; }

		/// <summary>
		/// Default: Gets or sets the view release handler that is responsible to release views and view models.
		/// </summary>
		/// <value>
		/// The view release handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Action<DependencyObject, ViewReleaseBehavior> DefaultViewReleaseHandler { get; private set; }


		/// <summary>
		/// Gets or sets the handler that determines if a region manager for the given view should be un-registered, the default behavior is that the region manager should be realsed if the view is not a singleton view.
		/// </summary>
		/// <value>
		/// The un-register region manager handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, bool> ShouldUnregisterRegionManagerOfView { get; set; }

		/// <summary>
		/// Default: Gets or sets the handler that determines if a region manager for the given view should be un-registered, the default behavior is that the region manager should be realsed if the view is not a singleton view.
		/// </summary>
		/// <value>
		/// The un-register region manager handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, bool> DefaultShouldUnregisterRegionManagerOfView { get; private set; }

		/// <summary>
		/// Gets or sets the handler that determines if a view should be relased, the default behavior is that the view is released if not a singleton view.
		/// </summary>
		/// <value>
		/// The view release handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, bool> ShouldReleaseView { get; set; }

		/// <summary>
		/// Default: Gets or sets the handler that determines if a view should be relased, the default behavior is that the view is released if not a singleton view.
		/// </summary>
		/// <value>
		/// The view release handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, bool> DefaultShouldReleaseView { get; private set; }

		/// <summary>
		/// Gets or sets the handler that determines if a view model should be automatically unsubscribed from all the subscriptions when its view is relased, the default behavior is that the view model is unsubscribed if the view is not a singleton view.
		/// </summary>
		/// <value>
		/// The unsubscribe handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, bool> ShouldUnsubscribeViewModelOnRelease { get; set; }

		/// <summary>
		/// Default: Gets or sets the handler that determines if a view model should be automatically unsubscribed from all the subscriptions when its view is relased, the default behavior is that the view model is unsubscribed if the view is not a singleton view.
		/// </summary>
		/// <value>
		/// The unsubscribe handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<DependencyObject, bool> DefaultShouldUnsubscribeViewModelOnRelease { get; private set; }


		/// <summary>
		/// Gets or sets the default view data context search behavior.
		/// </summary>
		/// <value>
		/// The default view data context search behavior.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public ViewDataContextSearchBehavior DefaultViewDataContextSearchBehavior
		{
			get;
			set;
		}
	}
}
