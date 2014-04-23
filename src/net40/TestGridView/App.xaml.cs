using System;
using System.Linq;
using System.Windows;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation.Boot;

namespace TestGridView
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
			Ensure.SourceInfoLoadStrategy = SourceInfoLoadStrategy.Skip;

            //New Guid cosi posso aprire + volte lo stesso programma se servisse.
            var bootstrapper = new WindsorApplicationBootstrapper<Presentation.MainView>()
                 .RegisterAsSingleton(Guid.NewGuid().ToString(), SingletonApplicationScope.Local);

        }

       
    }
}
