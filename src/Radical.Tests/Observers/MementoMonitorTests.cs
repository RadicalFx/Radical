namespace Radical.Tests.Observers
{
    using FakeItEasy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Observers;
    using SharpTestsEx;
    using System;

    [TestClass]
    public class MementoMonitorTests
    {
        [TestMethod]
        public void mementoMonitor_Changed_event_normal_should_fire_expected_event()
        {
            var expected = 1;
            var actual = 0;

            var svc = A.Fake<IChangeTrackingService>();

            var target = new MementoMonitor(svc);
            target.Changed += (s, e) => actual++;

            svc.TrackingServiceStateChanged += Raise.With(sender: svc, e: EventArgs.Empty);

            actual.Should().Be.EqualTo(expected);
        }
    }
}
