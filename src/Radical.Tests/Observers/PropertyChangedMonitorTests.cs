//extern alias tpx;

namespace Radical.Tests.Observers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical;
    using Radical.Observers;
    using SharpTestsEx;
    using System.ComponentModel;

    [TestClass]
    public class PropertyChangedMonitorTests
    {
        class TestStub : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged(string name)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }

            private string _value = null;
            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != Value)
                    {
                        _value = value;
                        OnPropertyChanged("Value");
                    }
                }
            }

            private readonly Observable<string> _text = new Observable<string>();
            public Observable<string> Text
            {
                get { return _text; }
            }
        }

        [TestMethod]
        public void propertyChangedMonitor_Observe_using_clr_property_should_behave_as_expected()
        {
            var expected = 1;
            var actual = 0;

            var stub = new TestStub();
            var target = new PropertyChangedMonitor<TestStub>(stub);
            target.Observe(s => s.Value);
            target.Changed += (s, e) => actual++;

            stub.Value = "Hello!";

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void propertyChangedMonitor_Observe_using_observable_property_should_behave_as_expected()
        {
            var expected = 1;
            var actual = 0;

            var stub = new TestStub();
            var target = new PropertyChangedMonitor<TestStub>(stub);
            target.Observe(stub.Text);
            target.Changed += (s, e) => actual++;

            stub.Text.Value = "Hello!";

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void propertyChangedMonitor_StopObserving_using_observable_property_should_behave_as_expected()
        {
            var expected = 2;
            var actual = 0;

            var stub = new TestStub();

            var target = new PropertyChangedMonitor<TestStub>(stub);
            target.Observe(stub.Text);
            target.Changed += (s, e) => actual++;

            stub.Text.Value = "Hello!";
            stub.Text.Value = "Should raise...";

            target.StopObserving(stub.Text);
            stub.Text.Value = "should not raise...";

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void propertyChangedMonitor_ForAllProperties_using_clr_property_should_behave_as_expected()
        {
            var expected = 1;
            var actual = 0;

            var stub = new TestStub();
            var target = new PropertyChangedMonitor(stub);
            target.Changed += (s, e) => actual++;

            stub.Value = "Hello!";

            actual.Should().Be.EqualTo(expected);
        }
    }
}
