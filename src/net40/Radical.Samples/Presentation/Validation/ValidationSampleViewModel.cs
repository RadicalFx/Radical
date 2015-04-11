using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		[Required( AllowEmptyStrings = false )]
		[DisplayName( "Altro Testo" )]
		public String AnotherText
		{
			get { return this.GetPropertyValue( () => this.AnotherText ); }
			set { this.SetPropertyValue( () => this.AnotherText, value ); }
		}
	}

	//public interface IRequireAsyncValidation : INotifyDataErrorInfo
	//{
	//	/// <summary>
	//	/// Gets a value indicating whether this instance is valid.
	//	/// </summary>
	//	/// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
	//	Boolean IsValid { get; }

	//	/// <summary>
	//	/// Gets the validation errors.
	//	/// </summary>
	//	/// <value>The validation errors.</value>
	//	ObservableCollection<ValidationError> ValidationErrors { get; }

	//	/// <summary>
	//	/// Validates this instance.
	//	/// </summary>
	//	/// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
	//	Task<Boolean> ValidateAsync();

	//	/// <summary>
	//	/// Validates this instance.
	//	/// </summary>
	//	/// <param name="behavior">The behavior.</param>
	//	/// <returns>
	//	///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
	//	/// </returns>
	//	Task<Boolean> ValidateAsync( ValidationBehavior behavior );

	//	/// <summary>
	//	/// Validates this instance.
	//	/// </summary>
	//	/// <param name="ruleSet">The rule set.</param>
	//	/// <param name="behavior">The behavior.</param>
	//	/// <returns>
	//	///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
	//	/// </returns>
	//	Task<Boolean> ValidateAsync( String ruleSet, ValidationBehavior behavior );

	//	/// <summary>
	//	/// Occurs when when the validation process is completed.
	//	/// </summary>
	//	event EventHandler Validated;

	//	/// <summary>
	//	/// Triggers the validation process on this instances forcing all the invalid
	//	/// fields to notify their invalid status.
	//	/// </summary>
	//	void TriggerValidation();
	//}

	[Sample( Title = "DataAnnotation Validation", Category = Categories.Validation )]
	class ValidationSampleViewModel :
		AbstractValidationSampleViewModel,
		IRequireValidationCallback<ValidationSampleViewModel>,
		IRequireValidation
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
			var svc = new DataAnnotationValidationService<ValidationSampleViewModel>( this )
			{
				MergeValidationErrors = this.MergeErrors
			}.AddRule
			(
				property: () => this.Text,
				error: ctx => "must be equal to 'foo'",
				rule: ctx => ctx.Entity.Text == "foo"
			);

			return svc;
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
