using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Topics.Radical.Reflection;

namespace Topics.Radical.Validation
{
	/// <summary>
	/// Extends the generic Ensure class to add behaviors when the inspected object
	/// is an <c>Enum</c>.
	/// </summary>
	public static class EnumEnsureExtensions
	{
		/// <summary>
		/// Determines whether the specified enumeration value is defined.
		/// </summary>
		/// <typeparam name="T">THe type of the enumeration to inspect.</typeparam>
		/// <param name="validator">The current validator.</param>
		/// <returns>
		/// The Ensure instance for fluent interface usage.
		/// </returns>
		/// <exception cref="NotSupportedException">A <c>NotSupportedException</c> is raised if the supplied type T is not an enum type.</exception>
		/// <exception cref="EnumValueOutOfRangeException">An <c>EnumValueOutOfRangeException</c>
		/// is raised if the supplied enum value is not defined.</exception>
		public static IEnsure<T> IsDefined<T>( this IEnsure<T> validator )
		{
			if( !typeof( T ).Is<Enum>() )
			{
				throw new NotSupportedException( "Only enum types are supported by this ensure extension" );
			}

			var enumType = validator.Value.GetType();
			if( !Enum.IsDefined( enumType, validator.Value ) )
			{
				validator.Throw( new EnumValueOutOfRangeException( "missing::EnumValidatorNotDefinedException" ) );
			}

			return validator;
		}
	}
}
