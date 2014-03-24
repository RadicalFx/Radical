using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;
using Topics.Radical.Windows.Presentation;

namespace Topics.Radical.Presentation.Commanding
{
	[Sample( Title = "Auto Command Binding (Nested source)", Category = Categories.Commanding )]
	public class AutoCommandBindingWithNestedSourceViewModel : SampleViewModel
	{
		public AutoCommandBindingWithNestedSourceViewModel()
		{
		
		}
	}

	public class NestedSource : AbstractViewModel
	{
		public long CalledOn
		{
			get { return this.GetPropertyValue( () => this.CalledOn ); }
			private set { this.SetPropertyValue( () => this.CalledOn, value ); }
		}

		public void Call()
		{
			this.CalledOn = DateTime.Now.Ticks;
		}
	}
}
