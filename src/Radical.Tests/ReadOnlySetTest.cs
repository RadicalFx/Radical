namespace Radical.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.Collections;

    [TestClass()]
    public class ReadOnlySetTest
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod()]
        public void ReadOnlySet_iEnumerable_ctor()
        {
            List<int> source = new List<int>();
            ReadOnlyCollection<int> actual = new ReadOnlyCollection<int>(source);

            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadOnlySet_ctor_null_source()
        {
            ReadOnlyCollection<int> actual = new ReadOnlyCollection<int>(null);
        }

        [TestMethod()]
        public void ReadOnlySet_count()
        {
            List<int> source = new List<int>() { 0, 1, 2, 3, 4 };
            ReadOnlyCollection<int> actual = new ReadOnlyCollection<int>(source);

            Assert.AreEqual<int>(source.Count, actual.Count);
        }

        [TestMethod()]
        public void ReadOnlySet_syncRoot()
        {
            List<int> source = new List<int>();
            ReadOnlyCollection<int> actual = new ReadOnlyCollection<int>(source);

            Assert.IsNotNull(actual.SyncRoot);
        }

        [TestMethod()]
        public void ReadOnlySet_isSynchronized()
        {
            List<int> source = new List<int>();
            ReadOnlyCollection<int> actual = new ReadOnlyCollection<int>(source);

            Assert.IsFalse(actual.IsSynchronized);
        }

        [TestMethod()]
        public void ReadOnlySet_getEnumerator()
        {
            List<int> source = new List<int>();
            ReadOnlyCollection<int> actual = new ReadOnlyCollection<int>(source);

            Assert.IsNotNull(actual.GetEnumerator());
        }

        [TestMethod()]
        public void ReadOnlySet_getEnumeratorOfT()
        {
            List<int> source = new List<int>();
            ReadOnlyCollection<int> actual = new ReadOnlyCollection<int>(source);

            Assert.IsNotNull(((IEnumerable<int>)actual).GetEnumerator());
        }

        [TestMethod()]
        public void ReadOnlySet_copyTo()
        {
            List<int> source = new List<int>() { 1, 2, 1, 3 };
            ReadOnlyCollection<int> actual = new ReadOnlyCollection<int>(source);

            int[] array = new int[source.Count];
            actual.CopyTo(array, 0);

            for (int i = 0; i < source.Count; i++)
            {
                Assert.AreEqual<int>(source[i], array[i]);
            }
        }
    }
}