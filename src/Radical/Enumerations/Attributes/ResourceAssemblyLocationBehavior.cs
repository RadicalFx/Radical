namespace Radical
{
    /// <summary>
    /// Determines how the assembly is located by the resource manager.
    /// </summary>
    public enum ResourceAssemblyLocationBehavior
    {
        /// <summary>
        /// Identify the assembly given its name.
        /// </summary>
        ByAssemblyName = 0,

        /// <summary>
        /// Use the executing assembly.
        /// </summary>
        UseExecutingAssembly,

        /// <summary>
        /// Use the calling assembly.
        /// </summary>
        UseCallingAssembly,

        /// <summary>
        /// Use the entry assembly.
        /// </summary>
        UseEntryAssembly

    }
}
