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
        public ValidationError(string propertyName, string propertyDisplayName, IEnumerable<string> detectedProblems)
        {
            Ensure.That(propertyName).Named(nameof(propertyName)).IsNotNullNorEmpty();
            Ensure.That(detectedProblems).Named("detectedProblems").IsNotNull();

            PropertyName = propertyName;
            PropertyDisplayName = propertyDisplayName;
            this.detectedProblems.AddRange(detectedProblems);
        }

        public string PropertyName { get; private set; }

        public string PropertyDisplayName { get; private set; }

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