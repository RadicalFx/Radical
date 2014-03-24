using System;
using System.Windows;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class UnityApplicationBootstrapper<TShellView> : 
		UnityApplicationBootstrapper 
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
