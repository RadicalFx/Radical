using System;
using System.Reflection;
using Topics.Radical.ComponentModel.Validation;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Presentation;

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
