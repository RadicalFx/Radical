using System;

namespace Topics.Radical.Diagnostics
{
	[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = false )]
	public sealed class BuildAttribute : Attribute
	{
		public BuildAttribute( Boolean isOptimized )
		{
			this.IsOptimized = isOptimized;
			this.Version = "0.0.0.0";
		}

		public Boolean IsOptimized { get; private set; }

		public String Version { get; set; }
	}
}
