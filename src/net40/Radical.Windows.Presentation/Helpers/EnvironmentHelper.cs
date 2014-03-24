using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Topics.Radical.Windows.Presentation.Helpers
{
    /// <summary>
    /// An helper calss for environment information.
    /// </summary>
    public class EnvironmentHelper
    {
        /// <summary>
        /// Gets the current directory.
        /// </summary>
        /// <returns>The directory the executable is running from.</returns>
        public static String GetCurrentDirectory()
        {
            return Path.GetDirectoryName( Assembly.GetEntryAssembly().Location );
        }
    }
}
