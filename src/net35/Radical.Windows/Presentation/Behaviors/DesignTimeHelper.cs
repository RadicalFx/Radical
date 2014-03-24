using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace Topics.Radical.Windows.Behaviors
{
	public static class DesignTimeHelper
	{
		/// <summary>
		/// Gets a value indicating whether we are in design mode.
		/// </summary>
		/// <returns><c>True</c> if we are in design mode, otherwise <c>false</c>.</returns>
		public static Boolean GetIsInDesignMode()
		{
			var descriptor = DependencyPropertyDescriptor.FromProperty(
				DesignerProperties.IsInDesignModeProperty,
				typeof( DependencyObject ) );

			var isInDesignMode = ( Boolean )descriptor.Metadata.DefaultValue;

			return isInDesignMode;
		}
	}
}
