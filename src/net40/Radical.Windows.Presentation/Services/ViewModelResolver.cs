//using System;
//using System.Windows;
//using Topics.Radical.Validation;
//using Topics.Radical.Windows.Presentation.ComponentModel;

//namespace Topics.Radical.Windows.Presentation.Services
//{
//	/// <summary>
//	/// Resolves view models automatically attaching to view by convention.
//	/// </summary>
//	class ViewModelResolver : IViewModelResolver
//	{
//		class Output : IViewModelResolver.IOutput
//		{
//			public object ViewModel { get; set; }
//			public DependencyObject View { get; set; }
//		}

//		class Output<T> : IViewModelResolver.IOutput<T>
//		{
//			public T ViewModel { get; set; }
//			public DependencyObject View { get; set; }
//		}

//		class Output<TViewModel, TView> : IViewModelResolver.IOutput<TViewModel, TView>
//			where TView : DependencyObject
//		{
//			public TViewModel ViewModel { get; set; }
//			public TView View { get; set; }
//		}

//		readonly IServiceProvider container;
//		readonly IConventionsHandler conventions;
//		readonly Action<Object> emptyInterceptor = o => { };

//		/// <summary>
//		/// Initializes a new instance of the <see cref="ViewModelResolver"/> class.
//		/// </summary>
//		/// <param name="container">The container.</param>
//		/// <param name="conventions">The conventions.</param>
//		public ViewModelResolver( IServiceProvider container, IConventionsHandler conventions )
//		{
//			Ensure.That( container ).Named( () => container ).IsNotNull();
//			Ensure.That( conventions ).Named( () => conventions ).IsNotNull();

//			this.container = container;
//			this.conventions = conventions;
//		}

//		//public DependencyObject GetView( Type viewType )
//		//{
//		//	return this.GetView( viewType, this.emptyInterceptor );
//		//}

//		//public T GetView<T>() where T : DependencyObject
//		//{
//		//	return ( T )this.GetView( typeof( T ), this.emptyInterceptor );
//		//}

//		//public T GetView<T>( Action<object> viewModelInterceptor ) where T : DependencyObject
//		//{
//		//	return ( T )this.GetView( typeof( T ), viewModelInterceptor );
//		//}

//		//public TView GetView<TView, TViewModel>( Action<TViewModel> viewModelInterceptor ) where TView : DependencyObject
//		//{
//		//	return ( TView )this.GetView( typeof( TView ), o =>
//		//	{
//		//		viewModelInterceptor( ( TViewModel )o );
//		//	} );
//		//}

//		//public DependencyObject GetView( Type viewType, Action<object> viewModelInterceptor )
//		//{
//		//	var view = ( DependencyObject )this.container.GetService( viewType );

//		//	if ( !this.conventions.ViewHasDataContext( view ) )
//		//	{
//		//		var viewModelType = this.conventions.ResolveViewModelType( viewType );
//		//		if ( viewModelType != null )
//		//		{
//		//			//we support view(s) without ViewModel

//		//			var viewModel = this.container.GetService( viewModelType );

//		//			viewModelInterceptor( viewModel );

//		//			this.conventions.AttachViewToViewModel( view, viewModel );
//		//			this.conventions.SetViewDataContext( view, viewModel );
//		//			this.conventions.AttachViewBehaviors( view );
//		//		}
//		//	}

//		//	return view;
//		//}

//		public IViewModelResolver.IOutput<T> GetViewModel<T>()
//		{
//			throw new NotImplementedException();
//		}

//		public IViewModelResolver.IOutput<T> GetViewModel<T>( Action<T> viewModelInterceptor )
//		{
//			throw new NotImplementedException();
//		}

//		public IViewModelResolver.IOutput<TViewModel, TView> GetViewModel<TViewModel, TView>( Action<TViewModel> viewModelInterceptor ) where TView : DependencyObject
//		{
//			throw new NotImplementedException();
//		}

//		public IViewModelResolver.IOutput GetViewModel( Type viewModelType )
//		{
//			throw new NotImplementedException();
//		}

//		public IViewModelResolver.IOutput GetViewModel( Type viewType, Action<object> viewModelInterceptor )
//		{
//			throw new NotImplementedException();
//		}
//	}
//}