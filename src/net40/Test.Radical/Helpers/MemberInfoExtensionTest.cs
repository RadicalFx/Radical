namespace Test.Radical.Helpers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Reflection;
    using Topics.Radical.Reflection;

    [TestClass()]
    public class MemberInfoExtensionTest
    {
        [AttributeUsage( AttributeTargets.Class )]
        private class MyTestAttribute : Attribute
        {

        }

        [MyTest]
        private class MyBaseTestClass
        {

        }

        private sealed class MyDerivedTestClass : MyBaseTestClass
        {

        }

        private sealed class MyTestClassWithoutAttributes
        {

        }

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
        public void IsAttributeDefined_memberInfo()
        {
            Type t = typeof( MyBaseTestClass );
            Boolean actual = MemberInfoExtensions.IsAttributeDefined<MyTestAttribute>( t );

            Assert.IsTrue( actual );
        }

        [TestMethod()]
        public void IsAttributeDefined_memberInfo_using_base_attribute()
        {
            Type t = typeof( MyBaseTestClass );
            Boolean actual = MemberInfoExtensions.IsAttributeDefined<Attribute>( t );

            Assert.IsTrue( actual );
        }

        [TestMethod()]
        public void IsAttributeDefined_memberInfo_inherit_false()
        {
            Type t = typeof( MyDerivedTestClass );
            Boolean actual = MemberInfoExtensions.IsAttributeDefined<MyTestAttribute>( t, false );

            Assert.IsFalse( actual );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void IsAttributeDefined_null_memberInfo()
        {
            MemberInfoExtensions.IsAttributeDefined<MyTestAttribute>( null );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void IsAttributeDefined_null_memberInfo_false()
        {
            MemberInfoExtensions.IsAttributeDefined<MyTestAttribute>( null, false );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void IsAttributeDefined_null_memberInfo_true()
        {
            MemberInfoExtensions.IsAttributeDefined<MyTestAttribute>( null, true );
        }

        [TestMethod()]
        public void GetAttribute()
        {
            Type t = typeof( MyBaseTestClass );
            MyTestAttribute actual = MemberInfoExtensions.GetAttribute<MyTestAttribute>( t );

            Assert.IsNotNull( actual );
        }

        [TestMethod()]
        public void GetAttribute_memeberInfo_false()
        {
            Type t = typeof( MyBaseTestClass );
            MyTestAttribute actual = MemberInfoExtensions.GetAttribute<MyTestAttribute>( t, false );

            Assert.IsNotNull( actual );
        }

        [TestMethod()]
        public void GetAttribute_memeberInfo_true()
        {
            Type t = typeof( MyBaseTestClass );
            MyTestAttribute actual = MemberInfoExtensions.GetAttribute<MyTestAttribute>( t, true );

            Assert.IsNotNull( actual );
        }

        [TestMethod()]
        public void GetAttribute_on_inherited()
        {
            Type t = typeof( MyDerivedTestClass );
            MyTestAttribute actual = MemberInfoExtensions.GetAttribute<MyTestAttribute>( t );

            Assert.IsNotNull( actual );
        }

        [TestMethod()]
        public void GetAttribute_on_inherited_false()
        {
            Type t = typeof( MyDerivedTestClass );
            MyTestAttribute actual = MemberInfoExtensions.GetAttribute<MyTestAttribute>( t, false );

            Assert.IsNull( actual );
        }

        [TestMethod()]
        public void GetAttribute_on_inherited_true()
        {
            Type t = typeof( MyDerivedTestClass );
            MyTestAttribute actual = MemberInfoExtensions.GetAttribute<MyTestAttribute>( t, true );

            Assert.IsNotNull( actual );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void GetAttribute_null_memberInfo_true()
        {
            MemberInfoExtensions.GetAttribute<MyTestAttribute>( null, true );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void GetAttribute_null_memberInfo_false()
        {
            MemberInfoExtensions.GetAttribute<MyTestAttribute>( null, true );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void GetAttribute_null_memberInfo()
        {
            MemberInfoExtensions.GetAttribute<MyTestAttribute>( null );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void GetAttributes_null_memberInfo()
        {
            MemberInfoExtensions.GetAttributes<MyTestAttribute>( null );
        }

        [TestMethod()]
        public void GetAttributes_memberInfo()
        {
            Type t = typeof( MyBaseTestClass );
            MyTestAttribute[] actual = MemberInfoExtensions.GetAttributes<MyTestAttribute>( t );

            Assert.IsNotNull( actual );
            Assert.AreEqual<Int32>( 1, actual.Length );
            Assert.IsNotNull( actual[ 0 ] );
        }

        [TestMethod()]
        public void GetAttributes_memberInfo_inherit()
        {
            Type t = typeof( MyDerivedTestClass );
            MyTestAttribute[] actual = MemberInfoExtensions.GetAttributes<MyTestAttribute>( t );

            Assert.IsNotNull( actual );
            Assert.AreEqual<Int32>( 1, actual.Length );
            Assert.IsNotNull( actual[ 0 ] );
        }

        [TestMethod()]
        public void GetAttributes_memberInfo_inherit_false()
        {
            Type t = typeof( MyDerivedTestClass );
            MyTestAttribute[] actual = MemberInfoExtensions.GetAttributes<MyTestAttribute>( t, false );

            Assert.IsNotNull( actual );
            Assert.AreEqual<Int32>( 0, actual.Length );
        }

        [TestMethod()]
        public void GetAttributes_memberInfo_inherit_true()
        {
            Type t = typeof( MyDerivedTestClass );
            MyTestAttribute[] actual = MemberInfoExtensions.GetAttributes<MyTestAttribute>( t, true );

            Assert.IsNotNull( actual );
            Assert.AreEqual<Int32>( 1, actual.Length );
            Assert.IsNotNull( actual[ 0 ] );
        }
    }
}
