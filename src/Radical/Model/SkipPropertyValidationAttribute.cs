using System;

namespace Radical.Model
{
    /// <summary>
    /// Applied to properties defines that validation should not be performed for the decorated property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SkipPropertyValidationAttribute : Attribute
    {

    }
}
