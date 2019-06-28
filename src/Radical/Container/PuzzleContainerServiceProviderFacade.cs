using Radical.ComponentModel;
using System;

namespace Radical
{
    public class PuzzleContainerServiceProviderFacade : IServiceProvider
    {
        readonly IPuzzleContainer container;

        public PuzzleContainerServiceProviderFacade(IPuzzleContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            if(this.container.IsRegistered(serviceType))
            {
                return this.container.Resolve(serviceType);
            }

            return null;
        }
    }
}
