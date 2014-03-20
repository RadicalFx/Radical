using System;
using System.Windows;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Presentation.Boot
{
	public class PuzzleApplicationBootstrapper<TShellView> : 
		PuzzleApplicationBootstrapper 
		where TShellView : UIElement
	{
		public PuzzleApplicationBootstrapper()
		{
			this.DefineHomeAs<TShellView>();
		}
	}
}
