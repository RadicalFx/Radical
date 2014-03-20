using System.Diagnostics;
using System;
using System.Reflection;
namespace Topics.Radical.Validation
{

	/// <summary>
	/// Enusre is a simple, fluent based, engine usefull to validate
	/// methods and constructors parameters.
	/// </summary>
	public static class Ensure
	{
		internal class SourceInfo
		{
            public readonly static SourceInfo Empty = new SourceInfo("", ""); //, MemberTypes.Custom );

			/// <summary>
			/// Initializes a new instance of the <see cref="SourceInfo"/> class.
			/// </summary>
			/// <param name="methodName">Name of the method.</param>
			/// <param name="className">Name of the class.</param>
			public SourceInfo( String methodName, String className )//, MemberTypes sourceType )
			{
				this.MethodName = methodName;
				this.ClassName = className;
                //this.SourceType = sourceType;
			}

			/// <summary>
			/// Gets the name of the method.
			/// </summary>
			/// <value>The name of the method.</value>
			public String MethodName { get; private set; }

			/// <summary>
			/// Gets the name of the class.
			/// </summary>
			/// <value>The name of the class.</value>
			public String ClassName { get; private set; }

            ///// <summary>
            ///// Gets the type of the source.
            ///// </summary>
            ///// <value>The type of the source.</value>
            //public MemberTypes SourceType { get; private set; }
		}

		/// <summary>
		/// Initialize a new instance of the generic Ensure class using the supplied
		/// value as the value to insepct.
		/// </summary>
		/// <typeparam name="T">The type of the inepcted object value.</typeparam>
		/// <param name="obj">The object value to inspect.</param>
		/// <returns>The Ensure instance for fluent interface usage.</returns>
		public static IConfigurableEnsure<T> That<T>( T obj )
		{
			var si = SourceInfo.Empty;

//#if !SILVERLIGHT

//            var st = new StackTrace( 1 );
//            if( st.FrameCount > 0 )
//            {
//                var f = st.GetFrame( 0 );
//                var mi = f.GetMethod();
//                if( mi != null )
//                {
//                    si = new SourceInfo( mi.Name, mi.DeclaringType.Name, mi.MemberType );
//                }
//            }

//#endif

			return new Ensure<T>( obj, si );
		}
	}
}
