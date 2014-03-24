using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Windows.Behaviors
{
	public class TextBoxCommandArgs : EventArgs
	{
		public TextBoxCommandArgs( System.Windows.Input.Key key, System.Windows.Input.ModifierKeys modifiers, Object commandParameter )
		{
			this.Key = key;
			this.Modifiers = modifiers;
			this.CommandParameter = commandParameter;
		}

		public System.Windows.Input.Key Key { get; private set; }
		public System.Windows.Input.ModifierKeys Modifiers { get; private set; }

		public Object CommandParameter { get; private set; }
	}
}
