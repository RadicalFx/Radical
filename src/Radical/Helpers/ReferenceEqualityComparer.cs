namespace Radical
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Radical.Validation;

    sealed class ReferenceEqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
        where T : class
    {
        public ReferenceEqualityComparer()
        {

        }

        public Boolean Equals( T x, T y )
        {
            return Object.Equals( x, y );
        }

        Boolean IEqualityComparer.Equals( Object x, Object y )
        {
            return Object.Equals( x, y );
        }

        int IEqualityComparer.GetHashCode( object obj )
        {
            Ensure.That( obj ).Named( "obj" ).IsNotNull();

            return obj.GetHashCode();
        }

        int IEqualityComparer<T>.GetHashCode( T obj )
        {
            Ensure.That( obj ).Named( "obj" ).IsNotNull();

            return obj.GetHashCode();
        }
    }
}
