using System;
using System.Linq;
using System.Collections.Generic;
using Radical.Validation;
using Radical.Linq;
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
            : this( ValidationErrors.Empty )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResults"/> class.
        /// </summary>
        /// <param name="errors">The initial errors.</param>
        public ValidationResults( IEnumerable<ValidationError> errors )
        {
            Ensure.That( errors ).Named( "errors" ).IsNotNull();

            this.Errors = errors;
        }

        /// <summary>
        /// Gets a value indicating whether validation succedeed or not.
        /// </summary>
        /// <value><c>true</c> if the validation succedeed; otherwise, <c>false</c>.</value>
        public Boolean IsValid
        {
            get
            {
                return this.Errors.None();
            }
        }

        /// <summary>
        /// Gets the list of validation errors.
        /// </summary>
        /// <value>The validation errors.</value>
        public IEnumerable<ValidationError> Errors { get; private set; }

        /// <summary>
        /// Adds a new error for the given key with the supplied detected problems.
        /// </summary>
        /// <typeparam name="T">The type of the validated property.</typeparam>
        /// <param name="key">The key the represents the name of the validated property.</param>
        /// <param name="detectedProblems">The detected problems.</param>
        [Obsolete( "Use the overload without the evil 'params' keyword using an explicit array: new[] { 'your error text here' }.", error: true )]
        public void AddError<T>( Expression<Func<T>> key, params String[] detectedProblems )
        {
            var error = ValidationError.Create<T>( key, key.GetMemberName(), detectedProblems );
            this.AddError( error );
        }

        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="detectedProblems">The detected problems.</param>
        public void AddError<T>( Expression<Func<T>> key, String displayName, String[] detectedProblems )
        {
            var error = ValidationError.Create<T>( key, displayName, detectedProblems );
            this.AddError( error );
        }

        /// <summary>
        /// Adds the given error to the validation errors list.
        /// </summary>
        /// <param name="error">The error to add.</param>
        public void AddError( ValidationError error )
        {
            Ensure.That( error ).Named( "error" ).IsNotNull();

            var err = this.Errors.Where( e => e.Key == error.Key ).SingleOrDefault();
            if ( err != null )
            {
                err.AddProblems( error.DetectedProblems );
            }
            else
            {
                var tmp = new List<ValidationError>( this.Errors );
                tmp.Add( error );

                this.Errors = tmp.AsReadOnly();
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
            sb.AppendLine( "Some errors occurred during the validation process:" );

            foreach ( var error in this.Errors )
            {
                sb.AppendLine();
                sb.AppendFormat( "{0}: {1}", error.Key, error );
            }

            return sb.ToString();
        }
    }
}