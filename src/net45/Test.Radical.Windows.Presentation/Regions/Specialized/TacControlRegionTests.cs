using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.Windows.Presentation.Regions;
using System.Windows.Controls;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using System.Windows.Markup;
using System.Windows;

namespace Test.Radical.Windows.Presentation.Regions.Specialized
{
    [TestClass]
    public class TacControlRegionTests
    {
        class TestTabControlRegion : TabControlRegion
        {
            public TestTabControlRegion()
            {
                this.TestHostingView = new Window();
            }

            public DependencyObject TestHostingView { get; set; }

            protected override DependencyObject FindHostingViewOf( FrameworkElement fe )
            {
                return this.TestHostingView;
            }

            protected override object TryGetViewModel( DependencyObject view )
            {
                return ( ( FrameworkElement )view ).DataContext;
            }
        }

        class TestViewModel : AbstractViewModel, IExpectViewActivatedCallback
        {

            public void OnViewActivated()
            {
                this.Invoked = true;
            }

            public bool Invoked { get; private set; }
        }

        class HardCodedServiceProvider : IServiceProvider
        {
            IProvideValueTarget ipvt = new IPVT()
            {
                TargetObject = new TabControl()
            };

            public HardCodedServiceProvider()
            {

            }

            public object GetService( Type serviceType )
            {
                return this.ipvt;
            }
        }

        class IPVT : IProvideValueTarget
        {
            public object TargetObject
            {
                get;
                set;
            }

            public object TargetProperty
            {
                get;
                set;
            }
        }

        [TestMethod]
        [TestCategory( "TabControlRegion" ), TestCategory( "Regions" ), TestCategory( "UI Composition" )]
        public void TabControlRegion_ActiveContentChanged_should_notify_VM_if_IExpectViewActivatedCallback()
        {
            var sut = new TestTabControlRegion();
            sut.ProvideValue( new HardCodedServiceProvider() );

            var vm1 = new TestViewModel();
            var item1 = new UserControl()
            {
                DataContext = vm1
            };
            sut.Add( item1 );

            var vm2 = new TestViewModel();
            var item2 = new UserControl()
            {
                DataContext = vm2
            };
            sut.Add( item2 );

            sut.Activate( item2 );
            sut.Activate( item1 );

            Assert.IsTrue( vm1.Invoked );
            Assert.IsTrue( vm2.Invoked );
        }
    }
}
