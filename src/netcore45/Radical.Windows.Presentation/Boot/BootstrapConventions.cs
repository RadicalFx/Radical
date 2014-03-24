using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topics.Radical.Reflection;
using Topics.Radical;
using Topics.Radical.ComponentModel;
using Topics.Radical.Windows.Presentation.ComponentModel.Regions;
using Topics.Radical.ComponentModel.Messaging;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public class BootstrapConventions
    {
        public BootstrapConventions()
        {
            this.IsConcreteType = t => !t.IsInterface && !t.IsAbstract && !t.IsGenericType;

            this.IsService = t =>
            {
                return this.IsConcreteType( t ) && t.Namespace.IsLike( "*.Services" );
            };

            this.SelectServiceContract = type =>
            {
                var interfaces = type.ImplementedInterfaces;
                if ( interfaces.Any() )
                {
                    return interfaces.First().GetTypeInfo();
                }

                return type;
            };

            this.IsMessageHandler = t => typeof( IHandleMessage ).GetTypeInfo().IsAssignableFrom( t ) && t.Namespace.IsLike( "*.Messaging.Handlers" );

            this.SelectMessageHandlerContract = type => type.ImplementedInterfaces.First().GetTypeInfo();

            this.IsViewModel = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*ViewModel" );

            this.IsShellViewModel = r =>
            {
                return r.Name.IsLike( "Main*" ) || r.Name.IsLike( "Shell*" );
            };

            this.SelectViewModelContract = type => type;

            this.IsView = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*View" );

            this.IsShellView = r =>
            {
                return r.Name.IsLike( "Main*" ) || r.Name.IsLike( "Shell*" );
            };

            this.SelectViewContract = type => type;

            this.GetInterestedRegionNameIfAny = type =>
            {
                if ( this.IsView( type ) )
                {
                    var attributes = type.GetCustomAttributes<InjectViewInRegionAttribute>();
                    if ( attributes.Any() )
                    {
                        return attributes.First().Named;
                    }

                    if ( type.Namespace.IsLike( "*.Presentation.Partial.*" ) )
                    {
                        var regionName = type.Namespace.Split( '.' ).Last();
                        return regionName;
                    }
                }

                return null;
            };

            this.IsExcluded = t => false;
        }

        public Predicate<TypeInfo> IsConcreteType { get; set; }

        public Predicate<TypeInfo> IsService { get; set; }

        public Func<TypeInfo, TypeInfo> SelectServiceContract { get; set; }

        public Predicate<TypeInfo> IsMessageHandler { get; set; }

        public Func<TypeInfo, TypeInfo> SelectMessageHandlerContract { get; set; }

        public Predicate<TypeInfo> IsView { get; set; }

        public Predicate<TypeInfo> IsViewModel { get; set; }

        public Func<TypeInfo, Boolean> IsShellView { get; set; }

        public Func<TypeInfo, Boolean> IsShellViewModel { get; set; }

        public Func<TypeInfo, TypeInfo> SelectViewContract { get; set; }

        public Func<TypeInfo, TypeInfo> SelectViewModelContract { get; set; }

        public Func<TypeInfo, String> GetInterestedRegionNameIfAny { get; set; }

        public Func<TypeInfo, Boolean> IsExcluded { get; set; }
    }
}
