using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Validation;

namespace Topics.Radical.Helpers
{
    /// <summary>
    /// Defines the command line argument name associated to a property.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = false )]
    public sealed class CommandLineArgumentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgumentAttribute"/> class.
        /// </summary>
        /// <param name="argumentName">Name of the argument.</param>
        public CommandLineArgumentAttribute( String argumentName )
        {
            Ensure.That( argumentName )
                .Named( () => argumentName )
                .IsNotNullNorEmpty();

            this.ArgumentName = argumentName;
            this.Aliases = new String[ 0 ];
        }

        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        /// <value>
        /// The name of the argument.
        /// </value>
        public String ArgumentName { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this command line argument is required.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this command line argument is required; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the aliases.
        /// </summary>
        /// <value>
        /// The aliases.
        /// </value>
        public String[] Aliases { get; set; }
    }
}
