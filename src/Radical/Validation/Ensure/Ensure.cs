﻿using System.Diagnostics;
using System.Reflection;

namespace Radical.Validation
{

    /// <summary>
    /// Ensure is a simple, fluent based, engine useful to validate
    /// methods and constructors parameters.
    /// </summary>
    public static class Ensure
    {
#if DEBUG
        static SourceInfoLoadStrategy _sourceInfoLoadStrategy = SourceInfoLoadStrategy.Load;
#else
        static SourceInfoLoadStrategy _sourceInfoLoadStrategy = SourceInfoLoadStrategy.Skip;
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
            public readonly static SourceInfo Empty = new SourceInfo();

            private readonly StackFrame frame;
            private readonly bool lazy;
            private bool loaded;

            private MemberTypes _sourceType;
            private string _className;
            private string _methodName;

            public static SourceInfo FromStack(StackTrace st, bool lazy)
            {
                SourceInfo si = Empty;
                if (st.FrameCount > 1)
                {
                    si = new SourceInfo(st.GetFrame(1), lazy);
                }

                return si;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SourceInfo"/> class.
            /// </summary>
            private SourceInfo()
            {
                _methodName = "";
                _className = "";
                _sourceType = MemberTypes.Custom;

                /*
                 * this .ctor is called only to create the Empty instance
                 * the loaded == true means that at runtime the "EnsureDataAreLoaded"
                 * method will be skipped since there is nothing to load, this is the
                 * Empty instance in the end :-)
                 * Fixes: https://github.com/RadicalFx/radical/issues/70
                 */
                loaded = true;
            }

            private SourceInfo(StackFrame frame, bool lazy)
            {
                this.frame = frame;
                this.lazy = lazy;
                if (!this.lazy)
                {
                    EnsureDataAreLoaded();
                }
            }

            void EnsureDataAreLoaded()
            {
                if (!loaded)
                {
                    var mi = frame.GetMethod();
                    if (mi != null)
                    {
                        _methodName = mi.Name;
                        _className = mi.DeclaringType.Name;
                        _sourceType = mi.MemberType;
                    }
                    else
                    {
                        _methodName = Empty.MethodName;
                        _className = Empty.ClassName;
                        _sourceType = Empty.SourceType;
                    }

                    loaded = true;
                }
            }

            /// <summary>
            /// Gets the name of the method.
            /// </summary>
            /// <value>The name of the method.</value>
            public string MethodName
            {
                get
                {
                    EnsureDataAreLoaded();
                    return _methodName;
                }
            }

            /// <summary>
            /// Gets the name of the class.
            /// </summary>
            /// <value>The name of the class.</value>
            public string ClassName
            {
                get
                {
                    EnsureDataAreLoaded();
                    return _className;
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
                    EnsureDataAreLoaded();
                    return _sourceType;
                }
            }
        }

        /// <summary>
        /// Initialize a new instance of the generic Ensure class using the supplied
        /// value as the value to inspect.
        /// </summary>
        /// <typeparam name="T">The type of the inspect object value.</typeparam>
        /// <param name="obj">The object value to inspect.</param>
        /// <returns>The Ensure instance for fluent interface usage.</returns>
        public static IConfigurableEnsure<T> That<T>(T obj)
        {
            return That<T>(obj, SourceInfoLoadStrategy);
        }

        /// <summary>
        /// Initialize a new instance of the generic Ensure class using the supplied
        /// value as the value to inspect.
        /// </summary>
        /// <typeparam name="T">The type of the inspect object value.</typeparam>
        /// <param name="obj">The object value to inspect.</param>
        /// <param name="strategy">Determines if the Ensure instance should load the current Stack.</param>
        /// <returns>The Ensure instance for fluent interface usage.</returns>
        public static IConfigurableEnsure<T> That<T>(T obj, SourceInfoLoadStrategy strategy)
        {
            var si = SourceInfo.Empty;

            if (strategy != SourceInfoLoadStrategy.Skip)
            {
                var lazy = strategy == SourceInfoLoadStrategy.LazyLoad;
                si = SourceInfo.FromStack(new StackTrace(1), lazy: lazy);
            }

            return new Ensure<T>(obj, si);
        }
    }

    public enum SourceInfoLoadStrategy
    {
        Load = 0,
        Skip,
        LazyLoad
    }
}
