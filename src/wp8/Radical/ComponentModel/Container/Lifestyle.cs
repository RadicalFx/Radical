using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Defines the lifestyle of a component.
	/// </summary>
	public enum Lifestyle
	{
		/// <summary>
		/// Singleton lifestyle.
		/// </summary>
		Singleton = 0,

		/// <summary>
		/// Transient lifestyle.
		/// </summary>
		Transient
	}	
}
