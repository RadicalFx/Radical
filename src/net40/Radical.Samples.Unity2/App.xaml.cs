using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Radical.Samples.Unity2.Presentation;

namespace Radical.Samples.Unity2
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			var b = new Topics.Radical.Windows.Presentation.Boot.UnityApplicationBootstrapper<MainView>();
		}
	}
}
