//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Topics.Radical
//{
//    /// <summary>
//    /// Represents a facade to abstract the interface of a dependency container based on delegates.
//    /// </summary>
//    public class DelegateDependencyResolver : ComponentModel.IResolveDependencies
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="DelegateDependencyResolver"/> class.
//        /// </summary>
//        public DelegateDependencyResolver()
//        {
//            this.OnResolveAll = type => new Object[ 0 ];
//            this.OnResolve = type => { throw new InvalidOperationException( "Dependency not found." ); };
//        }

//        /// <summary>
//        /// Gets or sets the delegate used to resolve a set of dependencies.
//        /// </summary>
//        /// <value>
//        /// The resolve all.
//        /// </value>
//        public Func<Type, IEnumerable<Object>> OnResolveAll
//        {
//            get;
//            set;
//        }

//        /// <summary>
//        /// Gets or sets the delegate used to resolve a single dependency.
//        /// </summary>
//        /// <value>
//        /// The resolve delegate.
//        /// </value>
//        public Func<Type, Object> OnResolve
//        {
//            get;
//            set;
//        }

//        /// <summary>
//        /// Resolves all the dependencies identified by the given type.
//        /// </summary>
//        /// <typeparam name="T">The type of the dependency.</typeparam>
//        /// <returns></returns>
//        public IEnumerable<T> ResolveAll<T>()
//        {
//            return this.OnResolveAll( typeof( T ) ).OfType<T>();
//        }

//        /// <summary>
//        /// Resolves the dependency identified by the given type.
//        /// </summary>
//        /// <typeparam name="T">The type of the dependency.</typeparam>
//        /// <returns></returns>
//        public T Resolve<T>()
//        {
//            return ( T )this.OnResolve( typeof( T ) );
//        }

//        /// <summary>
//        /// Resolves all the dependencies identified by the given type.
//        /// </summary>
//        /// <param name="t">The type of the dependency.</param>
//        /// <returns></returns>
//        public IEnumerable<object> ResolveAll( Type t )
//        {
//            return this.OnResolveAll( t );
//        }

//        /// <summary>
//        /// Resolves the dependency identified by the given type.
//        /// </summary>
//        /// <param name="t">The type of the dependency.</param>
//        /// <returns></returns>
//        public object Resolve( Type t )
//        {
//            return this.OnResolve( t );
//        }
//    }
//}
