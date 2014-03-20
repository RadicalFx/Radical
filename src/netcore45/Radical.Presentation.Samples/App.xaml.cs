using System.Threading.Tasks;
using Topics.Radical.Windows.Presentation.Boot;
using Topics.Radical.Windows.Presentation.Navigation.Hosts;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Topics.Radical;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Behaviors;
using System.Linq;
using Topics.Radical.Windows.Presentation.Behaviors;

namespace Radical.Presentation.Samples
{
    sealed partial class App : Application
    {
        ApplicationBootstrapper bootstrapper;

        public App()
        {
            this.InitializeComponent();

            this.bootstrapper = new PuzzleApplicationBootstrapper<Presentation.MainView>();
        }
    }
}
