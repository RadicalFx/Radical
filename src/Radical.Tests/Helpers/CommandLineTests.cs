namespace Radical.Tests.Helpers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpTestsEx;
    using Radical.Helpers;

    [TestClass]
    public class CommandLineTests
    {
        [TestMethod]
        public void commandLine_Contains_using_valid_args_should_return_true_ignoring_dash_arg_prefix()
        {
            var expected = true;

            var target = new CommandLine( new[] { "-d" } );
            bool actual = target.Contains( "d" );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void commandLine_Contains_using_valid_args_should_return_true_ignoring_slash_arg_prefix()
        {
            var expected = true;

            var target = new CommandLine( new[] { "/d" } );
            bool actual = target.Contains( "d" );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void commandLine_Contains_using_valid_args_should_return_true()
        {
            var expected = true;

            var target = new CommandLine( new[] { "d" } );
            bool actual = target.Contains( "d" );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void commandLine_Contains_using_valid_args_should_ignore_casing()
        {
            var expected = true;

            var target = new CommandLine( new[] { "D" } );
            bool actual = target.Contains( "d" );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void commandLine_Contains_using_invalid_args_should_return_false()
        {
            var expected = false;

            var target = new CommandLine( new[] { "-data" } );
            bool actual = target.Contains( "d" );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void commandLine_tryGetValue_using_valid_args_should_return_expected_value()
        {
            var expectedValue = 1000;

            var target = new CommandLine( new[] { "-d=1000" } );

            int value;
            bool actual = target.TryGetValue<int>( "d", out value );

            actual.Should().Be.True();
            value.Should().Be.EqualTo( expectedValue );
        }

        [TestMethod]
        public void commandLine_tryGetValue_using_empty_args_should_not_fail()
        {
            var expected = false;

            var target = new CommandLine( new string[ 0 ] );

            int v;
            bool actual = target.TryGetValue<int>( "xyz", out v );

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void commandLine_tryGetValue_using_nullable_as_expected_value_and_missing_parameter_should_return_null()
        {
            var target = new CommandLine( new string[ 0 ] );

            int? v;
            bool actual = target.TryGetValue<int?>( "xyz", out v );

            actual.Should().Be.False();
            v.HasValue.Should().Be.False();
        }

        [TestMethod]
        public void commandLine_tryGetValue_using_nullable_as_expected_value_and_valid_parameter_should_return_expected_value()
        {
            var expected = new Nullable<int>( 10 );
            var target = new CommandLine( new[] { "xyz=10" } );

            int? v;
            bool actual = target.TryGetValue<int?>( "xyz", out v );

            actual.Should().Be.True();
            v.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        public void commandLine_tryGetValue_using_invalid_format_input_value_should_not_fail()
        {
            var target = new CommandLine( new[] { "xyz=1_0" } );

            int v;
            bool actual = target.TryGetValue<int>( "xyz", out v );

            actual.Should().Be.False();
        }

        class Sample
        {
            [CommandLineArgument( "f", IsRequired = true )]
            public int FirstProperty { get; set; }

            [CommandLineArgument( "s" )]
            public string SecondProperty { get; set; }
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_as_using_valid_command_line_and_class_should_convert_command_line_to_class_instance()
        {
            var target = new CommandLine( new[] { "-f=1", "-s=foo" } );
            var instance = target.As<Sample>();

            instance.FirstProperty.Should().Be.EqualTo( 1 );
            instance.SecondProperty.Should().Be.EqualTo( "foo" );
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_as_using_valid_command_line_and_class_should_convert_command_line_to_class_instance_even_using_non_required_arguments()
        {
            var target = new CommandLine( new[] { "-f=1" } );
            var instance = target.As<Sample>();

            instance.FirstProperty.Should().Be.EqualTo( 1 );
            instance.SecondProperty.Should().Be.Null();
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        [ExpectedException( typeof( ArgumentException ) )]
        public void CommandLine_as_using_valid_command_line_and_class_should_not_convert_command_line_to_class_instance_using_missing_required_arguments()
        {
            var target = new CommandLine( new string[ 0 ] );
            var instance = target.As<Sample>();
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_as_using_valid_command_line_with_spaces_in_one_value_should_convert_command_line_to_class_instance()
        {
            var target = new CommandLine( new[] { "-f=1", "-s=\"bar foo\"" } );
            var instance = target.As<Sample>();

            instance.FirstProperty.Should().Be.EqualTo( 1 );
            instance.SecondProperty.Should().Be.EqualTo( "bar foo" );
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_asArguments_using_valid_source_should_generate_the_expected_arguments()
        {
            var source = new Sample() { FirstProperty = 100, SecondProperty = "bar" };
            var actual = CommandLine.AsArguments( source );

            actual.Should().Be.EqualTo( "-f=100 -s=bar" );
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_asArguments_using_valid_source_should_generate_the_expected_arguments_and_if_one_string_value_has_spaces_should_surround_value_with_quotes()
        {
            var source = new Sample() { FirstProperty = 100, SecondProperty = "foo bar" };
            var actual = CommandLine.AsArguments( source );

            actual.Should().Be.EqualTo( "-f=100 -s=\"foo bar\"" );
        }

        enum SampleEnum
        {
            Value
        }

        class SampleWithEnum
        {
            [CommandLineArgument( "s", IsRequired = true )]
            public SampleEnum Sample { get; set; }
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_as_using_valid_source_with_enum_should_generate_the_expected_arguments()
        {
            var target = new CommandLine( new[] { "-s=value" } );
            var actual = target.As<SampleWithEnum>();

            actual.Sample.Should().Be.EqualTo( SampleEnum.Value );
        }

        class SampleWithAliases
        {
            [CommandLineArgument( "Sample", Aliases = new[] { "s" }, IsRequired = true )]
            public string Sample { get; set; }
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_as_using_valid_source_with_alias_should_generate_the_expected_arguments()
        {
            var target = new CommandLine( new[] { "-s=foo" } );
            var actual = target.As<SampleWithAliases>();

            actual.Sample.Should().Be.EqualTo( "foo" );
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_using_valid_source_with_equals_in_the_parameter_value_should_handle_value_as_expected()
        {
            var target = new CommandLine( new[] { "-s=foo=bar" } );
            
            string actual;
            target.TryGetValue( "s", out actual );

            actual.Should().Be.EqualTo( "foo=bar" );
        }

        class UserDefinition
        {
            [CommandLineArgument( "username", IsRequired = true, Aliases = new[] { "u" } )]
            public string Username { get; set; }

            [CommandLineArgument( "password", IsRequired = true, Aliases = new[] { "p" } )]
            public string Password { get; set; }

            [CommandLineArgument( "administrator", IsRequired = false, Aliases = new[] { "a", "admin" } )]
            public bool AsAdmin { get; set; }
        }

        [TestMethod]
        [TestCategory( "CommandLine" )]
        public void CommandLine_as_using_valid_source_with_parameter_without_value_should_correctly_transalte_to_bool()
        {
            var target = new CommandLine( new[] { "username=Mauro", "password=P@ssw0rd", "admin=True" } );
            var actual = target.As<UserDefinition>();

            actual.Username.Should().Be.EqualTo("Mauro");
            actual.Password.Should().Be.EqualTo( "P@ssw0rd" );
        }
    }
}
