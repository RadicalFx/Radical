namespace Radical.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Entry point for accessing assembly types.
    /// </summary>
    public static class GetAssembly
    {
        /// <summary>
        /// Returns a reference to the assembly that contains the gievn type T.
        /// </summary>
        /// <typeparam name="T">The type whose assembly reference should be returned.</typeparam>
        /// <returns>An instance of the assembly containing the given type T.</returns>
        public static Assembly ThatContains<T>()
        {
            return typeof(T).Assembly;
        }
    }

    /// <summary>
    /// Adds behaviors to an assembly class instance.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Determines whether the given attribute is defined on the specified assembly.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        ///     <c>true</c> if the attribute is defined on the specified assembly; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAttributeDefined<T>(this Assembly assembly)
            where T : Attribute
        {
            return assembly.IsDefined(typeof(T), true);
        }

        /// <summary>
        /// Gets a list of attributes, identified by 
        /// the given attribute type, applied, at assembly
        /// level, to the assembly.
        /// </summary>
        /// <typeparam name="T">The attribute type to look for.</typeparam>
        /// <param name="assembly">The assembly to search in.</param>
        /// <returns>A list of attribute if any; otherwise an empty list.</returns>
        public static IEnumerable<T> GetAttributes<T>(this Assembly assembly)
            where T : Attribute
        {
            return assembly.GetCustomAttributes(typeof(T), true).OfType<T>();
        }

        /// <summary>
        /// Gets the attribute at assembly level identified by the 
        /// given attribute type T.
        /// </summary>
        /// <typeparam name="T">The attribute type to look for.</typeparam>
        /// <param name="assembly">The assembly to search in.</param>
        /// <returns>The attribute instance.</returns>
        public static T GetAttribute<T>(this Assembly assembly)
            where T : Attribute
        {
            return assembly.GetAttributes<T>().First();
        }

        /// <summary>
        /// Tries to get the attribute at assembly level identified by the 
        /// given attribute type T.
        /// </summary>
        /// <typeparam name="T">The attribute type to look for.</typeparam>
        /// <param name="assembly">The assembly to search in.</param>
        /// <param name="attribute">The attribute used to assign the output refence.</param>
        /// <returns><c>True</c> if an attribute of the given type can be found; otherwise false.</returns>
        public static bool TryGetAttribute<T>(this Assembly assembly, out T attribute)
            where T : Attribute
        {
            if (assembly.IsAttributeDefined<T>())
            {
                attribute = assembly.GetAttribute<T>();
            }
            else
            {
                attribute = null;
            }

            return attribute != null;
        }
    }
}
