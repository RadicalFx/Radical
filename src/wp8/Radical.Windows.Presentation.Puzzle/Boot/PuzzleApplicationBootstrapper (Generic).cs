using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public class PuzzleApplicationBootstrapper<TShellView> :
        PuzzleApplicationBootstrapper
        where TShellView : UIElement
    {
        public PuzzleApplicationBootstrapper()
        {
            this.DefineHomeAs<TShellView>();
        }
    }
}
