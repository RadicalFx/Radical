using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Topics.Radical.ComponentModel
{
    public interface IPuzzleSetupDescriptor
    {
        void Setup( IPuzzleContainer container, Func<IEnumerable<Type>> knownTypesProvider );
    }
}
