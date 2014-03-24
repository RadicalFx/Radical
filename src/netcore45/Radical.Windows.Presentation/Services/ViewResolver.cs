using System;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Presentation.Services
{
	/// <summary>
	/// Resolves view automatically attaching view models by convention.
	/// </summary>
	class ViewResolver : IViewResolver
	{
		readonly IServiceProvider container;
		readonly IConventionsHandler conventions;
        readonly Action<object> emptyInterceptor = vm => { };

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewResolver"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="conventions">The conventions.</param>
		public ViewResolver( IServiceProvider container, IConventionsHandler conventions )
		{
			Ensure.That( container ).Named( () => container ).IsNotNull();
			Ensure.That( conventions ).Named( () => conventions ).IsNotNull();

			this.container = container;
			this.conventions = conventions;
		}

		/// <summary>
		/// Gets the view of the given type.
		/// </summary>
		/// <param name="viewType"></param>
		/// <returns>
		/// The view instance.
		/// </returns>
        public FrameworkElement GetView( Type viewType )
		{
            return this.GetView( viewType, this.emptyInterceptor );
		}

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <typeparam name="T">The type of the view.</typeparam>
		/// <returns>
		/// The view instance.
		/// </returns>
        public T GetView<T>() where T : FrameworkElement
		{
			return ( T )this.GetView( typeof( T ), this.emptyInterceptor );
		}


        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModelInterceptor">The view model interceptor.</param>
        /// <returns></returns>
        public T GetView<T>( Action<object> viewModelInterceptor ) where T : FrameworkElement
        {
            return ( T )this.GetView( typeof( T ), viewModelInterceptor );
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModelInterceptor">The view model interceptor.</param>
        /// <returns></returns>
        public TView GetView<TView, TViewModel>( Action<TViewModel> viewModelInterceptor ) where TView : FrameworkElement
        {
            return ( TView )this.GetView( typeof( TView ), o => 
            {
                viewModelInterceptor( ( TViewModel )o );
            } );
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="viewModelInterceptor">The view model interceptor.</param>
        /// <returns></returns>
        public FrameworkElement GetView( Type viewType, Action<object> viewModelInterceptor )
        {
            Ensure.That( viewModelInterceptor ).Named( () => viewModelInterceptor ).IsNotNull();

            var view = ( FrameworkElement )this.container.GetService( viewType );

            if ( !this.conventions.ViewHasDataContext( view ) )
            {
                var viewModelType = this.conventions.ResolveViewModelType( viewType );
                if ( viewModelType != null )
                {
                    //we support view(s) without ViewModel

                    var viewModel = this.container.GetService( viewModelType );

                    viewModelInterceptor( viewModel );

                    this.conventions.AttachViewToViewModel( view, viewModel );
                    this.conventions.SetViewDataContext( view, viewModel );
                    this.conventions.AttachViewBehaviors( view );
                }
            }

            return view;
        }
    }
}