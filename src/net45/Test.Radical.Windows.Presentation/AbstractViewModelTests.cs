using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        abstract class TestViewModel : AbstractViewModel
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

            public Boolean Test_IsValidationEnabled { get { return false; } }
        }

        class SampleTestViewModel : TestViewModel
        {
            [Required( AllowEmptyStrings = false )]
            public String FirstName { get; set; }
        }

        class ImplementsIDataErrorInfo : TestViewModel, IDataErrorInfo
        {
            
        }

        class ImplementsICanBeValidated : TestViewModel, ICanBeValidated
        {

        }

        class ImplementsINotifyDataErrorInfo : TestViewModel, INotifyDataErrorInfo
        {

        }

        class ImplementsIRequireValidation : TestViewModel, IRequireValidation
        {

        }

        [TestMethod]
        public void ViewModel_with_no_validation_service_always_validates_to_true()
        {
            var sut = new SampleTestViewModel();
            var isValid = sut.Validate();

            Assert.IsTrue( isValid );
        }

        [TestMethod]
        public void ViewModel_with_no_validation_service_is_always_validat()
        {
            var sut = new SampleTestViewModel();
            var isValid = sut.IsValid;

            Assert.IsTrue( isValid );
        }

        [TestMethod]
        public void ViewModel_with_no_validation_service_has_no_errors()
        {
            var sut = new SampleTestViewModel();
            sut.Validate();
            var errors = sut.ValidationErrors;

            Assert.IsTrue( errors.Count == 0 );
        }

        [TestMethod]
        public void ViewModel_with_validation_service_should_generate_expected_errors()
        {
            var sut = new SampleTestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );
            sut.Validate();
            var errors = sut.ValidationErrors;

            Assert.IsTrue( errors.Count == 1 );
        }

        [TestMethod]
        public void ViewModel_as_IDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid()
        {
            var sut = new SampleTestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );

            var error = sut[ "FirstName" ];

            Assert.IsNull( error );
        }

        [TestMethod]
        public void ViewModel_as_IDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid_even_if_called_multiple_times()
        {
            var sut = new SampleTestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );

            var error = sut[ "FirstName" ];
            error = sut[ "FirstName" ];
            error = sut[ "FirstName" ];

            Assert.IsNull( error );
        }

        [TestMethod]
        public void ViewModel_as_INotifyDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid()
        {
            var sut = new SampleTestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );

            var errors = sut.GetErrors( "FirstName" ).OfType<Object>();

            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        public void ViewModel_as_INotifyDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid_even_if_called_multiple_times()
        {
            var sut = new SampleTestViewModel();
            sut.ValidationService = new DataAnnotationValidationService<TestViewModel>( sut );

            var errors = sut.GetErrors( "FirstName" ).OfType<Object>();
            errors = sut.GetErrors( "FirstName" ).OfType<Object>();
            errors = sut.GetErrors( "FirstName" ).OfType<Object>();

            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        public void ViewModel_INotifyDataErrorInfo_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsINotifyDataErrorInfo();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        public void ViewModel_IDataErrorInfo_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsIDataErrorInfo();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        public void ViewModel_ICanBeValidated_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsICanBeValidated();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        public void ViewModel_IRequireValidation_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsIRequireValidation();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }
    }
}
