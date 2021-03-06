﻿using Radical.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Radical.Validation
{
    /// <summary>
    /// Defines the result(s) of a validation process.
    /// </summary>
    public class ValidationResults
    {
        /// <summary>
        /// Helper instance to expose an empty validation result.
        /// </summary>
        public static readonly ValidationResults Empty = new ValidationResults();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResults"/> class.
        /// </summary>
        public ValidationResults()
            : this(ValidationErrors.Empty)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResults"/> class.
        /// </summary>
        /// <param name="errors">The initial errors.</param>
        public ValidationResults(IEnumerable<ValidationError> errors)
        {
            Ensure.That(errors).Named("errors").IsNotNull();

            Errors = errors;
        }

        /// <summary>
        /// Gets a value indicating whether validation succeeded or not.
        /// </summary>
        /// <value><c>true</c> if the validation succeeded; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get
            {
                return Errors.None();
            }
        }

        /// <summary>
        /// Gets the list of validation errors.
        /// </summary>
        /// <value>The validation errors.</value>
        public IEnumerable<ValidationError> Errors { get; private set; }


        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="detectedProblems">The detected problems.</param>
        public void AddError<T>(Expression<Func<T>> key, string displayName, string[] detectedProblems)
        {
            var error = new ValidationError(key.GetMemberName(), displayName, detectedProblems);
            AddError(error);
        }

        /// <summary>
        /// Adds the given error to the validation errors list.
        /// </summary>
        /// <param name="error">The error to add.</param>
        public void AddError(ValidationError error)
        {
            Ensure.That(error).Named("error").IsNotNull();

            var err = Errors.Where(e => e.PropertyName == error.PropertyName).SingleOrDefault();
            if (err != null)
            {
                err.AddProblems(error.DetectedProblems);
            }
            else
            {
                var tmp = new List<ValidationError>(Errors);
                tmp.Add(error);

                Errors = tmp.AsReadOnly();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Some errors occurred during the validation process:");

            foreach (var error in Errors)
            {
                sb.AppendLine();
                sb.AppendFormat("{0}: {1}", error.PropertyName, error);
            }

            return sb.ToString();
        }
    }
}