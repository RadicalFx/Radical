//extern alias tpx;

using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radical.ComponentModel.ChangeTracking;
using SharpTestsEx;
using System;

namespace Radical.Tests
{
    [TestClass()]
    public class ChangeArgsTests
    {
        [TestMethod]
        public void changeArgs_generic_ctor_normal_should_correctly_set_values()
        {
            var entity = new Object();
            var cachedValue = new GenericParameterHelper();
            var iChange = A.Dummy<IChange>();

            var target = new ChangeEventArgs<GenericParameterHelper>(entity, cachedValue, iChange);

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
            var iChange = A.Dummy<IChange>();

            var target = new ChangeEventArgs<GenericParameterHelper>(entity, cachedValue, iChange);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void changeArgs_ctor_using_null_reference_iChange_should_raise_ArgumentNullException()
        {
            var entity = new Object();
            var cachedValue = new GenericParameterHelper();
            IChange iChange = null;

            var target = new ChangeEventArgs<GenericParameterHelper>(entity, cachedValue, iChange);
        }
    }
}
