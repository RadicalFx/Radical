using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Presentation.Effects
{
	[Sample( Title = "Grayscale Shader", Category = Categories.Presentation )]
	public class EffectViewModel : SampleViewModel
	{
		public EffectViewModel()
		{
			this.SetInitialPropertyValue( () => this.IsChecked, true );
		}

		public Boolean IsChecked
		{
			get { return this.GetPropertyValue( () => this.IsChecked ); }
			set { this.SetPropertyValue( () => this.IsChecked, value ); }
		}
	}
}
