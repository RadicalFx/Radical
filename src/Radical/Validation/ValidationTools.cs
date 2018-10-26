using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Radical.Reflection;

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
        public String GetPropertyDisplayName( String propertyName, Object entity )
        {
            var displayName = propertyName;

#if FX35 || FX40 || FX45

            //Duplicate code in ValidatorBase
            var pi = entity.GetType().GetProperty( propertyName );
            if ( pi != null && pi.IsAttributeDefined<DisplayNameAttribute>() )
            {
                var a = pi.GetAttribute<DisplayNameAttribute>();
                displayName = a.DisplayName;
            }
#endif

            return displayName;
        }
    }
}
