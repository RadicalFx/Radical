namespace Radical.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.Linq;
    using SharpTestsEx;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class ListExtensionsTests
    {
        [TestMethod]
        public void listExtensions_sync_using_empty_lists_should_not_fail()
        {
            var source = new List<object>();
            var to = new List<object>();

            source.Sync(to);

            to.Should().Have.SameSequenceAs(source);
        }

        [TestMethod]
        public void listExtensions_sync_using_non_empty_source_list_should_sync_as_expected()
        {
            var source = new List<object>()
            {
                new object(), new object()
            };

            var to = new List<object>();

            source.Sync(to);

            to.Should().Have.SameSequenceAs(source);
        }

        [TestMethod]
        public void listExtensions_sync_using_empty_source_but_non_empty_to_should_sync_as_expected()
        {
            var source = new List<object>();
            var to = new List<object>()
            {
                new object(), new object()
            };

            source.Sync(to);

            to.Should().Have.SameSequenceAs(source);
        }

        [TestMethod]
        public void listExtensions_sync_using_non_empty_source_and_non_empty_to_should_sync_as_expected()
        {
            var source = new List<object>()
            {
                new object()
            };

            var to = new List<object>()
            {
                new object(), new object()
            };

            source.Sync(to);

            to.Should().Have.SameSequenceAs(source);
        }

        [TestMethod]
        public void listExtensions_sync_using_source_and_to_with_a_shared_item_should_sync_as_expected()
        {
            var same = new object();
            var source = new List<object>()
            {
                same,
                new object()
            };

            var to = new List<object>()
            {
                same, new object(), new object()
            };

            source.Sync(to);

            to.Should().Have.SameSequenceAs(source);
        }

        [TestMethod]
        public void listExtensions_sync_using_source_and_to_with_a_shared_item_in_different_position_should_sync_as_expected()
        {
            var same = new object();
            var source = new List<object>()
            {
                same,
                new object()
            };

            var to = new List<object>()
            {
                new object(), same, new object()
            };

            source.Sync(to);

            to.Should().Have.SameSequenceAs(source);
        }
    }
}
