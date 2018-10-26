using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical;
using SharpTestsEx;

namespace Radical.Tests
{
    [TestClass()]
    public class WeakReferenceTest
    {
        [TestMethod]
        public void weakReference_ctor_target_should_set_target()
        {
            var expected = new Object();
            var actual = new WeakReference<Object>( expected );

            actual.Target.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void weakReference_ctor_target_and_trackResurrection_should_set_target_and_resurrection_tracking()
        {
            var expected = new Object();
            var actual = new WeakReference<Object>( expected, true );

            actual.Target.Should().Be.EqualTo( expected );
            actual.TrackResurrection.Should().Be.True();
        }
    }
}
