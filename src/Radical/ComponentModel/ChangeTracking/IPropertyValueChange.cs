using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.ComponentModel.ChangeTracking
{
    /// <summary>
    /// Identifies a change occurred to a property.
    /// </summary>
    public interface IPropertyValueChange : IChange
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        String PropertyName { get; }
    }
}
