using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Windows.Markup
{
	public class ChoosenItemBinding : BindingDecoratorBase
	{
		public ChoosenItemBinding()
			: base()
		{
			this.InitDefaults();
		}

		public ChoosenItemBinding( String path )
			: base( path )
		{
			this.InitDefaults();
		}

		void InitDefaults()
		{
			this.Mode = System.Windows.Data.BindingMode.OneWayToSource;
			this.UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged;
		}
	}
}
