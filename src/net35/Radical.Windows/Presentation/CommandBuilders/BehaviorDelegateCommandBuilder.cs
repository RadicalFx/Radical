using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Windows.Behaviors;
using System.Windows;

namespace Topics.Radical.Windows.CommandBuilders
{
    class BehaviorDelegateCommandBuilder : DelegateCommandBuilder
    {
        public override bool CanCreateCommand( System.Windows.PropertyPath path, System.Windows.DependencyObject target )
        {
            return path != null && target is INotifyAttachedOjectLoaded;
        }

        public override object GetDataContext( System.Windows.DependencyObject target )
        {
            return ( ( INotifyAttachedOjectLoaded )target )
                    .GetAttachedObject<FrameworkElement>()
                    .DataContext;
        }
    }
}
