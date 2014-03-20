using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Topics.Radical.Linq;
using Topics.Radical.Model;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Presentation
{
	/// <summary>
	/// A base abstract ViewModel with builtin support for validation, error notification.
	/// </summary>
	public abstract class AbstractViewModel :
		Entity,
		IViewModel
	{
		/// <summary>
		/// Gets or sets the view. The view property is intended only for
		/// infrastructural purpose. It is required to hold the one-to-one
		/// relation beteewn the view and the view model.
		/// </summary>
		/// <value>
		/// The view.
		/// </value>
		DependencyObject IViewModel.View { get; set; }

		//IValidationService _validationService;

		///// <summary>
		///// Gets the validation service.
		///// </summary>
		///// <value>The validation service.</value>
		//protected IValidationService ValidationService
		//{
		//	get
		//	{
		//		if( this._validationService == null )
		//		{
		//			this._validationService = this.GetValidationService();
		//			this._validationService.StatusChanged += ( s, e ) =>
		//			{
		//				this.ValidationErrors.Clear();
		//				foreach( var error in this._validationService.ValidationErrors )
		//				{
		//					this.ValidationErrors.Add( error );
		//				}
		//			};

		//			this._validationService.Resetted += ( s, e ) =>
		//			{
		//				this.ValidationErrors.Clear();
		//				this.GetType()
		//					.GetProperties()
		//					.Select( p => p.Name )
		//					.ForEach( p => this.OnPropertyChanged( p ) );
		//			};
		//		}

		//		return this._validationService;
		//	}
		//}

		///// <summary>
		///// Gets the validation service, this method is called once the first time
		///// the validation service is accessed, inheritors should override this method
		///// in order to provide an <see cref="IValidationService"/> implementation.
		///// </summary>
		///// <returns>The validation service to use to validate this view model.</returns>
		//protected virtual IValidationService GetValidationService()
		//{
		//	return NullValidationService.Instance;
		//}

		///// <summary>
		///// Initializes a new instance of the <see cref="AbstractViewModel"/> class.
		///// </summary>
		//protected AbstractViewModel()
		//{
		//	this.ValidationErrors = new ObservableCollection<ValidationError>();
		//}

		///// <summary>
		///// Gets the error.
		///// </summary>
		///// <value>The error.</value>
		///// <remarks>Used only in order to satisfy IDataErrorInfo interface implementation, the default implementation always returns null.</remarks>
		//public virtual String Error
		//{
		//	get { return null; }
		//}

		///// <summary>
		///// Gets the error message, if any, for the property with the given name.
		///// </summary>
		//public virtual String this[ String propertyName ]
		//{
		//	get
		//	{
		//		var wasValid = this.IsValid;

		//		var error = this.ValidationService.Validate( propertyName );

		//		if( this.IsValid != wasValid )
		//		{
		//			this.OnPropertyChanged( () => this.IsValid );
		//		}

		//		this.OnValidated();

		//		return error;
		//	}
		//}

		///// <summary>
		///// Gets a value indicating whether this instance is valid.
		///// </summary>
		///// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
		//public virtual Boolean IsValid
		//{
		//	get { return this.ValidationService.IsValid; }
		//}

		///// <summary>
		///// Gets the validation errors if any.
		///// </summary>
		///// <value>The validation errors.</value>
		//public virtual ObservableCollection<ValidationError> ValidationErrors
		//{
		//	get;
		//	private set;
		//}

		///// <summary>
		///// Validates this instance.
		///// </summary>
		///// <returns><c>True</c> if this instance is valid; otherwise <c>false</c>.</returns>
		//public Boolean Validate()
		//{
		//	return this.Validate(null, ValidationBehavior.Default );
		//}

		///// <summary>
		///// Validates this instance.
		///// </summary>
		///// <param name="behavior">The behavior.</param>
		///// <returns>
		/////   <c>True</c> if this instance is valid; otherwise <c>false</c>.
		///// </returns>
		//public Boolean Validate( ValidationBehavior behavior )
		//{
		//	return this.Validate( null, behavior );
		//}

		///// <summary>
		///// Validates this instance.
		///// </summary>
		///// <param name="ruleSet">The rule set.</param>
		///// <param name="behavior">The behavior.</param>
		///// <returns>
		/////   <c>True</c> if this instance is valid; otherwise <c>false</c>.
		///// </returns>
		//public virtual Boolean Validate( String ruleSet, ValidationBehavior behavior )
		//{
		//	this.ValidationService.ValidateRuleSet( ruleSet );
		//	this.OnValidated();

		//	if( behavior == ValidationBehavior.TriggerValidationErrorsOnFailure && !this.ValidationService.IsValid )
		//	{
		//		this.TriggerValidation();
		//	}

		//	return this.ValidationService.IsValid;
		//}

		///// <summary>
		///// Occurs when the validation process terminates.
		///// </summary>
		//public event EventHandler Validated;

		///// <summary>
		///// Raises the Validated event.
		///// </summary>
		//protected virtual void OnValidated()
		//{
		//	if( this.Validated != null )
		//	{
		//		this.Validated( this, EventArgs.Empty );
		//	}
		//}

		///// <summary>
		///// Triggers the validation.
		///// </summary>
		//public virtual void TriggerValidation()
		//{
		//	if( !this.IsTriggeringValidation )
		//	{
		//		this.IsTriggeringValidation = true;

		//		foreach( var invalid in this.ValidationService.GetInvalidProperties() )
		//		{
		//			this.OnPropertyChanged( invalid );
		//		}

		//		this.IsTriggeringValidation = false;
		//	}
		//}

		///// <summary>
		///// Gets or sets a value indicating whether this instance is triggering validation.
		///// </summary>
		///// <value>
		///// 	<c>true</c> if this instance is triggering validation; otherwise, <c>false</c>.
		///// </value>
		//protected virtual Boolean IsTriggeringValidation
		//{
		//	get;
		//	private set;
		//}
	}
}