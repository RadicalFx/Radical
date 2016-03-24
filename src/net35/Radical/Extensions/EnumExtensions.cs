namespace Topics.Radical
{
	using System;
	using System.Linq;
	using Topics.Radical.Validation;

	/// <summary>
	/// Helper class that adds functionalities for enumerative types.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Calls FCL Enum.IsDefined and if the result is false throws
		/// an ArgumentException.
		/// </summary>
		/// <exception cref="ArgumentException">If FCL Enum.IsDefined returns false</exception>
		public static void EnsureIsDefined( this Enum value )
		{
			var enumType = value.GetType();
			if( !Enum.IsDefined( enumType, value ) )
			{
				throw new EnumValueOutOfRangeException( Resources.Exceptions.EnumValidatorNotDefinedException );
			}
		}

		/// <summary>
		/// Calls FCL Enum.IsDefined and returns the result.
		/// </summary>
		public static Boolean IsDefined( this Enum value )
		{
			var enumType = value.GetType();
			return Enum.IsDefined( enumType, value );
		}

		/// <summary>
		/// Determines whether <see cref="EnumItemDescriptionAttribute"/> is defined on the specified value.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <returns>
		/// 	<c>true</c> if is the attribute is defined; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsDescriptionAttributeDefined( this Enum value )
		{
			value.EnsureIsDefined();

			var enumType = value.GetType();
			var field = enumType.GetField( value.ToString() );
			var attributes = field.GetCustomAttributes( false );

			return attributes
				.OfType<EnumItemDescriptionAttribute>()
				.Any();
		}

		/// <summary>
		/// Gets the description attribute applied on the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>An instance of the <see cref="EnumItemDescriptionAttribute"/>.</returns>
		/// <exception cref="ArgumentException">No <see cref="EnumItemDescriptionAttribute"/> has been defined on the given enum value.</exception>
		public static EnumItemDescriptionAttribute GetDescriptionAttribute( this Enum value )
		{
			if( !value.IsDescriptionAttributeDefined() )
			{
				throw new ArgumentException( Resources.Exceptions.ExtractDescriptionMissingAttributeException );
			}

			return value.GetDescriptionAttributeCore();
		}

		static EnumItemDescriptionAttribute GetDescriptionAttributeCore( this Enum value )
		{
			Type enumType = value.GetType();

			var field = enumType.GetField( value.ToString() );
			var attributes = field.GetCustomAttributes( typeof( EnumItemDescriptionAttribute ), false );
			return ( EnumItemDescriptionAttribute )attributes[ 0 ];
		}

		/// <summary>
		/// Gets the description string holded by the <see cref="EnumItemDescriptionAttribute"/> applied to the given enumaration value.
		/// </summary>
		/// <param name="value">The enumeration value to search the attribute on.</param>
		/// <param name="attribute">The <see cref="EnumItemDescriptionAttribute"/> applied to given enumeration value.</param>
		/// <returns><c>True</c> if the operation has been successfully completed, otherwise <c>false</c>.</returns>
		public static Boolean TryGetDescriptionAttribute( this Enum value, out EnumItemDescriptionAttribute attribute )
		{
			value.EnsureIsDefined();

			if( value.IsDescriptionAttributeDefined() )
			{
				attribute = value.GetDescriptionAttributeCore();
			}
			else
			{
				attribute = null;
			}

			return attribute != null;
		}

		/// <summary>
		/// Gets the caption string holded by the <see cref="EnumItemDescriptionAttribute"/> 
		/// applied to the given enumaration value.
		/// </summary>
		/// <param name="value">The value to extract caption from.</param>
		/// <returns>The value applied to the <c>Caption</c> property of the <see cref="EnumItemDescriptionAttribute"/>.</returns>
		/// <exception cref="ArgumentException">No <see cref="EnumItemDescriptionAttribute"/> has been defined on the given enum value.</exception>
		public static String GetCaption( this Enum value )
		{
			var attribute = value.GetDescriptionAttribute();
			return attribute.Caption;
		}

		/// <summary>
		/// Gets the description string holded by the <see cref="EnumItemDescriptionAttribute"/> 
		/// applied to the given enumaration value.
		/// </summary>
		/// <param name="value">The value to extract description from.</param>
		/// <returns>The value applied to the <c>Description</c> property of the <see cref="EnumItemDescriptionAttribute"/>.</returns>
		/// <exception cref="ArgumentException">No <see cref="EnumItemDescriptionAttribute"/> has been defined on the given enum value.</exception>
		public static String GetDescription( this Enum value )
		{
			var attribute = value.GetDescriptionAttribute();
			return attribute.Description;
		}

        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <param name="variable">Flags enumeration to check</param>
        /// <param name="value">Flag to check for</param>
		/// <returns>
		/// 	<c>true</c> if the bit field or bit fields that are set in flag are also set in the current instance; otherwise, <c>false</c>.
		/// </returns>
        public static bool HasFlag(this Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException("value");

            if (!Enum.IsDefined(variable.GetType(), value))
            {
                throw new ArgumentException(string.Format(
                    "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                    value.GetType(), variable.GetType()));
            }

            ulong num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);
        }
    }
}