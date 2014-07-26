using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Topics.Radical.Windows.Presentation.Boot;
using WpfRadicalMultipleReceiver.Presentation;
using Topics.Radical;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace WpfRadicalMultipleReceiver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var bootstrapper = new WindsorApplicationBootstrapper<MainView>()
                .OnBoot( container =>
                {
                    var c = container.GetService<IConventionsHandler>();
                    c.DefaultViewDataContextSearchBehavior = ViewDataContextSearchBehavior.LocalOnly;
                } );
        }
    }
}
