using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Topics.Radical.Windows.Presentation.Boot;
using WpfRadicalMultipleReceiver.Presentation;

namespace WpfRadicalMultipleReceiver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var bootstrapper = new WindsorApplicationBootstrapper<MainView>();
        }
    }
}
