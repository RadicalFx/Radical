using System;
using System.Reflection;
using Topics.Radical.Reflection;

namespace Topics.Radical.ComponentModel
{
	public abstract class SampleViewModel : AbstractViewModel
	{
		protected SampleViewModel()
		{
			var attribute = this.GetType().GetAttribute<SampleAttribute>();
			this.Title = attribute.Title;
			this.Description = attribute.Description;
		}

		public String Title { get; private set; }
		public String Description { get; private set; }
	}
}
