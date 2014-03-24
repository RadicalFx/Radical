using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Presentation.Focus
{
	[Sample( Title = "Focus behavior", Category = Categories.Behaviors )]
	public class FocusViewModel : SampleViewModel
	{
		public void MoveFocusToName() 
		{
			this.MoveFocusTo( () => this.Name );
		}

		public String Name
		{
			get { return this.GetPropertyValue( () => this.Name ); }
			set { this.SetPropertyValue( () => this.Name, value ); }
		}
	}
}
