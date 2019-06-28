using System;

namespace Radical.Diagnostics
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class BuildAttribute : Attribute
    {
        public BuildAttribute(bool isOptimized)
        {
            this.IsOptimized = isOptimized;
            this.Version = "0.0.0.0";
        }

        public bool IsOptimized { get; private set; }

        public string Version { get; set; }
    }
}
