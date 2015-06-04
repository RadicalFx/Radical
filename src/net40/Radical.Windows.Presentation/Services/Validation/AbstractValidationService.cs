using System;
using System.Collections.Generic;
using System.Linq;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Services.Validation
{
	/// <summary>
	/// Provides a base implementation of the <see cref="IValidationService"/> interface.
	/// </summary>
	public abstract class AbstractValidationService : IValidationService
	{
		readonly List<ValidationError> _validationErrors = new List<ValidationError>();
		ValidationTools tools = new ValidationTools();

		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractValidationService"/> class.
		/// </summary>
		protected AbstractValidationService()
		{
			this.IsValid = true;
			this.MergeValidationErrors = false;
		}

        //readonly HashSet<String> validationCalledOnce = new HashSet<String>();

        ///// <summary>
        ///// Called in order to understand if the validation for the 
        ///// supplied property has already been called at least one time.
        ///// </summary>
        ///// <param name="propertyName">Name of the property.</param>
        ///// <returns><c>True</c> if the supplied property has been validated at least once; otherwise <c>false</c>.</returns>
        //protected virtual Boolean ValidationCalledOnceFor( String propertyName )
        //{
        //    return this.validationCalledOnce.Contains( propertyName );
        //}

        ///// <summary>
        ///// Registers that the validation process has been called 
        ///// at least once for the supplied property.
        ///// </summary>
        ///// <param name="propertyName">Name of the property.</param>
        //protected virtual void RegisterValidationCalledOnceFor( String propertyName )
        //{
        //    this.validationCalledOnce.Add( propertyName );
        //}

		/// <summary>
		/// Validates the specified property.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		/// <returns>
		/// The validation error message if any; otherwise a null or empty string.
		/// </returns>
		public String Validate( String propertyName )
		{
			return this.ValidateRuleSet( null, propertyName );
		}

		/// <summary>
		/// Starts the validation process.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <param name="propertyName">The name of the property to validate.</param>
		/// <returns>
		/// The validation error message if any; otherwise a null or empty string.
		/// </returns>
		public String ValidateRuleSet( String ruleSet, String propertyName )
		{
            //if( !this.ValidationCalledOnceFor( propertyName ) )
            //{
            //    /*
            //     * Se non abbiamo mai validato la proprietà significa che siamo 
            //     * allo startup della Window e il motore di validazione di WPF 
            //     * viene triggherato per ogni "set" di ogni binding. Dato che non
            //     * ci interessa visualizzare la Window come non valida sin da 
            //     * subito evitiamo la validazione al primo controllo.
            //     */
            //    this.RegisterValidationCalledOnceFor( propertyName );
            //    return null;
            //}

			if( this.IsValidationSuspended )
			{
				return null;
			}

			var isValidBeforeValidation = this.IsValid;

			var results = this.OnValidateProperty( ruleSet, propertyName );

			var toBeRemoved = this._validationErrors
				.Where( e => e.Key == propertyName )
				.ToArray()
				.ForEach( e => this._validationErrors.Remove( e ) );

			this.AddValidationErrors( results.ToArray() );

			var shouldTriggerStatusChanged = toBeRemoved.Any() || results.Any();
			this.IsValid = this.ValidationErrors.None();

			if( this.IsValid != isValidBeforeValidation || shouldTriggerStatusChanged )
			{
				this.OnStatusChanged( EventArgs.Empty );
			}

			if( results.Any() )
			{
				var error = results.Select( err => err.ToString() )
					.First();

				return error;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the validation process
		/// returns a valid response or not.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the validation process has successfully passed the validation process.; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsValid
		{
			get;
			private set;
		}

		/// <summary>
		/// Occurs when validation status changes.
		/// </summary>
		public event EventHandler StatusChanged;

		/// <summary>
		/// Occurs when this service is resetted.
		/// </summary>
		public event EventHandler ValidationReset;

		/// <summary>
		/// Raises the <see cref="E:StatusChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnStatusChanged( EventArgs e )
		{
			var h = this.StatusChanged;
			if( h != null )
			{
				h( this, e );
			}
		}

		/// <summary>
        /// Raises the <see cref="E:ValidationReset"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnValidationReset( EventArgs e )
		{
			var h = this.ValidationReset;
			if( h != null )
			{
				h( this, e );
			}
		}

		/// <summary>
		/// Gets the validation errors.
		/// </summary>
		/// <value>
		/// All the validation errors.
		/// </value>
		public IEnumerable<ValidationError> ValidationErrors
		{
			get { return this._validationErrors; }
		}

		/// <summary>
		/// Adds the validation errors.
		/// </summary>
		/// <param name="errors">The errors.</param>
		protected void AddValidationErrors( params ValidationError[] errors )
		{
			Ensure.That( errors ).Named( () => errors ).IsNotNull();

			if( this.MergeValidationErrors )
			{
				foreach( var error in errors )
				{
					var actual = this._validationErrors.SingleOrDefault( ve => ve.Key == error.Key );
					if( actual != null )
					{
						actual.AddProblems( error.DetectedProblems.ToArray() );
					}
					else
					{
						this._validationErrors.Add( error );
					}
				}
			}
			else
			{
				this._validationErrors.AddRange( errors );
			}
		}

		/// <summary>
		/// Clears all the current validation errors.
		/// </summary>
		protected void ClearErrors()
		{
			this._validationErrors.Clear();
		}

		/// <summary>
		/// Starts the validation process.
		/// </summary>
		/// <returns>
		///   <c>True</c> if the validation process succedeed; otherwise <c>false</c>.
		/// </returns>
		public Boolean Validate()
		{
			return this.ValidateRuleSet( null );
		}

		/// <summary>
		/// Starts the validation process.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <returns>
		///   <c>True</c> if the validation process succedeed; otherwise <c>false</c>.
		/// </returns>
		public virtual Boolean ValidateRuleSet( String ruleSet )
		{
			if( this.IsValidationSuspended )
			{
				return this.IsValid;
			}

			var isValidBeforeValidation = this.IsValid;
			var result = this.OnValidate( ruleSet );

			this.ClearErrors();
			this.AddValidationErrors( result.ToArray() );

			this.IsValid = result.None();

			if( this.IsValid != isValidBeforeValidation || !this.IsValid )
			{
				this.OnStatusChanged( EventArgs.Empty );
			}

			return this.IsValid;
		}

		/// <summary>
		/// Called in order to execute the concrete validation process.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <returns>
		/// A list of <seealso cref="ValidationError"/>.
		/// </returns>
		protected abstract IEnumerable<ValidationError> OnValidate( String ruleSet );

		/// <summary>
		/// Called in order to execute the concrete validation process on the given property.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>
		/// A list of <seealso cref="ValidationError" />.
		/// </returns>
		protected virtual IEnumerable<ValidationError> OnValidateProperty( String ruleSet, String propertyName )
		{
			if( !this.IsValidationSuspended && !this.ValidateRuleSet( ruleSet ) )
			{
				/*
				 * Se la validazione fallisce dobbiamo capire se è fallita
				 * per colpa della proprietà che ci è stato chiesto di validare.
				 * Cerchiamo quindi nella lista degli errori uno che abbia come Key
				 * il nome della proprietà
				 */
				return this.ValidationErrors
					.Where( err => err.Key == propertyName )
					.ToArray();
			}

			return new ValidationError[ 0 ];
		}

		/// <summary>
		/// Gets the invalid properties.
		/// </summary>
		/// <returns>
		/// A list of property names that identifies the invalid properties.
		/// </returns>
		public virtual IEnumerable<String> GetInvalidProperties()
		{
			return this.ValidationErrors.Select( ve => ve.Key )
                .Distinct()
                .AsReadOnly();
		}

		/// <summary>
		/// Clears the validation state resetting to its default valid value.
		/// </summary>
		public void Reset() 
		{
			this.Reset( ValidationResetBehavior.All );
		}

		/// <summary>
		/// Clears the validation state resetting to its default valid value.
		/// </summary>
		/// <param name="resetBehavior">The reset behavior.</param>
		public virtual void Reset( ValidationResetBehavior resetBehavior )
		{
			if( ( resetBehavior & ValidationResetBehavior.ErrorsOnly ) == ValidationResetBehavior.ErrorsOnly )
			{
				this._validationErrors.Clear();
			}

			this.IsValid = true;

            //if( ( resetBehavior & ValidationResetBehavior.ValidationTracker ) == ValidationResetBehavior.ValidationTracker )
            //{
            //    this.validationCalledOnce.Clear();
            //}

			this.OnValidationReset( EventArgs.Empty );
		}

		class ValidationSuspender : IDisposable
		{
			public void Dispose()
			{
				this.onDisposed();
			}

			readonly Action onDisposed;

			public ValidationSuspender( Action onDisposed )
			{
				this.onDisposed = onDisposed;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the validation process is suspended.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the validation process is suspended; otherwise, <c>false</c>.
		/// </value>
		public Boolean IsValidationSuspended { get; private set; }

		ValidationSuspender suspender = null;

		/// <summary>
		/// Suspends the validation.
		/// </summary>
		/// <returns>A disposable instance to automatically resume validation on dispose.</returns>
		public IDisposable SuspendValidation()
		{
			if( !this.IsValidationSuspended )
			{
				this.IsValidationSuspended = true;
				this.suspender = new ValidationSuspender( () => this.ResumeValidation() );
			}

			return this.suspender;
		}

		/// <summary>
		/// Resumes the validation.
		/// </summary>
		public void ResumeValidation()
		{
			if( this.IsValidationSuspended )
			{
				this.IsValidationSuspended = false;
				this.suspender = null;
			}
		}


		/// <summary>
		/// Gets the display name of the property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">The entity.</param>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public string GetPropertyDisplayName<T>( T entity, System.Linq.Expressions.Expression<Func<T, object>> property )
		{
			var propertyname = property.GetMemberName();
			return this.GetPropertyDisplayName( entity, propertyname );
		}

		/// <summary>
		/// Gets the display name of the property.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		public virtual string GetPropertyDisplayName( object entity, string propertyName )
		{
			return this.tools.GetPropertyDisplayName( propertyName, entity );
		}

		Boolean _mergeValidationErrors;

		/// <summary>
		/// Gets or sets if the service should merge validation errors related to the same property.
		/// </summary>
		/// <value>
		/// <c>True</c> if the service should merge validation errors related to the same property; otherwise <c>False</c>.
		/// </value>
		/// <exception cref="System.NotImplementedException">
		/// </exception>
		public bool MergeValidationErrors
		{
			get { return this._mergeValidationErrors; }
			set
			{
				if( value != this.MergeValidationErrors )
				{
					this._mergeValidationErrors = value;

					if( this.ValidationErrors.Any() )
					{
						//reset the errors if any so the have them gruoped
						var actual = this.ValidationErrors.ToArray();
						this._validationErrors.Clear();
						this.AddValidationErrors( actual );

						this.OnValidationReset( EventArgs.Empty );
					}
				}
			}
		}
	}
}