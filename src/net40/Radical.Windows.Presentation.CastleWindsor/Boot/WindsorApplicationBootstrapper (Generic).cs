using System;
using System.Windows;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class WindsorApplicationBootstrapper<TShellView> : 
		WindsorApplicationBootstrapper 
		where TShellView : Window
	{
		public WindsorApplicationBootstrapper()
		{
			this.UsingAsShell<TShellView>();
		}
	}
}
