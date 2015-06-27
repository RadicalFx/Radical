using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Topics.Radical.Windows.CommandBuilders;

namespace Test.Radical.Windows
{
    [TestClass]
    public class DelegateCommandBuilderTests
    {
        class TestDataContext
        {
            public TestDataContext()
            {
                this.MyProperty = new NestedPropertyClass();
            }

            public void DoSomething() { }

            public void EndsWithCommand() { }

            public NestedPropertyClass MyProperty { get; set; }
        }

        class NestedPropertyClass 
        {
            public void DoSomethingElse() { }

            public void OtherThatEndsWithCommand() { }
        }

        [TestMethod]
        [TestCategory( "DelegateCommandBuilder" )]
        public void DelegateCommandBuilder_using_simple_method_should_generate_CommandData() 
        {
            var sut = new DelegateCommandBuilder();

            CommandData cd;
            var succeeded = sut.TryGenerateCommandData( new PropertyPath( "DoSomething" ), new TestDataContext(), out cd );

            Assert.IsTrue( succeeded );
            Assert.IsTrue( cd.FastDelegate != null );
        }

        [TestMethod]
        [TestCategory( "DelegateCommandBuilder" )]
        public void DelegateCommandBuilder_using_simple_method_on_nested_property_should_generate_CommandData()
        {
            var sut = new DelegateCommandBuilder();

            CommandData cd;
            var succeeded = sut.TryGenerateCommandData( new PropertyPath( "MyProperty.DoSomethingElse" ), new TestDataContext(), out cd );

            Assert.IsTrue( succeeded );
            Assert.IsTrue( cd.FastDelegate != null );
        }

        [TestMethod]
        [TestCategory( "DelegateCommandBuilder" )]
        public void DelegateCommandBuilder_using_invalid_path_should_not_fail()
        {
            var sut = new DelegateCommandBuilder();

            CommandData cd;
            var succeeded = sut.TryGenerateCommandData( new PropertyPath( "ThisIsInvalid" ), new TestDataContext(), out cd );

            Assert.IsFalse( succeeded );
            Assert.IsNull( cd );
        }

        [TestMethod]
        [TestCategory( "DelegateCommandBuilder" )]
        public void DelegateCommandBuilder_using_invalid_nested_path_should_not_fail()
        {
            var sut = new DelegateCommandBuilder();

            CommandData cd;
            var succeeded = sut.TryGenerateCommandData( new PropertyPath( "MyProperty.ThisIsInvalid" ), new TestDataContext(), out cd );

            Assert.IsFalse( succeeded );
            Assert.IsNull( cd );
        }

        [TestMethod]
        [TestCategory( "DelegateCommandBuilder" )]
        public void DelegateCommandBuilder_using_invalid_full_nested_path_should_not_fail()
        {
            var sut = new DelegateCommandBuilder();

            CommandData cd;
            var succeeded = sut.TryGenerateCommandData( new PropertyPath( "Invalid.ThisIsInvalid" ), new TestDataContext(), out cd );

            Assert.IsFalse( succeeded );
            Assert.IsNull( cd );
        }

        [TestMethod]
        [TestCategory( "DelegateCommandBuilder" )]
        public void DelegateCommandBuilder_using_PropertyPath_that_ends_with_Command_should_succeed()
        {
            var sut = new DelegateCommandBuilder();

            CommandData cd;
            var succeeded = sut.TryGenerateCommandData( new PropertyPath( "EndsWithCommand" ), new TestDataContext(), out cd );

            Assert.IsTrue( succeeded );
            Assert.IsTrue( cd.FastDelegate != null );
        }

        [TestMethod]
        [TestCategory( "DelegateCommandBuilder" )]
        public void DelegateCommandBuilder_using_nested_PropertyPath_that_ends_with_Command_should_succeed()
        {
            var sut = new DelegateCommandBuilder();

            CommandData cd;
            var succeeded = sut.TryGenerateCommandData( new PropertyPath( "MyProperty.OtherThatEndsWithCommand" ), new TestDataContext(), out cd );

            Assert.IsTrue( succeeded );
            Assert.IsTrue( cd.FastDelegate != null );
        }
    }
}
