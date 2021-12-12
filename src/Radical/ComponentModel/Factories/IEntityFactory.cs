using System;

namespace Radical.ComponentModel.Factories
{
#pragma warning disable 1591
    [Obsolete("IEntityFactory has been obsoleted and will be removed in v3.0.0")]
    public interface IEntityFactory
    {
        T Create<T>(params object[] constructorArguments);
        object Create(Type type, params object[] constructorArguments);
    }
#pragma warning restore 1591
}
