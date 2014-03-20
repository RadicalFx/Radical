using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Topics.Radical.DataBinding;

namespace Topics.Radical.Helpers
{
	static partial class EnumHelper
	{
		/// <summary>
		/// Extracts the binding data from the supplied enum type.
		/// </summary>
		/// <typeparam name="T">The enum type to extract binding data from.</typeparam>
		/// <param name="filter">The a predicate that can be used to filer extracted data.</param>
		/// <returns>
		/// An readonly list of generic EnumBinder objects.
		/// </returns>
		public static IEnumerable<EnumBinder<T>> ExtractBindingData<T>( Predicate<T> filter )
		{
			var lst = new List<EnumBinder<T>>();

			foreach( var obj in EnumHelper.GetValues( typeof( T ) ) )
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
	}
}
