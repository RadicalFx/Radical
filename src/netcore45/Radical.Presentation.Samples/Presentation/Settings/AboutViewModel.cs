using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radical.Windows.Presentation.Conventions.Settings;
using Radical.Windows.Presentation.Conventions.Settings.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Radical.Presentation.Samples.Presentation.Settings
{
    [SettingDescriptorAttribute( CommandId = "about", CommandLabel = "About" )]
    class AboutViewModel : ISettingsViewModel
    {
        public SolidColorBrush HeaderBrush
        {
            get { return new SolidColorBrush( Colors.Yellow ); }
        }

        public SolidColorBrush Background
        {
            get { return new SolidColorBrush( Colors.Gainsboro ); }
        }

        public string HeaderText
        {
            get { return "About Header"; }
        }
    }
}
