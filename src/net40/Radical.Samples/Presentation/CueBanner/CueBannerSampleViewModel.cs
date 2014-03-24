using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Presentation.CueBanner
{
	[Sample( Title = "CueBanner behavior", Category = Categories.Behaviors )]
	class CueBannerSampleViewModel : SampleViewModel
	{
		public Boolean IsPasswordBoxVisible
		{
			get { return this.GetPropertyValue( () => this.IsPasswordBoxVisible ); }
			set { this.SetPropertyValue( () => this.IsPasswordBoxVisible, value ); }
		}

		public Boolean IsTextBoxVisible
		{
			get { return this.GetPropertyValue( () => this.IsTextBoxVisible ); }
			set { this.SetPropertyValue( () => this.IsTextBoxVisible, value ); }
		}
	}
}
