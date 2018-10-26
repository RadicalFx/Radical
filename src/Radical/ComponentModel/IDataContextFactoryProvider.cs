using System;
using Radical.ComponentModel.Factories;

namespace Radical.ComponentModel
{
    public interface IDataContextFactoryProvider
    {
        IDataContextFactory GetDefaultInstance();
        IDataContextFactory GetInstance( String name );
    }
}
