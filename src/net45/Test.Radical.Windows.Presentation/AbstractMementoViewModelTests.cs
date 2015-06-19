using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.ChangeTracking;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.ComponentModel.Validation;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Services.Validation;

namespace Test.Radical.Windows.Presentation
{
    [TestClass]
    public class AbstractMementoViewModelTests
    {
        abstract class TestViewModel : AbstractMementoViewModel
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

            public String Test_ValidateProperty( String propertyName )
            {
                return this.Test_ValidateProperty( propertyName, ValidationBehavior.Default );
            }

            public String Test_ValidateProperty( String propertyName, ValidationBehavior behavior )
            {
                return this.ValidateProperty( propertyName, behavior );
            }

            public void Test_RaisePropertyChanged<T>( Expression<Func<T>> property ) 
            {
                this.OnPropertyChanged( property );
            }

            public void Test_SetInitialPropertyValue<T>( Expression<Func<T>> property, T value ) 
            {
                this.SetInitialPropertyValue( property, value );
            }
        }

        class SampleTestViewModel : TestViewModel
        {
            [Required( AllowEmptyStrings = false )]
            [StringLength( 10, MinimumLength = 5 )]
            public String NotNullNotEmpty
            {
                get { return this.GetPropertyValue( () => this.NotNullNotEmpty ); }
                set { this.SetPropertyValue( () => this.NotNullNotEmpty, value ); }
            }

            public String Another
            {
                get { return this.GetPropertyValue( () => this.Another ); }
                set { this.SetPropertyValue( () => this.Another, value ); }
            }

            public String AnotherOne
            {
                get { return this.GetPropertyValue( () => this.AnotherOne ); }
                set { this.SetPropertyValue( () => this.AnotherOne, value ); }
            }

            public String OnceMore
            {
                get { return this.GetPropertyValue( () => this.OnceMore ); }
                set { this.SetPropertyValue( () => this.OnceMore, value ); }
            }
        }

        class SampleTestViewModelWithValidationCallback : SampleTestViewModel, IRequireValidationCallback<SampleTestViewModelWithValidationCallback>
        {
            public Action<ValidationContext<SampleTestViewModelWithValidationCallback>> Test_OnValidate { get; set; }

            public void OnValidate( ValidationContext<SampleTestViewModelWithValidationCallback> context )
            {
                this.Test_OnValidate( context );
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
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_with_no_validation_service_always_validates_to_true()
        {
            var sut = new SampleTestViewModel();
            var isValid = sut.Validate();

            Assert.IsTrue( isValid );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_with_no_validation_service_is_always_validat()
        {
            var sut = new SampleTestViewModel();
            var isValid = sut.IsValid;

            Assert.IsTrue( isValid );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_with_no_validation_service_has_no_errors()
        {
            var sut = new SampleTestViewModel();
            sut.Validate();
            var errors = sut.ValidationErrors;

            Assert.IsTrue( errors.Count == 0 );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_with_validation_service_should_generate_expected_errors()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ) );
            sut.Validate();
            var errors = sut.ValidationErrors;

            Assert.IsTrue( errors.Count == 1 );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_as_IDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ) );

            var error = sut[ "NotNullNotEmpty" ];

