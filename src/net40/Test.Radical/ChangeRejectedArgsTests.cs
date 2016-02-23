//extern alias tpx;

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Topics.Radical.ComponentModel.ChangeTracking;
using SharpTestsEx;

namespace Test.Radical
{
    [TestClass()]
    public class ChangeRejectedArgsTests : ChangeArgsTests
    {
        protected override ChangeEventArgs<T> CreateMock<T>( object entity, T cachedValue, IChange source )
        {
            return this.CreateMock<T>( entity, cachedValue, source, RejectReason.RejectChanges );
        }

        protected ChangeRejectedEventArgs<T> CreateMock<T>( object entity, T cachedValue, IChange source, RejectReason reason )
        {
            return new ChangeRejectedEventArgs<T>( entity, cachedValue, source, reason );
        }

        [TestMethod]
        public void changeRejectedArgs_generic_ctor_normal_should_correctly_set_values()
        {
            var entity = new Object();
            var cachedValue = new GenericParameterHelper();
            var iChange = MockRepository.GenerateStub<IChange>();
            var reason = RejectReason.Redo;

            var target = this.CreateMock<GenericParameterHelper>( entity, cachedValue, iChange, reason );

            target.Entity.Should().Be.EqualTo( entity );
            target.CachedValue.Should().Be.EqualTo( cachedValue );
            target.Source.Should().Be.EqualTo( iChange );
            target.Reason.Should().Be.EqualTo( reason );
        }
    }
}
