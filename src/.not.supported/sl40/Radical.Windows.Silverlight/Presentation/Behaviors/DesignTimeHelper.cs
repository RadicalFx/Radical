using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace Topics.Radical.Windows.Behaviors
{
	/// <summary>
	/// An helper class to detect the design mode.
	/// </summary>
	public static class DesignTimeHelper
	{
		/// <summary>
		/// Gets a value indicating whether we are in design mode.
		/// </summary>
		/// <returns><c>True</c> if we are in design mode, otherwise <c>false</c>.</returns>
		public static Boolean GetIsInDesignMode()
		{
			return DesignerProperties.IsInDesignTool;
		}
	}
}