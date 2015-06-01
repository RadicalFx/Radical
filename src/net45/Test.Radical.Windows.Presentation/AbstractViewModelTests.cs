using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;
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
            IValidationService _validationService;
            Boolean? _forceIsValidationEnabledTo;
            internal void ValidateUsing( IValidationService validationService, Boolean? forceIsValidationEnabledTo = null )
            {
                this._validationService = validationService;
                this._forceIsValidationEnabledTo = forceIsValidationEnabledTo;
            }

            protected override bool IsValidationEnabled
            {
                get
                {
                    if( this._forceIsValidationEnabledTo.HasValue )
                    {
                        return this._forceIsValidationEnabledTo.Value;
                    }

                    return base.IsValidationEnabled;
                }
            }

            protected override IValidationService GetValidationService()
            {
                if( this._validationService != null )
                {
                    return this._validationService;
                }

                return base.GetValidationService();
            }

            public Boolean Test_IsValidationEnabled { get { return this.IsValidationEnabled; } }
        }

        class SampleTestViewModel : TestViewModel
        {
            [Required( AllowEmptyStrings = false )]
            public String NotNullNotEmpty
            {
                get { return this.GetPropertyValue( () => this.NotNullNotEmpty ); }
                set { this.SetPropertyValue( () => this.NotNullNotEmpty, value ); }
            }
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
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_with_no_validation_service_always_validates_to_true()
        {
            var sut = new SampleTestViewModel();
            var isValid = sut.Validate();

            Assert.IsTrue( isValid );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_with_no_validation_service_is_always_validat()
        {
            var sut = new SampleTestViewModel();
            var isValid = sut.IsValid;

            Assert.IsTrue( isValid );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_with_no_validation_service_has_no_errors()
        {
            var sut = new SampleTestViewModel();
            sut.Validate();
            var errors = sut.ValidationErrors;

            Assert.IsTrue( errors.Count == 0 );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_with_validation_service_should_generate_expected_errors()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ) );
            sut.Validate();
            var errors = sut.ValidationErrors;

            Assert.IsTrue( errors.Count == 1 );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_as_IDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ) );

            var error = sut[ "NotNullNotEmpty" ];

            Assert.IsNull( error );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_as_IDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid_even_if_called_multiple_times()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ) );

            var error = sut[ "NotNullNotEmpty" ];
            error = sut[ "NotNullNotEmpty" ];
            error = sut[ "NotNullNotEmpty" ];

            Assert.IsNull( error );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_as_INotifyDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ) );

            var errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();

            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_as_INotifyDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid_even_if_called_multiple_times()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ) );

            var errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();
            errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();
            errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();

            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_INotifyDataErrorInfo_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsINotifyDataErrorInfo();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_IDataErrorInfo_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsIDataErrorInfo();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_ICanBeValidated_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsICanBeValidated();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_IRequireValidation_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsIRequireValidation();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_PropertyChanged_is_raised_GetErrors_should_contain_expected_errors()
        {
            IEnumerable<Object> errors = null;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ) );
            sut.NotNullNotEmpty = "";

            Assert.IsNotNull( errors );
            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_PropertyChanged_is_raised_Error_indexer_should_contain_expected_errors()
        {
            String errors = null;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                errors = sut[ "NotNullNotEmpty" ];
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsFalse( String.IsNullOrWhiteSpace( errors ) );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_PropertyChanged_is_raised_IsValid_should_be_false()
        {
            Boolean isValid = true;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                isValid = sut.IsValid;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsFalse( isValid );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_PropertyChanged_is_raised_ValidationErrors_should_contain_expected_errors()
        {
            ObservableCollection<ValidationError> errors = null;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                errors = sut.ValidationErrors;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsNotNull( errors );
            Assert.AreEqual( 1, errors.Count );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_when_validation_status_changes_ErrorsChanged_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.ErrorsChanged += ( s, e ) =>
            {
                raised = true;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_when_validation_status_changes_Validated_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.Validated += ( s, e ) =>
            {
                raised = true;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_when_validation_is_reset_ErrorsChanged_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.ErrorsChanged += ( s, e ) =>
            {
                raised = true;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.ResetValidation();

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_when_validation_status_changes_PropertyChanged_for_IsValid_property_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                if( e.PropertyName == "IsValid" )
                {
                    raised = true;
                }
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_when_validation_status_changes_PropertyChanged_for_HasErrors_property_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                if( e.PropertyName == "HasErrors" )
                {
                    raised = true;
                }
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_when_validating_entire_entity_PropertyChanged_for_HasErrors_property_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                if( e.PropertyName == "HasErrors" )
                {
                    raised = true;
                }
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            
            var isValid = sut.Validate();

            Assert.IsFalse( isValid );
            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_when_validating_entire_entity_PropertyChanged_for_IsValid_property_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                if( e.PropertyName == "IsValid" )
                {
                    raised = true;
                }
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<TestViewModel>( sut ),
                forceIsValidationEnabledTo: true );

            var isValid = sut.Validate();

            Assert.IsFalse( isValid );
            Assert.IsTrue( raised );
        }
    }
}
