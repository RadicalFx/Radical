using System;

namespace Radical.ComponentModel.Factories
{
    [Obsolete("IEntityFactory has been obsoleted and will be removed in v3.0.0")]
    public interface IEntityFactory
    {
        T Create<T>(params object[] constructorArguments);
        object Create(Type type, params object[] constructorArguments);
    }
}
