using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
    /// <summary>
    /// When attached to a View prevents the View and the ViewModel associated to be automatically disposed.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class )]
    public class ViewManualReleaseAttribute : Attribute
    {
    }
}
