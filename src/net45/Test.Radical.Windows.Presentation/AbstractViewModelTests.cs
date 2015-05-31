using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Services.Validation;

namespace Test.Radical.Windows.Presentation
{
    [TestClass]
    public class AbstractViewModelTests
    {
        class TestViewModel : AbstractViewModel
        {
            public IValidationService ValidationService { get; set; }

            protected override IValidationService GetValidationService()
            {
                if( this.ValidationService != null )
                {
                    return this.ValidationService;
                }

                return base.GetValidationService();
            }

            [Required( AllowEmptyStrings = false )]
            public String FirstName { get; set; }
        }

        [TestMethod]
        public void ViewModel_with_no_validation_service_always_valid()
        {
            var sut = new TestViewModel();
            var isValid = sut.Validate();

            Assert.IsTrue( isValid );
        }

        [TestMethod]
        public void ViewModel_with_no_validation_service_has_no_errors()
        {
            var sut = new TestViewModel();
            sut.Validate();
            var errors = sut.ValidationErrors;

            Assert.IsTrue( errors.Count == 0 );
        }

        [TestMethod]
        public void ViewModel_with_validation_service_should_generate_expected_errors()
        {
            var sut = new TestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );
            sut.Validate();
            var errors = sut.ValidationErrors;

            Assert.IsTrue( errors.Count == 1 );
        }

        [TestMethod]
        public void ViewModel_as_IDataErrorInfo_with_validation_service_should_ignore_first_validation_attempt()
        {
            var sut = new TestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );

            var error = sut[ "FirstName" ];

            Assert.IsNull( error );
        }

        [TestMethod]
        public void ViewModel_as_INotifyDataErrorInfo_with_validation_service_should_not_ignore_first_validation_attempt()
        {
            var sut = new TestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );
            sut.Validate();

            var errors = sut.GetErrors( "FirstName" ).OfType<Object>();

            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        public void ViewModel_as_IDataErrorInfo_with_validation_service_testing_same_property_twice_should_fail_validation()
        {
            var sut = new TestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );

            var error = sut[ "FirstName" ];
            error = sut[ "FirstName" ];

            Assert.IsNotNull( error );
        }

        [TestMethod]
        public void ViewModel_as_INotifyDataErrorInfo_with_validation_service_testing_same_property_twice_should_fail_validation()
        {
            var sut = new TestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );

            var errors = sut.GetErrors( "FirstName" ).OfType<Object>();
            errors = sut.GetErrors( "FirstName" ).OfType<Object>();
            errors = sut.GetErrors( "FirstName" ).OfType<Object>();

            Assert.AreEqual( 0, errors.Count() );
        }
    }
}
