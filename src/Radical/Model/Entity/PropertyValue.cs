using System;
namespace Radical.Model
{
    /// <summary>
    /// Identifies a property value.
    /// </summary>
    public abstract class PropertyValue
    {
        /// <summary>
        /// Gets the stored property value.
        /// </summary>
        /// <returns>The stored value.</returns>
        public abstract Object GetValue();
    }
}
