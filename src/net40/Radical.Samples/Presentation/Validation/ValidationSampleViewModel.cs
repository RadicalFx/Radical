using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Validation;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Services.Validation;

namespace Topics.Radical.Presentation.Validation
{
	class AbstractValidationSampleViewModel : SampleViewModel
	{
		[Required( AllowEmptyStrings = false )]
		[DisplayName( "Testo" )]
		public String Text
		{
			get { return this.GetPropertyValue( () => this.Text ); }
			set { this.SetPropertyValue( () => this.Text, value ); }
		}
	}


	[Sample( Title = "DataAnnotation Validation", Category = Categories.Validation )]
	class ValidationSampleViewModel :
		AbstractValidationSampleViewModel,
		ICanBeValidated,
		IRequireValidationCallback<ValidationSampleViewModel>
	{
		public ValidationSampleViewModel()
		{
			this.GetPropertyMetadata( () => this.Text )
				.AddCascadeChangeNotifications( () => this.Sample );

			this.SetInitialPropertyValue( () => this.MergeErrors, true )
				.OnChanged( pvc =>
				{
					this.ValidationService.MergeValidationErrors = this.MergeErrors;
				} );
		}

		protected override IValidationService GetValidationService()
		{
			return new DataAnnotationValidationService<ValidationSampleViewModel>( this )
			{
				MergeValidationErrors = this.MergeErrors
			}.AddRule
			(
				property: () => this.Text,
				error: ctx => "must be equal to 'foo'",
				rule: ctx => ctx.Entity.Text == "foo"
			);
		}

		

		[DisplayName( "Esempio" )]
		public Int32 Sample
		{
			get { return this.GetPropertyValue( () => this.Sample ); }
			set { this.SetPropertyValue( () => this.Sample, value ); }
		}

		public Boolean MergeErrors
		{
			get { return this.GetPropertyValue( () => this.MergeErrors ); }
			set { this.SetPropertyValue( () => this.MergeErrors, value ); }
		}

		public void OnValidate( Radical.Validation.ValidationContext<ValidationSampleViewModel> context )
		{
			var displayname = this.ValidationService.GetPropertyDisplayName( this, o => o.Sample );

			context.Results.AddError( () => this.Sample, displayname, new[] { "This is fully custom, and works even on non-bound properties such as 'Sample'." } );
		}

		public void RunValidation()
		{
			this.Validate( ValidationBehavior.TriggerValidationErrorsOnFailure );
		}
	}
}
