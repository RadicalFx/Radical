using System;
using System.Collections.Generic;

namespace Radical.ComponentModel
{
    public interface IPuzzleSetupDescriptor
    {
        void Setup(IPuzzleContainer container, Func<IEnumerable<Type>> knownTypesProvider);
    }
}
