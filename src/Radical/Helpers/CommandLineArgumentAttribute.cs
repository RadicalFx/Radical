using Radical.Validation;
using System;

namespace Radical.Helpers
{
    /// <summary>
    /// Defines the command line argument name associated to a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class CommandLineArgumentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgumentAttribute"/> class.
        /// </summary>
        /// <param name="argumentName">Name of the argument.</param>
        public CommandLineArgumentAttribute(string argumentName)
        {
            Ensure.That(argumentName)
                .Named(() => argumentName)
                .IsNotNullNorEmpty();

            ArgumentName = argumentName;
            Aliases = new string[0];
        }

        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        /// <value>
        /// The name of the argument.
        /// </value>
        public string ArgumentName { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this command line argument is required.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this command line argument is required; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the aliases.
        /// </summary>
        /// <value>
        /// The aliases.
        /// </value>
        public string[] Aliases { get; set; }
    }
}
