namespace Radical
{
    using System;
    using System.Linq;
    using Radical.Validation;

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
        ///     <c>true</c> if is the attribute is defined; otherwise, <c>false</c>.
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
    }
}