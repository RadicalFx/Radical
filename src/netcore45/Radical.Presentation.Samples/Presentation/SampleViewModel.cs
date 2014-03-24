using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Radical.Samples.Presentation
{
    class SampleViewModel : AbstractViewModel
    {
        readonly ISuspensionManager suspentionManager;

        public SampleViewModel( ISuspensionManager suspentionManager )
        {
            this.suspentionManager = suspentionManager;

            this.suspentionManager.SetValue( "my-value-key", "may value", StorageLocation.Roaming );

            var myValue = this.suspentionManager.GetValue<String>( "my-value-key" );
        }
    }
}
