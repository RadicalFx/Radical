using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Radical.Windows.Presentation.Conventions.Settings.ComponentModel
{
    public interface ISettingsViewModel
    {
        SolidColorBrush HeaderBrush { get; }
        SolidColorBrush Background { get; }
        String HeaderText { get; }
    }
}
