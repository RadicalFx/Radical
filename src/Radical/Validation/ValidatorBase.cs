using System;
using System.Linq.Expressions;
using Radical.ComponentModel.Validation;
using Radical.Linq;
using Radical.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace Radical.Validation
{
    /// <summary>
    /// Defines a base implementation of the generic IValidator interface.
    /// </summary>
    /// <typeparam name="T">The type of the validated object.</typeparam>
    public class ValidatorBase<T> : IValidator<T>
    {
        readonly List<Action<ValidationContext<T>>> rules = new List<Action<ValidationContext<T>>>();
        ValidationTools tools = new ValidationTools();

        /// <summary>
        /// Gets the display name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected virtual String GetPropertyDisplayName(String propertyName, Object entity) 
        {
            return this.tools.GetPropertyDisplayName( propertyName, entity );
        }

        /// <summary>
        /// Gets the rule set.
        /// </summary>
        public String RuleSet { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBase&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        public ValidatorBase( String ruleSet )
        {
            this.RuleSet = ruleSet;
        }

        /// <summary>
        /// Determines whether the specified entity is valid.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid( T entity )
        {
            var results = this.Validate( entity );

            return results.IsValid;
        }

        /// <summary>
        /// Executes the validation process.
        /// </summary>
        /// <param name="context">The validation context.</param>
        protected virtual void OnValidate( ValidationContext<T> context )
        {
            this.rules.ForEach( rule => rule( context ) );

            var ivc = context.Entity as IRequireValidationCallback<T>;
            if ( ivc != null )
            {
                ivc.OnValidate( context );
            }
        }

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An instance of the <see cref="ValidationResults"/> with the results of the validation process.</returns>
        public ValidationResults Validate( T entity )
        {
            var context = new ValidationContext<T>( entity, this )
            {
                RuleSet = RuleSet
            };

            this.OnValidate( context );

            return context.Results;
        }

        /// <summary>
        /// Validates the specified property of the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults"/> with the results of the validation process.
        /// </returns>
        public ValidationResults Validate( T entity, string propertyName )
        {
            var context = new ValidationContext<T>( entity, this )
            {
                RuleSet = RuleSet,
                PropertyName = propertyName
            };

            this.OnValidate( context );

            return context.Results;
        }

        /// <summary>
        /// Validates the specified property of the given entity.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="property">The property to validate.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults"/> with the results of the validation process.
        /// </returns>
        public ValidationResults Validate<TProperty>( T entity, Expression<Func<T, TProperty>> property )
        {
            return this.Validate( entity, property.GetMemberName() );
        }

        /// <summary>
        /// Adds the given rule to the list of the validation rules.
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        /// <returns>The current validator instance.</returns>
        public IValidator<T> AddRule( Action<ValidationContext<T>> rule )
        {
            rules.Add( rule );
            return this;
        }


        /// <summary>
        /// Adds the rule.
        /// </summary>
        /// <param name="propertyIdentifier">The property identifier.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public IValidator<T> AddRule( Expression<Func<T, object>> propertyIdentifier, Func<ValidationContext<T>, RuleEvaluation> rule, string error )
        {
            return this.AddRule( propertyIdentifier, rule, ctx => error );
        }

        /// <summary>
        /// Adds the rule.
        /// </summary>
        /// <param name="propertyIdentifier">The property identifier.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IValidator<T> AddRule( Expression<Func<T, object>> propertyIdentifier, Func<ValidationContext<T>, RuleEvaluation> rule, Func<ValidationContext<T>, string> error )
        {
            return this.AddRule( ctx =>
            {
                var result = rule( ctx );
                if ( result == RuleEvaluation.Failed )
                {
                    var propertyName = propertyIdentifier.GetMemberName();
                    var displayName = this.GetPropertyDisplayName( propertyName, ctx.Entity );

                    ctx.Results.AddError( new ValidationError( propertyName, displayName, new[] { error( ctx ) } ) );
                }
            } );
        }

        /// <summary>
        /// Determines whether the specified entity is valid.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IValidator.IsValid( object entity )
        {
            return this.IsValid( ( T )entity );
        }

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults" /> with the results of the validation process.
        /// </returns>
        ValidationResults IValidator.Validate( object entity )
        {
            return this.Validate( ( T )entity );
        }

        /// <summary>
        /// Validates the specified property of the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults" /> with the results of the validation process.
        /// </returns>
        ValidationResults IValidator.Validate( object entity, string propertyName )
        {
            return this.Validate( ( T )entity, propertyName );
        }
    }
}