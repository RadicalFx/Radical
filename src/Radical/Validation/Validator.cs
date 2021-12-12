using Radical.ComponentModel.Validation;
using Radical.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Radical.Validation
{
    /// <summary>
    /// Defines a base implementation of the generic IValidator interface.
    /// </summary>
    /// <typeparam name="T">The type of the validated object.</typeparam>
    public class Validator<T>
    {
        readonly List<ValidationRule<T>> rules = new List<ValidationRule<T>>();

        /// <summary>
        /// Determines whether the specified entity is valid.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid(T entity)
        {
            return Validate(entity).IsValid;
        }

        ValidationResults OnValidate(ValidationContext<T> context)
        {
            var rulesToEvaluate = rules.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(context.PropertyName)) 
            {
                rulesToEvaluate = rulesToEvaluate.Where(rule => rule.Property.GetMemberName() == context.PropertyName);
            }

            rulesToEvaluate.ForEach(rule => context.Evaluate(rule));

            if (context.Entity is IRequireValidationCallback<T> irvc)
            {
                irvc.OnValidate(context);
            }

            return context.Results;
        }

        /// <summary>
        /// Validates the given entity running all the validation rules.
        /// </summary>
        /// <param name="entity">The entity to run the validation against.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults"/> with the results of the validation process.
        /// </returns>
        public ValidationResults Validate(T entity)
        {
            return OnValidate(new ValidationContext<T>(entity, this));
        }

        /// <summary>
        /// Validates the given property of the supplied entity running all the validation rules for the given property.
        /// </summary>
        /// <param name="entity">The entity to run the validation against.</param>
        /// <param name="propertyName">The property to validate.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults"/> with the results of the validation process.
        /// </returns>
        public ValidationResults ValidateProperty(T entity, string propertyName)
        {
            return OnValidate(new ValidationContext<T>(entity, this)
            {
                PropertyName = propertyName
            });
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
        public ValidationResults ValidateProperty<TProperty>(T entity, Expression<Func<T, TProperty>> property)
        {
            return ValidateProperty(entity, property.GetMemberName());
        }

        /// <summary>
        /// Add a new validation rule.
        /// </summary>
        /// <param name="property">The property to validate.</param>
        /// <param name="rule">The callback executed to run the rule.</param>
        public Validator<T> AddRule(Expression<Func<T, object>> property, Func<ValidationContext<T>, ValidationResult> rule)
        {
            rules.Add(new ValidationRule<T> 
            {
                Property = property,
                Rule = rule
            });
            return this;
        }

        /// <summary>
        /// Determines whether the specified entity is valid.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid(object entity)
        {
            return IsValid((T)entity);
        }

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults" /> with the results of the validation process.
        /// </returns>
        public ValidationResults Validate(object entity)
        {
            return Validate((T)entity);
        }

        /// <summary>
        /// Validates the specified property of the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="propertyName">The name of the property to validate.</param>
        /// <returns>
        /// An instance of the <see cref="ValidationResults" /> with the results of the validation process.
        /// </returns>
        public ValidationResults ValidateProperty(object entity, string propertyName)
        {
            return ValidateProperty((T)entity, propertyName);
        }
    }
}