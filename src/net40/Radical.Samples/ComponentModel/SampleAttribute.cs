using System;

namespace Topics.Radical.ComponentModel
{
	[AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
	public sealed class SampleAttribute : Attribute
	{
		public SampleAttribute()
		{
			this.Category = Categories.Default;
			this.Description = "<-- not set -->";
		}

		public String PageUri { get; set; }
		public String Title { get; set; }
		public String Category { get; set; }
		public String Description { get; set; }
		public Int32 Order { get; set; }
		//public IEnumerable<String> Tags { get; set; }
	}
}
