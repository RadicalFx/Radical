namespace Topics.Radical
{
    using System;
    using Topics.Radical.ComponentModel;
    using Topics.Radical.Validation;

    public static class KeyExtensions
    {
        public static T ValueOr<T>( this IKey value, T defaultValue ) where T : IComparable, IComparable<T>
        {
            if( value == null )
            {
                return defaultValue;
            }

            Ensure.That( value ).Named( "value" ).IsTrue( obj => obj is IKey<T> );
            
            return value.IsEmpty ? defaultValue : ( ( IKey<T> )value ).Value;
        }
    }
}
