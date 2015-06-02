using System;
using System.Windows;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class AutofacApplicationBootstrapper<TShellView> :
		AutofacApplicationBootstrapper 
		where TShellView : Window
	{
		public AutofacApplicationBootstrapper()
		{
			this.UsingAsShell<TShellView>();
		}
	}
}
