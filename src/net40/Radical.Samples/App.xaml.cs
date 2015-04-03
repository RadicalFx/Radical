using System.Windows;
using Castle;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Topics.Radical.Windows.Presentation.Boot;

namespace Topics.Radical
{
	public partial class App : Application
	{
		public App()
		{
			var bootstrapper = new WindsorApplicationBootstrapper<Presentation.MainView>()
				.EnableSplashScreen()
				.OnBoot( container =>
				{
					var w = ( ServiceProviderWrapper )container;
					w.Container.AddFacility<TypedFactoryFacility>();
				} );
		}
	}
}
