using System;

namespace Topics.Radical.ComponentModel.Factories
{
    public interface IEntityFactory
    {
        T Create<T>( params object[] constructorArguments );
        object Create( Type type, params object[] constructorArguments );
    }
}
