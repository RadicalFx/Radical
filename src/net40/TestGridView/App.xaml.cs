using System;
using System.Linq;
using System.Linq.Expressions;
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

			//Expression<Func<Person, Object>> foo = p => p.LastName;
			//Expression<Func<Person, Object>> bar = p => p.LastName;

			//var x = foo.GetHashCode() == bar.GetHashCode();

			//New Guid cosi posso aprire + volte lo stesso programma se servisse.
            var bootstrapper = new WindsorApplicationBootstrapper<Presentation.MainView>()
                 .RegisterAsSingleton(Guid.NewGuid().ToString(), SingletonApplicationScope.Local);

        }


		class Person 
		{
			public String FirstName { get; set; }

			public String LastName { get; set; }
		}
    }
}