            Assert.IsNull( error );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_as_IDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid_even_if_called_multiple_times()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ) );

            var error = sut[ "NotNullNotEmpty" ];
            error = sut[ "NotNullNotEmpty" ];
            error = sut[ "NotNullNotEmpty" ];

            Assert.IsNull( error );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_as_INotifyDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ) );

            var errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();

            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_as_INotifyDataErrorInfo_with_validation_service_invalid_property_not_validated_is_valid_even_if_called_multiple_times()
        {
            var sut = new SampleTestViewModel();
            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ) );

            var errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();
            errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();
            errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();

            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_INotifyDataErrorInfo_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsINotifyDataErrorInfo();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_IDataErrorInfo_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsIDataErrorInfo();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_ICanBeValidated_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsICanBeValidated();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_IRequireValidation_IsValidationEnabled_should_be_true()
        {
            var sut = new ImplementsIRequireValidation();
            Assert.IsTrue( sut.Test_IsValidationEnabled );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_PropertyChanged_is_raised_GetErrors_should_contain_expected_errors()
        {
            IEnumerable<Object> errors = null;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                errors = sut.GetErrors( "NotNullNotEmpty" ).OfType<Object>();
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ) );
            sut.NotNullNotEmpty = "";

            Assert.IsNotNull( errors );
            Assert.AreEqual( 0, errors.Count() );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_PropertyChanged_is_raised_Error_indexer_should_contain_expected_errors()
        {
            String errors = null;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                errors = sut[ "NotNullNotEmpty" ];
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsFalse( String.IsNullOrWhiteSpace( errors ) );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_PropertyChanged_is_raised_IsValid_should_be_false()
        {
            Boolean isValid = true;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                isValid = sut.IsValid;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsFalse( isValid );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_PropertyChanged_is_raised_ValidationErrors_should_contain_expected_errors()
        {
            ObservableCollection<ValidationError> errors = null;

            var sut = new SampleTestViewModel();
            sut.PropertyChanged += ( s, e ) =>
            {
                errors = sut.ValidationErrors;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsNotNull( errors );
            Assert.AreEqual( 1, errors.Count );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_when_validation_status_changes_ErrorsChanged_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.ErrorsChanged += ( s, e ) =>
            {
                raised = true;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_when_validation_status_changes_Validated_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.Validated += ( s, e ) =>
            {
                raised = true;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_when_validation_is_reset_ErrorsChanged_should_be_raised()
        {
            bool raised = false;

            var sut = new SampleTestViewModel();
            sut.ErrorsChanged += ( s, e ) =>
            {
                raised = true;
            };

            sut.ValidateUsing(
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.ResetValidation();

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_when_validation_status_changes_PropertyChanged_for_IsValid_property_should_be_raised()
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
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_when_validation_status_changes_PropertyChanged_for_HasErrors_property_should_be_raised()
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
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );
            sut.NotNullNotEmpty = "";

            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_when_validating_entire_entity_PropertyChanged_for_HasErrors_property_should_be_raised()
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
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );

            var isValid = sut.Validate();

            Assert.IsFalse( isValid );
            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_when_validating_entire_entity_PropertyChanged_for_IsValid_property_should_be_raised()
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
                new DataAnnotationValidationService<SampleTestViewModel>( sut ),
                forceIsValidationEnabledTo: true );

            var isValid = sut.Validate();

            Assert.IsFalse( isValid );
            Assert.IsTrue( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractViewModel_When_merge_errors_changes_it_should_not_fail()
        {
            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );

            sut.Validate();
            svc.MergeValidationErrors = !svc.MergeValidationErrors;
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_When_ValidateProperty_PropertyChanged_event_should_not_be_raised()
        {
            List<String> raised = new List<string>();
            var propName = "NotNullNotEmpty";

            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.PropertyChanged += ( s, e ) => raised.Add( e.PropertyName );

            sut.Test_ValidateProperty( propName );

            Assert.IsFalse( raised.Contains( propName ) );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_When_ValidateProperty_with_Trigger_berhavior_PropertyChanged_event_should_be_raised()
        {
            List<String> raised = new List<string>();
            var propName = "NotNullNotEmpty";

            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.PropertyChanged += ( s, e ) => raised.Add( e.PropertyName );

            sut.Test_ValidateProperty( propName, ValidationBehavior.TriggerValidationErrorsOnFailure );

            Assert.IsTrue( raised.Contains( propName ) );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_When_ValidateProperty_with_Trigger_berhavior_ErrorsChanged_event_should_not_be_raised()
        {
            List<String> raised = new List<string>();
            var propName = "NotNullNotEmpty";

            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.ErrorsChanged += ( s, e ) => raised.Add( e.PropertyName );

            sut.Test_ValidateProperty( propName );

            Assert.IsFalse( raised.Contains( propName ) );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_When_ValidateProperty_with_Trigger_berhavior_ErrorsChanged_event_should_be_raised()
        {
            List<String> raised = new List<string>();
            var propName = "NotNullNotEmpty";

            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.ErrorsChanged += ( s, e ) => raised.Add( e.PropertyName );

            sut.Test_ValidateProperty( propName, ValidationBehavior.TriggerValidationErrorsOnFailure );

            Assert.IsTrue( raised.Contains( propName ) );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_When_ValidateProperty_with_Trigger_berhavior_ErrorsChanged_event_should_be_raised_if_the_status_of_errors_changes()
        {
            List<String> raised = new List<string>();
            var propName = "NotNullNotEmpty";

            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.ErrorsChanged += ( s, e ) => raised.Add( e.PropertyName );

            sut.Test_ValidateProperty( propName, ValidationBehavior.TriggerValidationErrorsOnFailure );

            using( svc.SuspendValidation() ) //so that we can change a property without triggering the validation process
            {
                sut.NotNullNotEmpty = "qwertyqwerty";
            }

            sut.Test_ValidateProperty( propName, ValidationBehavior.TriggerValidationErrorsOnFailure );

            Assert.IsTrue( raised.Count( p => p == propName ) == 2 );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_When_ValidateProperty_with_Trigger_berhavior_PropertyChanged_event_should_be_raised_if_the_status_of_errors_changes()
        {
            List<String> raised = new List<string>();
            var propName = "NotNullNotEmpty";

            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.PropertyChanged += ( s, e ) => raised.Add( e.PropertyName );

            sut.Test_ValidateProperty( propName, ValidationBehavior.TriggerValidationErrorsOnFailure );

            using( svc.SuspendValidation() ) //so that we can change a property without triggering the validation process
            {
                sut.NotNullNotEmpty = "qwertyqwerty"; //this raises 1 PropertyChanged
            }

            sut.Test_ValidateProperty( propName, ValidationBehavior.TriggerValidationErrorsOnFailure );

            Assert.IsTrue( raised.Count( p => p == propName ) == 3 );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_When_ValidateProperty_and_Validation_is_suspended_Validated_event_should_not_be_raised()
        {
            Boolean raised = false;
            var propName = "NotNullNotEmpty";

            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.Validated += ( s, e ) => raised = true;

            using( svc.SuspendValidation() ) //so that we can change a property without triggering the validation process
            {
                sut.Test_ValidateProperty( propName, ValidationBehavior.TriggerValidationErrorsOnFailure );
            }

            Assert.IsFalse( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" )]
        public void AbstractMementoViewModel_When_Validate_and_Validation_is_suspended_Validated_event_should_not_be_raised()
        {
            Boolean raised = false;
            
            var sut = new SampleTestViewModel();
            var svc = new DataAnnotationValidationService<SampleTestViewModel>( sut );
            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.Validated += ( s, e ) => raised = true;

            using( svc.SuspendValidation() ) //so that we can change a property without triggering the validation process
            {
                sut.Validate();
            }

            Assert.IsFalse( raised );
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" ), TestCategory( "Issue#176" )]
        public void AbstractMementoViewModel_it_should_be_possible_to_change_a_validatable_property_at_custom_validation_time()
        {
            var sut = new SampleTestViewModelWithValidationCallback();
            var svc = DataAnnotationValidationService.CreateFor( sut );

            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.Test_OnValidate = ctx => 
            {
                sut.AnotherOne = "fail";
            };

            sut.NotNullNotEmpty = "a value";
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" ), TestCategory( "Issue#176" )]
        public void AbstractMementoViewModel_it_should_be_possible_to_change_a_validatable_property_in_a_custom_validation_rule()
        {
            var sut = new SampleTestViewModel();
            var svc = DataAnnotationValidationService.CreateFor( sut );
            svc.AddRule(
                property: () => sut.NotNullNotEmpty,
                error: ctx => "error",
                rule: ctx =>
                {
                    sut.AnotherOne = "fail";

                    return true;
                } );

            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );

            sut.NotNullNotEmpty = "a value";
        }

        [TestMethod]
        [TestCategory( "AbstractMementoViewModel" ), TestCategory( "Validation" ), TestCategory( "Issue#177" )]
        public void AbstractMementoViewModel_it_should_be_possible_to_change_a_validatable_property_in_the_validated_event()
        {
            var sut = new SampleTestViewModel();
            var svc = DataAnnotationValidationService.CreateFor( sut );

            sut.ValidateUsing( svc, forceIsValidationEnabledTo: true );
            sut.Validated += ( s, e ) =>
            {
                sut.AnotherOne = "fail";
            };

            sut.NotNullNotEmpty = "a value";
        }
    }
}