using System;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Applied to a ViewModel issues automatically a ViewModelActivated message.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
    public class NotifyActivatedAttribute : Attribute
	{
	}
}
