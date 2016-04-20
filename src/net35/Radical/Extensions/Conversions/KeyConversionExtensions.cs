namespace Topics.Radical.Conversions
{
    using System;
    using Topics.Radical.ComponentModel;
    using Topics.Radical.Validation;

    public static class KeyConversionExtensions
    {
        public static IKey<T> AsKey<T>( this T value ) where T : IComparable, IComparable<T>
        {
            return new Key<T>( value );
        }

        public static T AsValue<T>( this IKey value ) where T : IComparable, IComparable<T>
        {
            Ensure.That( value ).Named( "value" ).IsTrue( obj => obj is IKey<T> );

            return ( ( IKey<T> )value ).Value;
        }
    }
}
