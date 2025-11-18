//extern alias tpx;

namespace Radical.Tests.Extensions
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.Linq;
    using SharpTestsEx;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass()]
    public class EnumerableExtensionTest
    {
        [TestMethod()]
        public void ForEachTest_null_list()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                Linq.EnumerableExtensions.ForEach<int>(null, a => a++);
            });
        }

        [TestMethod()]
        public void ForEachTest_null_action()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                Linq.EnumerableExtensions.ForEach<int>(new[] { 0, 1, 2 }, null);
            });
        }

        [TestMethod()]
        public void ForEachTest()
        {
            var count = 0;
            var source = new[] { 0, 1, 2 };
            Linq.EnumerableExtensions.ForEach<int>(source, a => count++);

            Assert.AreEqual<int>(source.Length, count);
        }

        [TestMethod()]
        public void ForEachTest_return_value_not_null()
        {
            var source = new[] { 0, 1, 2 };
            var actual = Linq.EnumerableExtensions.ForEach<int>(source, a => a++);

            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void AsReadOnly_null_expects_exception()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                Linq.EnumerableExtensions.AsReadOnly<object>(null);
            });
        }

        [TestMethod()]
        public void AsReadOnly_result_isNotNull()
        {
            var source = new[] { 0, 1, 2 };
            IEnumerable<int> actual = Linq.EnumerableExtensions.AsReadOnly(source);

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void iEnumerableExtensions_enumerate_normal_should_invoke_actions()
        {
            var list = new object[]
            {
                new object(),
                new object(),
                new object(),
                new object()
            };

            int expected = list.Length;
            int actual = 0;

            Action<object> action = o => { actual++; };

            Linq.EnumerableExtensions.Enumerate(list, action);

            actual.Should().Be.EqualTo(expected);
        }

        [TestMethod]
        public void iEnumerableExtensions_enumerate_null_list_reference_should_raise_ArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                Linq.EnumerableExtensions.Enumerate(null, null);
            });
        }

        [TestMethod]
        public void iEnumerableExtensions_enumerate_null_action_reference_should_raise_ArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                Linq.EnumerableExtensions.Enumerate(new object[0], null);
            });
        }

        [TestMethod]
        public void iEnumerableExtensions_enumerate_normal_should_return_valid_list_reference()
        {
            var expected = new object[]
            {
                new object(),
                new object(),
                new object(),
                new object()
            };

            var actual = Linq.EnumerableExtensions.Enumerate(expected, o => { });

            Enumerable.OfType<object>(actual).Should().Have.SameSequenceAs(expected);
        }

        [TestMethod]
        public void enumerableExtensions_alternateWith_using_valid_list_should_alternate_with_separator()
        {
            var list = new List<int>()
            {
                1,2,3,4
            };

            var actual = Linq.EnumerableExtensions.AlternateWith(list, 0);

            actual.Count().Should().Be.EqualTo(7);
            actual.ElementAt(1).Should().Be.EqualTo(0);
            actual.ElementAt(3).Should().Be.EqualTo(0);
            actual.ElementAt(5).Should().Be.EqualTo(0);
        }

        [TestMethod]
        public void enumerableExtensions_alternateWith_using_empty_list_should_return_empty_list()
        {
            var list = new List<string>();
            var actual = Linq.EnumerableExtensions.AlternateWith(list, "");

            actual.Count().Should().Be.EqualTo(0);
        }

        [TestMethod]
        public void enumerableExtensions_alternateWith_using_single_item_list_should_return_single_item_without_the_alternate()
        {
            var list = new List<string>()
            {
                "Foo"
            };

            var actual = Linq.EnumerableExtensions.AlternateWith(list, "Bar");

            actual.Count().Should().Be.EqualTo(list.Count);
            actual.ElementAt(0).Should().Be.EqualTo(list[0]);
        }

        [TestMethod]
        public void enumerableExtensions_alternateWith_using_null_list_should_throw_ArgumentNullException()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var x = Linq.EnumerableExtensions.AlternateWith<object>(null, null);
                x.ToList();
            });
        }

        [TestMethod]
        public void enumerableExtensions_isChildOfAny_using_valid_tree_and_direct_child_should_be_true()
        {
            var root = new Node()
            {
                Id = "0"
            };

            var node = new Node()
            {
                Id = "0.1"
            };

            root.Nodes.Add(node);

            var tree = new List<Node>()
            {
                root
            };

            var actual = Linq.EnumerableExtensions.IsChildOfAny(node, tree, n => n.Parent);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void enumerableExtensions_shouffle_should_return_source_list_in_a_different_order()
        {
            var source = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var actual = source.Shouffle();

            actual.Should().Have.SameValuesAs(source);
            actual.Should().Not.Have.SameSequenceAs(source);
        }




        [TestMethod]
        public void enumerableExtensions_asReadOnly_should_return_new_list_when_source_is_list()
        {
            var source = new List<string>();
            var actual = Linq.EnumerableExtensions.AsReadOnly(source);

            Assert.AreNotEqual(source, actual);
            Assert.AreNotSame(source, actual);
        }

        [TestMethod]
        public void enumerableExtensions_asReadOnly_should_return_new_list_when_source_is_net_read_only_collection()
        {
            var source = new System.Collections.ObjectModel.ReadOnlyCollection<string>(new[] { "" });
            var actual = Linq.EnumerableExtensions.AsReadOnly(source);

            Assert.AreNotEqual(source, actual);
            Assert.AreNotSame(source, actual);
        }

        [TestMethod]
        public void enumerableExtensions_asReadOnly_should_return_new_list_when_source_is_dictionary_keys()
        {
            var source = new Dictionary<object, object>();
            var actual = Linq.EnumerableExtensions.AsReadOnly(source.Keys);

            Assert.AreNotEqual(source.Keys, actual);
            Assert.AreNotSame(source.Keys, actual);
        }

        [TestMethod]
        public void enumerableExtensions_asReadOnly_should_return_new_list_when_source_is_dictionary_values()
        {
            var source = new Dictionary<object, object>();
            var actual = Linq.EnumerableExtensions.AsReadOnly(source.Values);

            Assert.AreNotEqual(source.Values, actual);
            Assert.AreNotSame(source.Values, actual);
        }
    }

    class Node
    {
        public Node()
        {
            Nodes = new NodeList(this);
        }

        public string Id { get; set; }

        public IList<Node> Nodes { get; private set; }

        public Node Parent { get; set; }
    }

    class NodeList : System.Collections.ObjectModel.Collection<Node>
    {
        readonly Node parent;

        public NodeList(Node parent)
        {
            this.parent = parent;
        }

        protected override void InsertItem(int index, Node item)
        {
            base.InsertItem(index, item);
            item.Parent = parent;
        }
    }
}
