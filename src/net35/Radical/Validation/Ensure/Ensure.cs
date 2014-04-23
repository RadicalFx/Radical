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
#if DEBUG
		static SourceInfoLoadStrategy _sourceInfoLoadStrategy = SourceInfoLoadStrategy.LoadSourceInfo;
#else
		static SourceInfoLoadStrategy _sourceInfoLoadStrategy = SourceInfoLoadStrategy.LazyLoadSourceInfo;
#endif

		/// <summary>
		/// Determines if the Ensure class tries to load the Stack before moving on.
		/// </summary>
		public static SourceInfoLoadStrategy SourceInfoLoadStrategy
		{
			get { return _sourceInfoLoadStrategy; }
			set { _sourceInfoLoadStrategy = value; }
		}

		internal class SourceInfo
		{
			public readonly static SourceInfo Empty = new SourceInfo( "", "", MemberTypes.Custom );

			private StackFrame frame;
			private bool lazy;
			private bool loaded;

			private MemberTypes _sourceType;
			private string _className;
			private string _methodName;

			public static SourceInfo FromStack( StackTrace st, bool lazy )
			{
				SourceInfo si = SourceInfo.Empty;
				if ( st.FrameCount > 0 )
				{
					var frame = st.GetFrame( 0 );
					si = new SourceInfo( frame, lazy );
				}

				return si;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="SourceInfo"/> class.
			/// </summary>
			/// <param name="methodName">Name of the method.</param>
			/// <param name="className">Name of the class.</param>
			/// <param name="sourceType">Type of the source.</param>
			private SourceInfo( String methodName, String className, MemberTypes sourceType )
			{
				this._methodName = methodName;
				this._className = className;
				this._sourceType = sourceType;
			}

			private SourceInfo( StackFrame frame, bool lazy )
			{
				this.frame = frame;
				this.lazy = lazy;
				if ( !this.lazy )
				{
					this.EnsureDataAreLoaded();
				}
			}

			void EnsureDataAreLoaded()
			{
				if ( !this.loaded )
				{
					var mi = this.frame.GetMethod();
					if ( mi != null )
					{
						this._methodName = mi.Name;
						this._className = mi.DeclaringType.Name;
						this._sourceType = mi.MemberType;
					}

					this.loaded = true;
				}
			}

			/// <summary>
			/// Gets the name of the method.
			/// </summary>
			/// <value>The name of the method.</value>
			public String MethodName 
			{
				get 
				{
					this.EnsureDataAreLoaded();
					return this._methodName; 
				} }

			/// <summary>
			/// Gets the name of the class.
			/// </summary>
			/// <value>The name of the class.</value>
			public String ClassName
			{
				get
				{
					this.EnsureDataAreLoaded();
					return this._className;
				}
			}

			/// <summary>
			/// Gets the type of the source.
			/// </summary>
			/// <value>The type of the source.</value>
			public MemberTypes SourceType
			{
				get
				{
					this.EnsureDataAreLoaded();
					return this._sourceType;
				}
			}
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
			return That<T>( obj, Ensure.SourceInfoLoadStrategy );
		}

		/// <summary>
		/// Initialize a new instance of the generic Ensure class using the supplied
		/// value as the value to insepct.
		/// </summary>
		/// <typeparam name="T">The type of the inepcted object value.</typeparam>
		/// <param name="obj">The object value to inspect.</param>
		/// <param name="strategy">Determines if the Ensure instance should load the current Stack.</param>
		/// <returns>The Ensure instance for fluent interface usage.</returns>
		public static IConfigurableEnsure<T> That<T>( T obj, SourceInfoLoadStrategy strategy )
		{
			var si = SourceInfo.Empty;

#if !SILVERLIGHT

			switch ( strategy )
			{
				case SourceInfoLoadStrategy.LoadSourceInfo:
					si = SourceInfo.FromStack( new StackTrace( 1 ), lazy: false );
					break;

				case SourceInfoLoadStrategy.LazyLoadSourceInfo:
					si = SourceInfo.FromStack( new StackTrace( 1 ), lazy: true );
					break;
			}

#endif

			return new Ensure<T>( obj, si );
		}
	}

	public enum SourceInfoLoadStrategy
	{
		LoadSourceInfo,
		SkipSourceInfoLoad,
		LazyLoadSourceInfo
	}
}
