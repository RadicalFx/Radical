using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Topics.Radical.ComponentModel;
//using Topics.Radical.Reflection;

namespace Topics.Radical
{
    /// <summary>
    /// Defines a facility that automatically boot a component at registration time.
    /// </summary>
    public class BootableFacility : IPuzzleContainerFacility
    {
        IPuzzleContainer container;

        /// <summary>
        /// Initializes this facility.
        /// </summary>
        /// <param name="container">The container hosting the facility.</param>
        public void Initialize(IPuzzleContainer container)
        {
            this.container = container;
            container.ComponentRegistered += new EventHandler<ComponentRegisteredEventArgs>(OnComponentRegistered);
        }

        Boolean IsBootable(TypeInfo type)
        {
            return type.IsAssignableFrom(typeof(IBootable).GetTypeInfo());
        }

        void OnComponentRegistered(object sender, ComponentRegisteredEventArgs e)
        {
            if (this.IsBootable(e.Entry.Service) || this.IsBootable(e.Entry.Component))
            {
                var t = this.GetTypeToResolve(e.Entry);
                var svc = (IBootable)this.container.Resolve(t);
                svc.Boot();
            }
        }

        TypeInfo GetTypeToResolve(IContainerEntry entry)
        {
            return entry.Service ?? entry.Component;
        }

        /// <summary>
        /// Teardowns this facility.
        /// </summary>
        /// <param name="container">The container hosting the facility.</param>
        public void Teardown(IPuzzleContainer container)
        {
            container.ComponentRegistered -= new EventHandler<ComponentRegisteredEventArgs>(OnComponentRegistered);
        }
    }
}
