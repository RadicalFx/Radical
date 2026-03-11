using System;
using System.Collections.Generic;
using System.Text;

namespace Radical.Validation
{
    /// <summary>
    /// Represents a validation error.
    /// </summary>
    public class ValidationError
    {
        readonly List<string> detectedProblems = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property that failed validation.</param>
        /// <param name="propertyDisplayName">The display name of the property.</param>
        /// <param name="detectedProblems">The list of detected problem descriptions.</param>
        public ValidationError(string propertyName, string propertyDisplayName, IEnumerable<string> detectedProblems)
        {
            Ensure.That(propertyName).Named(nameof(propertyName)).IsNotNullNorEmpty();
            Ensure.That(detectedProblems).Named("detectedProblems").IsNotNull();

            PropertyName = propertyName;
            PropertyDisplayName = propertyDisplayName;
            this.detectedProblems.AddRange(detectedProblems);
        }

        /// <summary>Gets the name of the property that failed validation.</summary>
        public string PropertyName { get; private set; }

        /// <summary>Gets the display name of the property.</summary>
        public string PropertyDisplayName { get; private set; }

        /// <summary>Gets the list of detected problem descriptions.</summary>
        public IEnumerable<string> DetectedProblems { get { return detectedProblems; } }

        /// <summary>
        /// Adds the given list of problems to the currently detected problems.
        /// </summary>
        /// <param name="problems">The problems to add.</param>
        public void AddProblems(IEnumerable<string> problems)
        {
            Ensure.That(problems).Named("problems").IsNotNull();

            detectedProblems.AddRange(problems);

            stringValue = null;
        }

        private string stringValue;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (stringValue == null)
            {
                var sb = new StringBuilder();

                foreach (var problem in DetectedProblems)
                {
                    sb.AppendLine(problem);
                }

                stringValue = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            }

            return stringValue;
        }
    }
}