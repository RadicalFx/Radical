//using System;
//using System.Windows;

//namespace Topics.Radical.Windows.Presentation.ComponentModel
//{
//	/// <summary>
//	/// Resolves view models, ideal to support a ViewModel first approach
//	/// </summary>
//	public interface IViewModelResolver
//	{
//		public interface IOutput
//		{
//			Object ViewModel { get; }
//			DependencyObject View { get; }
//		}

//		public interface IOutput<T>
//		{
//			T ViewModel { get; }
//			DependencyObject View { get; }
//		}

//		public interface IOutput<TViewModel, TView>
//			where TView : DependencyObject
//		{
//			TViewModel ViewModel { get; }
//			TView View { get; }
//		}

//		IOutput<T> GetViewModel<T>();
//		IOutput<T> GetViewModel<T>( Action<T> viewModelInterceptor );

//		IOutput<TViewModel, TView> GetViewModel<TViewModel, TView>( Action<TViewModel> viewModelInterceptor )
//			where TView : DependencyObject;

//		IOutput GetViewModel( Type viewModelType );

//		IOutput GetViewModel( Type viewType, Action<Object> viewModelInterceptor );
//	}
//}
