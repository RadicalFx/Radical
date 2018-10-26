using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Radical.ComponentModel;
using Radical.Linq;
using Radical.Validation;
using System.Linq.Expressions;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        object IServiceProvider.GetService( Type service )
        {
            if( this.site != null )
            {
                this.EnsureNotDisposed();
                return this.site.GetService( service );
            }

            return null;
        }
    }
}