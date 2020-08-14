using System;
using System.Collections.Generic;

namespace Radical.ComponentModel
{
    [Obsolete("IPuzzleSetupDescriptor has been obsoleted and will be removed in v3.0.0")]
    public interface IPuzzleSetupDescriptor
    {
        void Setup(IPuzzleContainer container, Func<IEnumerable<Type>> knownTypesProvider);
    }
}
