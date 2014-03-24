using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Windows.Presentation.Boot
{
    /// <summary>
    /// Instructs the automatic registration process to ignore
    /// a type marked with this attribue.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class )]
    public class DisableAutomaticRegistrationAttribute : Attribute
    {
    }
}
