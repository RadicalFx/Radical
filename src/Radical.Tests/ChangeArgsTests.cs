//extern alias tpx;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel.ChangeTracking;
using Rhino.Mocks;
using SharpTestsEx;
using System;

namespace Radical.Tests
{
    [TestClass()]
    public class ChangeArgsTests
    {
        protected virtual ChangeEventArgs<T> CreateMock<T>(Object entity, T cachedValue, IChange source)
        {
            return new ChangeEventArgs<T>(entity, cachedValue, source);
        }

        [TestMethod]
        public void changeArgs_generic_ctor_normal_should_correctly_set_values()
        {
            var entity = new Object();
            var cachedValue = new GenericParameterHelper();
            var iChange = MockRepository.GenerateStub<IChange>();

            var target = this.CreateMock<GenericParameterHelper>(entity, cachedValue, iChange);

            target.Entity.Should().Be.EqualTo(entity);
            target.CachedValue.Should().Be.EqualTo(cachedValue);
            target.Source.Should().Be.EqualTo(iChange);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void changeArgs_ctor_using_null_reference_entity_should_raise_ArgumentNullException()
        {
            Object entity = null;
            var cachedValue = new GenericParameterHelper();
            var iChange = MockRepository.GenerateStub<IChange>();

            var target = this.CreateMock<GenericParameterHelper>(entity, cachedValue, iChange);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void changeArgs_ctor_using_null_reference_iChange_should_raise_ArgumentNullException()
        {
            var entity = new Object();
            var cachedValue = new GenericParameterHelper();
            IChange iChange = null;

            var target = this.CreateMock<GenericParameterHelper>(entity, cachedValue, iChange);
        }
    }
}
