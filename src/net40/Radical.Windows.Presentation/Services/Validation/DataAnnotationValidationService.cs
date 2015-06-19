using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Topics.Radical.Linq;
using Topics.Radical.ComponentModel.Validation;
using Topics.Radical.Validation;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Presentation.Services.Validation;
using System.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Services.Validation
{
    /// <summary>
    /// DataAnnotationValidationService factory.
    /// </summary>
    public static class DataAnnotationValidationService 
    {
        /// <summary>
        /// Creates a new DataAnnotationValidationService instance for the given entity..
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static DataAnnotationValidationService<TEntity> CreateFor<TEntity>( TEntity entity ) 
        {
            return new DataAnnotationValidationService<TEntity>( entity );
        }
    }

	/// <summary>
	/// Validation service specialized to validates entities 
	/// decorated with the DataAnnotation attributes.
	/// </summary>
	/// <typeparam name="TEntity">The type of the entity.</typeparam>
	public class DataAnnotationValidationService<TEntity> : AbstractValidationService
	{
		TEntity entity;
		IValidator<TEntity> customValidator;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataAnnotationValidationService{TEntity}" /> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public DataAnnotationValidationService( TEntity entity )
		{
			this.entity = entity;
			this.PropertyValueGetter = ( property, obj ) =>
			{
				var val = obj.GetType()
					.GetProperty( property )
					.GetValue( obj, null );

				return val;
			};

			var factory = new ValidatorBaseFactory();
			this.customValidator = factory.CreateValidator<TEntity>();
		}

		/// <summary>
		/// Gets or sets the property value getter that is responsible to
		/// retrieve the value of the property that will be validated.
		/// </summary>
		/// <value>
		/// The property value getter.
		/// </value>
		public Func<String, Object, Object> PropertyValueGetter { get; set; }

		/// <summary>
		/// Called in order to execute the concrete validation process.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <returns>
		/// A list of <seealso cref="ValidationError" />.
		/// </returns>
		protected override IEnumerable<ValidationError> OnValidate( string ruleSet )
		{
			var errors = new List<ValidationError>();

			var results = new List<ValidationResult>();
			var ctx = new ValidationContext( this.entity, null, null );
			Validator.TryValidateObject( this.entity, ctx, results, true );

			var customRulesResults = this.customValidator.Validate( this.entity );
			if ( !customRulesResults.IsValid )
			{
				errors.AddRange( customRulesResults.Errors );
			}

			if ( results.Any() )
			{
				errors.AddRange( results.Select( r =>
				{
					var memberName = r.MemberNames.Single();
					return new ValidationError( 
						memberName, 
						this.GetPropertyDisplayName( this.entity, memberName ), 
						new[] { r.ErrorMessage } );
				} ) );
			}

			return errors;
		}

		/// <summary>
		/// Called in order to execute the concrete validation process on the given property.
		/// </summary>
		/// <param name="ruleSet">The rule set.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>
		/// A list of <seealso cref="ValidationError" />.
		/// </returns>
		protected override IEnumerable<ValidationError> OnValidateProperty(String ruleSet, string propertyName )
		{
			var errors = new List<ValidationError>();

			var results = new List<ValidationResult>();
			var ctx = new ValidationContext( this.entity, null, null )
			{
				MemberName = propertyName,
			};

			var val = this.PropertyValueGetter( propertyName, this.entity );

			Validator.TryValidateProperty( val, ctx, results );

			var customRulesResults = this.customValidator.Validate( this.entity, propertyName );
			if ( !customRulesResults.IsValid )
			{
				errors.AddRange( customRulesResults.Errors.Where( e => e.Key == propertyName ) );
			}

			if ( results.Any() )
			{
				errors.AddRange( results.Select( r =>
				{
					var memberName = r.MemberNames.Single();
					return new ValidationError( 
						memberName, 
						this.GetPropertyDisplayName( this.entity, memberName ), 
						new[] { r.ErrorMessage } );
				} ) );
			}

			return errors;
		}

		/// <summary>
		/// Gets the display name of the property.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		public override string GetPropertyDisplayName( object entity, string propertyName )
		{
			var pi = entity.GetType().GetProperty( propertyName );
			if ( pi != null && pi.IsAttributeDefined<DisplayAttribute>() )
			{
				var a = pi.GetAttribute<DisplayAttribute>();
				return a.GetName();
			}

			return base.GetPropertyDisplayName( entity, propertyName );
		}

		/// <summary>
		/// Adds a custom validation rule.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <param name="error">The error.</param>
		/// <param name="rule">The rule.</param>
		/// <returns></returns>
		public DataAnnotationValidationService<TEntity> AddRule( Expression<Func<Object>> property, Func<ValidationContext<TEntity>, String> error, Func<ValidationContext<TEntity>, Boolean> rule )
		{
			this.customValidator.AddRule( ctx =>
			{
				var result = rule( ctx );
				if ( !result )
				{
					var propertyName = property.GetMemberName();
					var displayname = this.GetPropertyDisplayName( this.entity, propertyName );
					ctx.Results.AddError( new ValidationError( propertyName, displayname, new[] { error( ctx ) } ) );
				}
			} );

			return this;
		}
	}
}
