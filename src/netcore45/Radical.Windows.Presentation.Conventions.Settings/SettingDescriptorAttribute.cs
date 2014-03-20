using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.Windows.Presentation.Conventions.Settings
{
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
    public class SettingDescriptorAttribute : Attribute
    {
        public String CommandId { get; set; }

        public String CommandLabel { get; set; }
    }
}
