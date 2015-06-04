using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Services.Validation;

namespace Test.Radical.Windows.Presentation
{
    [TestClass]
    public class AbstractValidationServiceTests
    {
        class TestValidationService : AbstractValidationService
        {
            //ValidationError[] errorsToReturnUnderTest;

            public TestValidationService( ValidationError[] errorsToReturnUnderTest )
            {
                //this.errorsToReturnUnderTest = errorsToReturnUnderTest;
                this.AddValidationErrors( errorsToReturnUnderTest );
            }

            protected override IEnumerable<ValidationError> OnValidate( string ruleSet )
            {
                var errors = new List<ValidationError>( this.ValidationErrors );

                return errors;
            }
        }

        [TestMethod]
        [TestCategory( "AbstractValidationService" ), TestCategory( "Validation" )]
        public void AbstractValidationService_validate_property_using_entity_with_non_valid_property_should_report_expected_errors()
        {
            var propName = "TestProperty";

            var expected = new[] { new ValidationError( propName, propName, new[] { "--fake--" } ) };
            var sut = new TestValidationService( expected );

            sut.Validate( propName );

            Assert.AreEqual( sut.ValidationErrors.Count(), expected.Length );
            Assert.AreEqual( sut.ValidationErrors.ElementAt( 0 ).Key, expected[ 0 ].Key );
        }

        [TestMethod]
        [TestCategory( "AbstractValidationService" ), TestCategory( "Validation" )]
        public void AbstractValidationService_StatusChanged_event_should_be_triggered_each_time_errors_list_changes_even_if_validity_does_not_change()
        {
            var actual = 0;

            var errors = new[]
            { 
                new ValidationError( "p1", "p1",new[] { "--fake--" } ),
                new ValidationError( "p2", "p2", new[] { "--fake--" } )
            };

            var sut = new TestValidationService( errors );
            sut.StatusChanged += ( s, e ) => actual += 1;

            sut.Validate( "p1" );
            sut.Validate( "p2" );
            sut.Validate( "p3" );

            Assert.AreEqual( 5, actual );
        }

        [TestMethod]
        [TestCategory( "AbstractValidationService" ), TestCategory( "Validation" )]
        public void AbstractValidationService_GetInvalidProperties_should_return_distinct_list()
        {
            var errors = new[]
            { 
                new ValidationError( "p1", "p1", new[] { "--fake 1--" } ),
                new ValidationError( "p1", "p1", new[] { "--fake 2--" } ),
                new ValidationError( "p2", "p2", new[] { "--fake--" } )
            };

            var sut = new TestValidationService( errors );
            var invalid = sut.GetInvalidProperties();

            Assert.AreEqual( 2, invalid.Count() );
        }

    }
}
