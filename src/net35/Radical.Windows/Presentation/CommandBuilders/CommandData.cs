using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Reflection;
using Topics.Radical.ComponentModel.Windows.Input;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Windows.CommandBuilders
{
    public class CommandData
    {
        public Object DataContext;
        public LateBoundVoidMethod FastDelegate;

        public Fact Fact;
        public BooleanFact BooleanFact;

        public Boolean HasParameter;
        public Type ParameterType;

        public KeyBindingAttribute[] KeyBindings;
        public CommandDescriptionAttribute Description;

        public IMonitor Monitor;
    }
}
