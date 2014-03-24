using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Windows.Presentation.Boot
{
    /// <summary>
    /// Arguments to handle the application startup when singleton mode is set up.
    /// </summary>
    public class SingletonApplicationStartupArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonApplicationStartupArgs"/> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public SingletonApplicationStartupArgs( SingletonApplicationScope scope )
        {
            this.Scope = scope;
            this.AllowStartup = true;
        }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        public SingletonApplicationScope Scope { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the startup is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the startup is allowed; otherwise, <c>false</c>.
        /// </value>
        public Boolean AllowStartup { get; set; }
    }
}
