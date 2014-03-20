using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Topics.Radical.ComponentModel.Windows.Input;
using System.Windows.Data;

namespace Topics.Radical.Windows.Markup
{
	public class EditorBinding : BindingDecoratorBase
	{
		public EditorBinding()
			: base()
		{
			this.InitDefaults();
		}

		public EditorBinding( String path )
			: base( path )
		{
			this.InitDefaults();
		}

		void InitDefaults()
		{
			this.NotifyOnValidationError = true;
			this.ValidatesOnDataErrors = true;
			this.ValidatesOnExceptions = true;
			this.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
		}
	}
}