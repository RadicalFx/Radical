using System;
using Topics.Radical.ComponentModel.Factories;

namespace Topics.Radical.ComponentModel
{
    public interface IDataContextFactoryProvider
    {
        IDataContextFactory GetDefaultInstance();
        IDataContextFactory GetInstance( String name );
    }
}
