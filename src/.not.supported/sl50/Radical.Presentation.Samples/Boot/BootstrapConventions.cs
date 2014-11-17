using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Topics.Radical.Reflection;
using Topics.Radical.ComponentModel.Messaging;
using System.Xml;
using System.Windows;
using Topics.Radical.Windows.Presentation.ComponentModel.Regions;
//using Topics.Radical.Windows.Presentation.ComponentModel.Regions;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public class BootstrapConventions
    {
        Boolean IsConcreteType( Type t )
        {
            return !t.IsInterface && !t.IsAbstract && !t.IsGenericType;
        }

        public BootstrapConventions()
        {
            this.IsService = t => this.IsConcreteType( t ) && t.Namespace.IsLike( "*.Services" );

            this.SelectServiceContracts = type =>
            {
                var interfaces = type.GetInterfaces();
                if ( interfaces.Any() )
                {
                    return interfaces.Take( 1 );
                }

                return new[] { type };
            };

            this.IsMessageHandler = t => t.Is( typeof( IMessageHandler ) ) && t.Namespace.IsLike( "*.Messaging.Handlers" );

            this.SelectMessageHandlerContracts = type => type.GetInterfaces().Take( 1 );

            this.IsViewModel = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*ViewModel" );

            this.IsShellViewModel = r => r.Implementation.Name.IsLike( "Main*" ) || r.Implementation.Name.IsLike( "Shell*" );

            this.SelectViewModelContracts = type => new[] { type };

            this.IsView = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*View" );

            this.IsShellView = r => r.Implementation.Name.IsLike( "Main*" ) || r.Implementation.Name.IsLike( "Shell*" );

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

            this.EntryXapKnownTypes = () =>
            {
                if ( this.entryXapTypes == null )
                {
                    this.entryXapTypes = this.GetAllEntryXapTypes();
                }

                return this.entryXapTypes;
            };
        }

        IEnumerable<Type> entryXapTypes = null;

        IEnumerable<Type> GetAllEntryXapTypes()
        {
            var temp = new List<Type>();

            var reader = XmlReader.Create( Application.GetResourceStream( new Uri( "AppManifest.xaml", UriKind.Relative ) ).Stream );
            var parts = new AssemblyPartCollection();

            if ( reader.Read() )
            {
                reader.ReadStartElement();

                if ( reader.ReadToNextSibling( "Deployment.Parts" ) )
                {
                    while ( reader.ReadToFollowing( "AssemblyPart" ) )
                    {
                        parts.Add( new AssemblyPart() { Source = reader.GetAttribute( "Source" ) } );
                    }
                }
            }

            foreach ( var part in parts )
            {
                if ( part.Source.ToLower().EndsWith( ".dll" ) )
                {
                    var stream = Application.GetResourceStream( new Uri( part.Source, UriKind.Relative ) ).Stream;
                    var assembly = part.Load( stream );
                    var allTypes = assembly.GetTypes();

                    temp.AddRange( allTypes );
                }
            }

            return temp;
        }

        public Func<IEnumerable<Type>> EntryXapKnownTypes { get; set; }

        public Predicate<Type> IsService { get; set; }

        public Func<Type, IEnumerable<Type>> SelectServiceContracts { get; set; }

        public Predicate<Type> IsMessageHandler { get; set; }

        public Func<Type, IEnumerable<Type>> SelectMessageHandlerContracts { get; set; }

        public Predicate<Type> IsView { get; set; }

        public Predicate<Type> IsViewModel { get; set; }

        public Func<ComponentRegistration, Boolean> IsShellView { get; set; }

        public Func<ComponentRegistration, Boolean> IsShellViewModel { get; set; }

        public Func<Type, IEnumerable<Type>> SelectViewContracts { get; set; }

        public Func<Type, IEnumerable<Type>> SelectViewModelContracts { get; set; }

        public Func<Type, String> GetInterestedRegionNameIfAny { get; set; }
    }
}
