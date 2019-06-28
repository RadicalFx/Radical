using Radical.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Radical.Validation
{
    /// <summary>
    /// Represents a validation error.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Creates a new <see cref="ValidationError" /> with the specified key and problems.
        /// </summary>
        /// <typeparam name="T">The type of the validated property.</typeparam>
        /// <param name="key">The key that represents the name of the validated property.</param>
        /// <param name="keyDisplayName">Display name of the key.</param>
        /// <param name="detectedProblems">The detected problems.</param>
        /// <returns>
        /// The newly created <see cref="ValidationError" />.
        /// </returns>
        public static ValidationError Create<T>( Expression<Func<T>> key, string keyDisplayName, params string[] detectedProblems )
        {
            return new ValidationError( key.GetMemberName(), keyDisplayName, detectedProblems );
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ValidationError" /> class.
        ///// </summary>
        ///// <param name="key">The key.</param>
        ///// <param name="keyDisplayName">Display name of the key.</param>
        ///// <param name="detectedProblems">The detected problems.</param>
        //public ValidationError( string key, string keyDisplayName, params string[] detectedProblems )
        //    : this( key, keyDisplayName, ( IEnumerable<string> )detectedProblems )
        //{

        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError" /> class.
        /// </summary>
        /// <param name="key">The key, tipically the invalid property name.</param>
        /// <param name="keyDisplayName">Display name of the key.</param>
        /// <param name="detectedProblems">The detected problems.</param>
        public ValidationError( string key, string keyDisplayName, IEnumerable<string> detectedProblems )
        {
            Ensure.That( key ).Named( "key" ).IsNotNullNorEmpty();
            Ensure.That( detectedProblems ).Named( "detectedProblems" ).IsNotNull();

            this.Key = key;
            this.KeyDisplayName = keyDisplayName;
            this.DetectedProblems = detectedProblems;
        }

        /// <summary>
        /// Gets the error key.
        /// </summary>
        /// <value>The error key.</value>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the display name of the key.
        /// </summary>
        /// <value>
        /// The display name of the key.
        /// </value>
        public string KeyDisplayName { get; private set; }

        /// <summary>
        /// Gets the detected problems.
        /// </summary>
        /// <value>The detected problems.</value>
        public IEnumerable<string> DetectedProblems { get; private set; }

        /// <summary>
        /// Adds the given list of problems to the currently detected problems.
        /// </summary>
        /// <param name="problems">The problems to add.</param>
        public void AddProblems( IEnumerable<string> problems )
        {
            Ensure.That( problems ).Named( "problems" ).IsNotNull();

            var tmp = new List<string>( this.DetectedProblems );
            tmp.AddRange( problems );

            this.DetectedProblems = tmp.AsReadOnly();

            this.stringValue = null;
        }

        private string stringValue;

        /// <summary>
        /// Returns a <see cref="System.string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if( stringValue == null )
            {
                var sb = new StringBuilder();

                foreach( var problem in this.DetectedProblems )
                {
                    sb.AppendLine( problem );
                }

                stringValue = sb.ToString().TrimEnd( Environment.NewLine.ToCharArray() );
            }

            return stringValue;
        }
    }
}