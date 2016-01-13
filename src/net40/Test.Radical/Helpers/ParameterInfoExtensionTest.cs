namespace Test.Radical.Helpers
{
    using System;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Topics.Radical.Reflection;

    [TestClass()]
    public class ParameterInfoExtensionTest
    {
        [AttributeUsage( AttributeTargets.Parameter )]
        private class MyTestAttribute : Attribute
        {

        }

        [AttributeUsage( AttributeTargets.Parameter )]
        private class MyInheritedTestAttribute : MyTestAttribute
        {

        }

        private class MyTestClass
        {
            public void Foo( [MyTest]object obj )
            {
 
            }

            public void FooWithInherited( [MyInheritedTest]object obj )
            {

            }

            public void FooWithNoParameters( object obj )
            {

            }
        }

        static ParameterInfo GetParameter()
        {
            return typeof( MyTestClass ).GetMethod( "Foo" ).GetParameters()[ 0 ];
        }

        static ParameterInfo GetParameterWithInherited()
        {
            return typeof( MyTestClass ).GetMethod( "FooWithInherited" ).GetParameters()[ 0 ];
        }

        static ParameterInfo GetParameterWithNoAttributes()
        {
            return typeof( MyTestClass ).GetMethod( "FooWithNoParameters" ).GetParameters()[ 0 ];
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
        public void IsAttributeDefined_parameterInfo()
        {
            ParameterInfo pi = GetParameter();
            Boolean actual = ParameterInfoExtension.IsAttributeDefined<MyTestAttribute>( pi );

            Assert.IsTrue( actual );
        }

        [TestMethod()]
        public void IsAttributeDefined_parameterInfo_using_inherited_attribute_but_searching_for_base_attribute_type()
        {
            ParameterInfo pi = GetParameterWithInherited();
            Boolean actual = ParameterInfoExtension.IsAttributeDefined<MyTestAttribute>( pi );

            Assert.IsTrue( actual );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void IsAttributeDefined_null_parameterInfo()
        {
            ParameterInfoExtension.IsAttributeDefined<MyTestAttribute>( null );
        }

        [TestMethod()]
        public void GetAttribute()
        {
            ParameterInfo pi = GetParameter();
            MyTestAttribute actual = ParameterInfoExtension.GetAttribute<MyTestAttribute>( pi );

            Assert.IsNotNull( actual );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void GetAttribute_null_parameterInfo()
        {
            ParameterInfoExtension.GetAttribute<MyTestAttribute>( null );
        }

        [TestMethod()]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void GetAttributes_null_parameterInfo()
        {
            ParameterInfoExtension.GetAttributes<MyTestAttribute>( null );
        }

        [TestMethod()]
        public void GetAttributes_on_parameterInfo_with_no_attributes()
        {
            ParameterInfo pi = GetParameterWithNoAttributes();
            MyTestAttribute[] data= ParameterInfoExtension.GetAttributes<MyTestAttribute>( pi );

            Assert.IsNotNull( data );
            Assert.IsTrue( data.Length == 0 );
        }

        [TestMethod()]
        public void GetAttributes_parameterInfo()
        {
            ParameterInfo pi = GetParameter();
            MyTestAttribute[] actual = ParameterInfoExtension.GetAttributes<MyTestAttribute>( pi );

            Assert.IsNotNull( actual );
            Assert.AreEqual<Int32>( 1, actual.Length );
            Assert.IsNotNull( actual[ 0 ] );
        }
    }
}
