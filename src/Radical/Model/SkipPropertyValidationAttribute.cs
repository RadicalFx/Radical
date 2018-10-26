using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.Model
{
    /// <summary>
    /// Applied to properties asks that models skip this property when notifying property changes.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public sealed class SkipPropertyValidationAttribute : Attribute
    {

    }
}
