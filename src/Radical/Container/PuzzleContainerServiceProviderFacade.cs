using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Radical.ComponentModel;

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
