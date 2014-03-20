using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
    public interface ISettingsCommandsRequestHandler
    {
        void Handle( global::Windows.UI.ApplicationSettings.SettingsPaneCommandsRequestedEventArgs e );
    }
}
