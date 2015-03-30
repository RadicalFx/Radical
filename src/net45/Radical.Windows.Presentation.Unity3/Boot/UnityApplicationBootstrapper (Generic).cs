using System;
using System.Windows;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class UnityApplicationBootstrapper<TShellView> : 
		UnityApplicationBootstrapper 
		where TShellView : Window
	{
		public UnityApplicationBootstrapper()
		{
			this.UsingAsShell<TShellView>();
		}
	}
}
