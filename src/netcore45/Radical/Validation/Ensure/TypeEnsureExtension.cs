//using System;
//using System.Reflection;
//using Topics.Radical.Reflection;

//namespace Topics.Radical.Validation
//{
//    /// <summary>
//    /// Defines some extensions facilities for the <see cref="Ensure"/> class
//    /// when used to validate system types.
//    /// </summary>
//    public static class TypeEnsureExtension
//    {
//        /// <summary>
//        /// Throws an <see cref="ArgumentException"/> if the currently 
//        /// inspected <see cref="Type"/> does not match the specified Type.
//        /// </summary>
//        /// <typeparam name="T">The <see cref="Type"/> to compare.</typeparam>
//        /// <param name="validator">The current validator.</param>
//        /// <returns>The current validator for fluent interface usage.</returns>
//        public static IEnsure<Type> Is<T>( this IEnsure<Type> validator )
//        {
//            return validator.IsTrue( data => data.Is<T>() );
//        }
//    }
//}
