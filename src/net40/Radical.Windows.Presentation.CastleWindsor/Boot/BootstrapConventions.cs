using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Presentation.ComponentModel.Regions;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public class BootstrapConventions
    {
        public BootstrapConventions()
        {
            this.IsConcreteType = t => !t.IsInterface && !t.IsAbstract && !t.IsGenericType;

            this.IsService = t => this.IsConcreteType( t ) && t.Namespace.IsLike( "*.Services" );

            this.SelectServiceContracts = type =>
            {
                var types = new HashSet<Type>( type.GetInterfaces() );
                if ( types.None() || type.IsAttributeDefined<ContractAttribute>() )
                {
                    types.Add( type );
                }

                return types;
            };

            this.IsMessageHandler = t =>
            {
                return t.Namespace != null && t.Namespace.IsLike( "*.Messaging.Handlers" ) && ( t.Is<IMessageHandler>() || t.Is<IHandleMessage>() );
            };

            this.SelectMessageHandlerContracts = type => type.GetInterfaces().Take( 1 );

            this.IsViewModel = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*ViewModel" );

            this.IsShellViewModel = ( services, implementation ) =>
            {
                return services.Any( t => t.Name.IsLike( "Main*" ) || t.Name.IsLike( "Shell*" ) );
            };

            this.SelectViewModelContracts = type => new[] { type };

            this.IsView = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*View" );

            this.IsShellView = ( services, implementation ) =>
            {
                return services.Any( t => t.Name.IsLike( "Main*" ) || t.Name.IsLike( "Shell*" ) );
            };

            this.SelectViewContracts = type => new[] { type };

            this.GetInterestedRegionNameIfAny = type =>
            {
                if ( this.IsView( type ) )
                {
                    if ( type.IsAttributeDefined<InjectViewInRegionAttribute>() )
                    {
                        return type.GetAttribute<InjectViewInRegionAttribute>().Named;
                    }

                    if ( type.Namespace.IsLike( "*.Presentation.Partial.*" ) )
                    {
                        var regionName = type.Namespace.Split( '.' ).Last();
                        return regionName;
                    }
                }

                return null;
            };

            this.IsExcluded = t =>
            {
                return t.IsAttributeDefined<DisableAutomaticRegistrationAttribute>();
            };
        }

        public Predicate<Type> IsConcreteType { get; set; }

        public Predicate<Type> IsService { get; set; }

        public Func<Type, IEnumerable<Type>> SelectServiceContracts { get; set; }

        public Predicate<Type> IsMessageHandler { get; set; }

        public Func<Type, IEnumerable<Type>> SelectMessageHandlerContracts { get; set; }

        public Predicate<Type> IsView { get; set; }

        public Predicate<Type> IsViewModel { get; set; }

        public Func<IEnumerable<Type>, Type, Boolean> IsShellView { get; set; }

        public Func<IEnumerable<Type>, Type, Boolean> IsShellViewModel { get; set; }

        public Func<Type, IEnumerable<Type>> SelectViewContracts { get; set; }

        public Func<Type, IEnumerable<Type>> SelectViewModelContracts { get; set; }

        public Func<Type, String> GetInterestedRegionNameIfAny { get; set; }

        public Func<Type, Boolean> IsExcluded { get; set; }
    }
}
