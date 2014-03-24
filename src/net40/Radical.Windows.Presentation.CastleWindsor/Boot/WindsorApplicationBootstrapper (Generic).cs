using System;
using System.Windows;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class WindsorApplicationBootstrapper<TShellView> : 
		WindsorApplicationBootstrapper 
		where TShellView : Window
	{
		protected override void OnBootCompleted( IServiceProvider serviceProvider )
		{
			var mainView = serviceProvider.GetService<IViewResolver>()
				.GetView<TShellView>();

			Application.Current.MainWindow = mainView;

			mainView.Show();

			base.OnBootCompleted( serviceProvider );
		}
	}
}
