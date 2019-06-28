using Radical.Reflection;
using System;
using System.ComponentModel;

namespace Radical.Validation
{
    public class ValidationTools
    {
        /// <summary>
        /// Gets the display name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public string GetPropertyDisplayName( string propertyName, Object entity )
        {
            var displayName = propertyName;

            //Duplicate code in ValidatorBase
            var pi = entity.GetType().GetProperty( propertyName );
            if ( pi != null && pi.IsAttributeDefined<DisplayNameAttribute>() )
            {
                var a = pi.GetAttribute<DisplayNameAttribute>();
                displayName = a.DisplayName;
            }

            return displayName;
        }
    }
}
