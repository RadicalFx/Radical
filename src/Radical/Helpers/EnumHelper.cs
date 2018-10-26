using System;
using System.Linq;
using System.Collections.Generic;
using Radical.DataBinding;
using Radical.Linq;
using System.Reflection;

namespace Radical.Helpers
{
    /// <summary>
    /// Helper class for enumeration types.
    /// </summary>
    public static partial class EnumHelper
    {
        /// <summary>
        /// Given an enuration type, where enuration items are marked with a <see cref="EnumItemDescriptionAttribute"/>,
        /// extracts descriptions.
        /// </summary>
        /// <returns>A readonly list of strings that represents descriptions data.</returns>
        public static IEnumerable<String> ExtractDescriptions<T>()
        {
            List<String> lst = new List<String>();

            EnumHelper.ExtractBindingData<T>()
                .ForEach( eb => lst.Add( eb.Caption ) );

            return lst.AsReadOnly();
        }

        /// <summary>
        /// Extracts the binding data from the supplied enum type.
        /// </summary>
        /// <returns>An readonly list of <see cref="EnumBinder&lt;T&gt;"/> objects.</returns>
        public static IEnumerable<EnumBinder<T>> ExtractBindingData<T>()
        {
            return ExtractBindingData<T>( null );
        }

        /// <summary>
        /// Gets all the values of the given enum type.
        /// </summary>
        /// <typeparam name="T">The type of the enum to extract values from.</typeparam>
        /// <returns>The list of enum values.</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            var values = EnumHelper.GetValues( typeof( T ) );
            return values.Cast<T>();
        }

        /// <summary>
        /// Gets all the values of the given enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>The list of enum values.</returns>
        public static IEnumerable<object> GetValues( Type enumType )
        {
            if( !enumType.IsEnum )
            {
                throw new ArgumentException( "Type '" + enumType.Name + "' is not an enum" );
            }

            var values = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field.GetValue( enumType );

            return values;
        }

#if !SILVERLIGHT
        /// <summary>
        /// Extracts the binding data from the supplied enum type.
        /// </summary>
        /// <typeparam name="T">The enum type to extract binding data from.</typeparam>
        /// <param name="filter">The a predicate that can be used to filer extracted data.</param>
        /// <returns>
        /// An readonly list of <see cref="EnumBinder"/> objects.
        /// </returns>
        public static IEnumerable<EnumBinder<T>> ExtractBindingData<T>( Predicate<T> filter )
        {
            var lst = new List<EnumBinder<T>>();

            foreach( var obj in Enum.GetValues( typeof( T ) ) )
            {
                if( filter == null || filter( ( T )obj ) )
                {
                    var e = ( Enum )obj;
                    EnumItemDescriptionAttribute attribute;
                    if( e.TryGetDescriptionAttribute( out attribute ) )
                    {
                        var eb = new EnumBinder<T>( attribute, ( T )obj );
                        lst.Add( eb );
                    }
                }

            }

            lst.Sort( ( v1, v2 ) => v1.Index.CompareTo( v2.Index ) );

            return lst.AsReadOnly();
        } 
#endif
    }
}
