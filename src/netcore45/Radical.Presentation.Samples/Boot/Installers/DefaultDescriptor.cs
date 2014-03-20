//using System;
//using System.Collections.Generic;
//using System.Composition;
//using System.Diagnostics;
//using System.Reflection;
//using System.Threading.Tasks;
//using System.Windows;
//using Topics.Radical;
//using Topics.Radical.ComponentModel;
//using Topics.Radical.ComponentModel.Messaging;
//using Topics.Radical.Messaging;
//using Topics.Radical.Windows.Presentation.Navigation.Hosts;
//using Windows.ApplicationModel.Core;
//using Windows.UI.Core;
//using Windows.UI.Xaml;

//namespace Radical.Presentation.Samples.Boot.Installers
//{
//    [Export( typeof( IPuzzleSetupDescriptor ) )]
//    public class DefaultDescriptor : IPuzzleSetupDescriptor
//    {
//        public async Task Setup( IPuzzleContainer container, Func<IEnumerable<TypeInfo>> knownTypesProvider )
//        {
//            container.Register(
//                EntryBuilder.For<NavigationHost>()
//                    .UsingFactory( () =>
//                    {
//                        return new WindowNavigationHost( Window.Current );
//                    } )
//                    .WithLifestyle( Lifestyle.Singleton )
//            );
//        }
//    }
//}