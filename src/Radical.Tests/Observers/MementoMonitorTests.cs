namespace Radical.Tests.Observers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using SharpTestsEx;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Observers;

    [TestClass]
    public class MementoMonitorTests
    {
        [TestMethod]
        public void mementoMonitor_Changed_event_normal_should_fire_expected_event()
        {
            var expected = 1;
            var actual = 0;

            var svc = MockRepository.GenerateStub<IChangeTrackingService>();
            var raiser = svc.GetEventRaiser( obj => obj.TrackingServiceStateChanged += null );

            var target = new MementoMonitor( svc );
            target.Changed += ( s, e ) => actual++;

            raiser.Raise( svc, EventArgs.Empty );

            actual.Should().Be.EqualTo( expected );
        }
    }
}
